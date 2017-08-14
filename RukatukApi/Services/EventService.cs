using FlickrNet;
using Microsoft.Azure.WebJobs.Host;
using RukatukApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RukatukApi.Services
{
    public class EventService : IEventService
    {
        private static readonly HashSet<string> CancelledEvents = new HashSet<string>(new[] { "27361536091" });

        private readonly IEventbriteClient _eventbriteClient;
        private readonly Flickr _flickrClient;
        private readonly IConfiguration _configuration;
        private readonly IEventRepository _eventRepository;

        public EventService(
            IEventbriteClient eventbriteClient,
            Flickr flickrClient,
            IConfiguration configuration,
            IEventRepository eventRepository)
        {
            _eventbriteClient = eventbriteClient;
            _flickrClient = flickrClient;
            _configuration = configuration;
            _eventRepository = eventRepository;
        }

        public async Task UpdateEventsAsync(TraceWriter log, CancellationToken cancellationToken)
        {
            var getEventsTask = _eventbriteClient.GetEventsAsync(log, cancellationToken);
            var getVenuesTask = _eventbriteClient.GetVenuesAsync(log, cancellationToken);
            var getCountriesTask = _eventbriteClient.GetCountriesAsync(log, cancellationToken);
            var getPhotosTask = GetPhotosAsync(cancellationToken);

            await Task.WhenAll(getEventsTask, getVenuesTask, getCountriesTask, getPhotosTask);

            var eventbriteEvents = getEventsTask.Result;
            var venuesLookup = getVenuesTask.Result.ToDictionary(v => v.Id);
            var countriesLookup = getCountriesTask.Result.ToDictionary(c => c.Code);
            var photos = getPhotosTask.Result;

            var ticketClassRequests = eventbriteEvents.Select(e => _eventbriteClient.GetTicketClassesAsync(e.Id, log, cancellationToken));
            var ticketClassLookup = (await Task.WhenAll(ticketClassRequests))
                                        .SelectMany(t => t)
                                        .GroupBy(tc => tc.EventId)
                                        .ToDictionary(grp => grp.Key, grp => grp.ToList());

            var events = eventbriteEvents
                            .Where(e => !CancelledEvents.Contains(e.Id))
                            .Select(e => CreateEvent(e, venuesLookup, ticketClassLookup, countriesLookup, photos))
                            .ToList();

            log.Info($"Upserting {events.Count} events...");

            await _eventRepository.UpsertEventsAsync(events, cancellationToken);
        }

        public Task<IReadOnlyList<Event>> GetEventsAsync(CancellationToken cancellationToken)
        {
            return _eventRepository.GetEventsAsync(cancellationToken);
        }

        private Task<PhotoLookup> GetPhotosAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var largePhotos = new Dictionary<string, string>();
                var smallPhotos = new Dictionary<string, string>();
                var photoSet = _flickrClient.PhotosetsGetPhotos(_configuration.FlickrPhotoSetId);
                foreach (var photo in photoSet)
                {
                    var tokens = photo.Title?.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tokens?.Length != 2)
                    {
                        continue;
                    }

                    var eventId = tokens[0];
                    Dictionary<string, string> lookup;
                    string photoUrl;
                    if (tokens[1] == "large" || tokens[1] == "rect")
                    {
                        lookup = largePhotos;
                        photoUrl = photo.Large1600Url ?? photo.LargeUrl;
                    }
                    else
                    {
                        lookup = smallPhotos;
                        photoUrl = photo.Small320Url ?? photo.SmallUrl;
                    }

                    lookup.Add(eventId, photo.WebUrl);
                }

                return new PhotoLookup(largePhotos, smallPhotos);
            },
            cancellationToken);
        }

        private Event CreateEvent(
            EventbriteEvent @event,
            IReadOnlyDictionary<string, EventbriteVenue> venueLookup,
            IReadOnlyDictionary<string, List<EventbriteTicketClass>> ticketClassLookup,
            IReadOnlyDictionary<string, EventbriteCountry> countriesLookup,
            PhotoLookup photos)
        {
            EventbriteVenue venue;
            if (!venueLookup.TryGetValue(@event.VenueId, out venue))
            {
                throw new InvalidOperationException(
                    $"Couldn't find Venue {@event.VenueId} for Event {@event.Id}");
            }

            EventbriteCountry country;
            if (!countriesLookup.TryGetValue(venue.Address.CountryCode, out country))
            {
                throw new InvalidOperationException(
                    $"Couldn't find Country {venue.Address.CountryCode} for Venue {venue.Name}");
            }

            List<EventbriteTicketClass> ticketClasses;
            if (!ticketClassLookup.TryGetValue(@event.Id, out ticketClasses))
            {
                ticketClasses = new List<EventbriteTicketClass>();
            }

            var minTicketPrice = ticketClasses.Where(tc => tc.IsFree != true)
                                              .OrderBy(tc => tc.ActualCost?.Value ?? int.MaxValue)
                                              .Select(tc => tc.ActualCost?.Display)
                                              .FirstOrDefault();
            var imageUrl = GetPhotoUrl(@event, photos.Large);
            var squareImageUrl = GetPhotoUrl(@event, photos.Small);

            return new Event
            {
                Id = @event.Id,
                Name = @event.Name.Text,
                Status = @event.Status,
                DescriptionText = @event.Description.Text,
                DescriptionHtml = @event.Description.Html,
                CreatedDate = @event.Created,
                StartDate = ConvertDate(@event.Start),
                EndDate = ConvertDate(@event.End),
                VanityUrl = @event.VanityUrl,
                ImageUrl = imageUrl,
                SquareImageUrl = squareImageUrl,
                MinTicketPrice = minTicketPrice,
                Venue = new Venue
                {
                    Name = venue.Name,
                    AddressLine1 = venue.Address.Address1,
                    AddressLine2 = venue.Address.Address2,
                    City = venue.Address.City,
                    PostalCode = venue.Address.PostalCode,
                    Country = country.Label,
                    Latitude = venue.Address.Latitude,
                    Longitude = venue.Address.Longitude
                }
            };
        }

        private string GetPhotoUrl(EventbriteEvent @event, IReadOnlyDictionary<string, string> lookup)
        {
            return lookup.TryGetValue(@event.Id, out string photoUrl) ? photoUrl : @event.Logo?.Original?.Url;
        }

        private DateTimeOffset ConvertDate(EventbriteDate date)
        {
            return new DateTimeOffset(date.Local, date.Local - date.Utc);
        }

        private class PhotoLookup
        {
            public PhotoLookup(
                IReadOnlyDictionary<string, string> large,
                IReadOnlyDictionary<string, string> small)
            {
                Large = large;
                Small = small;
            }

            public IReadOnlyDictionary<string, string> Large { get; }
            public IReadOnlyDictionary<string, string> Small { get; }
        }
    }
}
