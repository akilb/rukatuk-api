using Microsoft.Azure.WebJobs.Host;
using RukatukApi.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RukatukApi.Services
{
    public interface IEventService
    {
        Task UpdateEventsAsync(TraceWriter log, CancellationToken cancellationToken);
        Task<IReadOnlyList<Event>> GetEventsAsync(CancellationToken cancellationToken);
    }
}
