using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using RukatukApi.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RukatukApi.Services
{
    public class AzureStorageEventRepository : IEventRepository
    {
        private readonly CloudBlobClient _blobClient;

        public AzureStorageEventRepository(IConfiguration configuration)
        {
            var storageAccount = CloudStorageAccount.Parse(configuration.AzureStorageConnectionString);
            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        public async Task<IReadOnlyList<Event>> GetEventsAsync(CancellationToken cancellationToken)
        {
            var blockBlob = await GetEventsJsonBlob(cancellationToken);
            var eventsJson = await blockBlob.DownloadTextAsync(cancellationToken);

            var events = JsonConvert.DeserializeObject<Event[]>(eventsJson);
            return events;
        }

        public async Task UpsertEventsAsync(IReadOnlyList<Event> events, CancellationToken cancellationToken)
        {
            var blockBlob = await GetEventsJsonBlob(cancellationToken);

            var eventsJson = JsonConvert.SerializeObject(events);
            await blockBlob.UploadTextAsync(eventsJson, cancellationToken);
        }

        private async Task<CloudBlockBlob> GetEventsJsonBlob(CancellationToken cancellationToken)
        {

            var container = _blobClient.GetContainerReference("rukatuk-api");
            await container.CreateIfNotExistsAsync(cancellationToken);

            var blockBlob = container.GetBlockBlobReference("events.json");
            return blockBlob;
        }
    }
}
