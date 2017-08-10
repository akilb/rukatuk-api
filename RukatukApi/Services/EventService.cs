using Microsoft.Azure.WebJobs.Host;
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
            var events = await _eventbriteClient.GetEventsAsync(log, cancellationToken);
        }
    }
}
