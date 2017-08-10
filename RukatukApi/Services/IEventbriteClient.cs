using Microsoft.Azure.WebJobs.Host;
using RukatukApi.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RukatukApi.Services
{
    public interface IEventbriteClient
    {
        Task<IReadOnlyList<EventbriteEvent>> GetEventsAsync(TraceWriter log, CancellationToken cancellationToken);
        Task<IReadOnlyList<EventbriteVenue>> GetVenuesAsync(TraceWriter log, CancellationToken cancellationToken);

        Task<IReadOnlyList<EventbriteTicketClass>> GetTicketClassesAsync(string eventId, TraceWriter log, CancellationToken cancellationToken);
        Task<IReadOnlyList<EventbriteCountry>> GetCountriesAsync(TraceWriter log, CancellationToken cancellationToken);
    }
}
