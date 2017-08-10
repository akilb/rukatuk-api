using Microsoft.Azure.WebJobs.Host;
using System.Threading;
using System.Threading.Tasks;

namespace RukatukApi.Services
{
    public interface IEventService
    {
        Task UpdateEventsAsync(TraceWriter log, CancellationToken cancellationToken);
    }
}
