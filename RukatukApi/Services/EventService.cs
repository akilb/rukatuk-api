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
        private readonly IEventbriteClient _eventbriteClient;

        public EventService(IEventbriteClient eventbriteClient)
        {
            _eventbriteClient = eventbriteClient;
        }

        public async Task UpdateEventsAsync(TraceWriter log, CancellationToken cancellationToken)
        {
            var getEventsTask = _eventbriteClient.GetEventsAsync(log, cancellationToken);
            var getVenuesTask = _eventbriteClient.GetVenuesAsync(log, cancellationToken);
            var getCountriesTask = _eventbriteClient.GetCountriesAsync(log, cancellationToken);

            await Task.WhenAll(getEventsTask, getVenuesTask, getCountriesTask);

            var eventbriteEvents = getEventsTask.Result;
            var venuesLookup = getVenuesTask.Result.ToDictionary(v => v.Id);
            var countriesLookup = getCountriesTask.Result.ToDictionary(c => c.Code);

            var ticketClassRequests = eventbriteEvents.Select(e => _eventbriteClient.GetTicketClassesAsync(e.Id, log, cancellationToken));
            var ticketClassLookup = (await Task.WhenAll(ticketClassRequests))
                                        .SelectMany(t => t)
                                        .GroupBy(tc => tc.EventId)
                                        .ToDictionary(grp => grp.Key, grp => grp.ToList());

            var events = eventbriteEvents.Select(e => CreateEvent(e, venuesLookup, ticketClassLookup, countriesLookup)).ToList();
        }

        private Event CreateEvent(
            EventbriteEvent @event,
            IReadOnlyDictionary<string, EventbriteVenue> venueLookup,
            IReadOnlyDictionary<string, List<EventbriteTicketClass>> ticketClassLookup,
            IReadOnlyDictionary<string, EventbriteCountry> countriesLookup)
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
                ImageUrl = @event.Logo?.Original?.Url,
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

        private DateTimeOffset ConvertDate(EventbriteDate date)
        {
            return new DateTimeOffset(date.Local, date.Local - date.Utc);
        }
    }
}
