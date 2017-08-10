using Microsoft.Azure.WebJobs.Host;
using RukatukApi.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace RukatukApi.Services
{
    public class EventbriteClient : IEventbriteClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public EventbriteClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public Task<IReadOnlyList<EventbriteEvent>> GetEventsAsync(
            TraceWriter log,
            CancellationToken cancellationToken)
        {
            return GetPaginatedDataAsync<EventbriteEvent, EventbriteEventsPage>(
                "users/me/owned_events/?status=live,started,ended&order_by=start_desc",
                p => p.Events,
                cancellationToken);
        }

        public Task<IReadOnlyList<EventbriteVenue>> GetVenuesAsync(
            TraceWriter log,
            CancellationToken cancellationToken)
        {
            return GetPaginatedDataAsync<EventbriteVenue, EventbriteVenuesPage>(
                "users/me/venues/",
                p => p.Venues,
                cancellationToken);
        }

        public Task<IReadOnlyList<EventbriteTicketClass>> GetTicketClassesAsync(
            string eventId,
            TraceWriter log,
            CancellationToken cancellationToken)
        {
            return GetPaginatedDataAsync<EventbriteTicketClass, EventbriteTicketClassesPage>(
                $"events/{eventId}/ticket_classes/",
                p => p.TicketClasses,
                cancellationToken);
        }

        public Task<IReadOnlyList<EventbriteCountry>> GetCountriesAsync(
            TraceWriter log,
            CancellationToken cancellationToken)
        {
            return GetPaginatedDataAsync<EventbriteCountry, EventbriteCountriesPage>(
                "system/countries/",
                p => p.Countries,
                cancellationToken);
        }

        private async Task<IReadOnlyList<T>> GetPaginatedDataAsync<T, TPage>(
            string relativeUrl,
            Func<TPage, IList<T>> getItemsFunc,
            CancellationToken cancellationToken) where TPage : EventbritePage
        {
            var items = new List<T>();
            var hasMorePages = true;
            var page = 1;
            while (hasMorePages)
            {
                var pageQueryParamSeparator = relativeUrl.Contains("?") ? "&" : "?";
                var response = await GetAsync<TPage>(
                                    $"{relativeUrl}{pageQueryParamSeparator}page={page}",
                                    cancellationToken);

                // TODO: Handle API error
                if (response == null)
                {
                    hasMorePages = false;
                    continue;
                }

                items.AddRange(getItemsFunc(response));

                var pagination = response.Pagination;
                hasMorePages = pagination.PageNumber < pagination.PageCount;
                page++;
            }

            return items;
        }

        private async Task<T> GetAsync<T>(string relativeUrl, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"https://www.eventbriteapi.com/v3/{relativeUrl}"));
            request.Headers.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("rukatuk-api", "1.0")));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _configuration.EventbriteOAuthToken);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return default(T);
            }

            var result = await response.Content.ReadAsAsync<T>(cancellationToken);
            return result;
        }

        [DataContract]
        public class EventbriteEventsPage : EventbritePage
        {
            [DataMember(Name = "events")]
            public IList<EventbriteEvent> Events { get; set; }
        }

        [DataContract]
        public class EventbriteVenuesPage : EventbritePage
        {
            [DataMember(Name = "venues")]
            public IList<EventbriteVenue> Venues { get; set; }
        }

        [DataContract]
        public class EventbriteTicketClassesPage : EventbritePage
        {
            [DataMember(Name = "ticket_classes")]
            public IList<EventbriteTicketClass> TicketClasses { get; set; }
        }

        [DataContract]
        public class EventbriteCountriesPage : EventbritePage
        {
            [DataMember(Name = "countries")]
            public IList<EventbriteCountry> Countries { get; set; }
        }

        [DataContract]
        public class EventbritePage
        {
            [DataMember(Name = "pagination")]
            public EventbritePaginationInformation Pagination { get; set; }
        }

        [DataContract]
        public class EventbritePaginationInformation
        {
            [DataMember(Name = "page_number")]
            public int? PageNumber { get; set; }

            [DataMember(Name = "page_count")]
            public int? PageCount { get; set; }
        }
    }
}
