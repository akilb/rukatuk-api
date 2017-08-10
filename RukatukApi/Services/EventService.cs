using Microsoft.Azure.WebJobs.Host;
using System.Threading;
using System.Threading.Tasks;

namespace RukatukApi.Services
{
    public class EventService : IEventService
    {
        public Task UpdateEventsAsync(TraceWriter log, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
