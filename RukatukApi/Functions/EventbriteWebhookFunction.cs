using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using RukatukApi.Models;

namespace RukatukApi.Functions
{
    public static class EventbriteWebhookFunction
    {
        [FunctionName("eventchanges")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req,
            TraceWriter log)
        {
            // Get request body
            var payload = await req.Content.ReadAsAsync<EventbriteWebhookPayload>();

            log.Info(
                $"Webhook {payload?.Config?.WebhookId} triggered. Action: {payload?.Config?.Action}. " +
                $"ApiUrl: {payload?.ApiUrl}");

            // TODO: everything

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}