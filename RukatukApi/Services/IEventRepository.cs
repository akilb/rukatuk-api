using RukatukApi.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RukatukApi.Services
{
    public interface IEventRepository
    {
        Task UpsertEventsAsync(IReadOnlyList<Event> events, CancellationToken cancellationToken);
    }
}
