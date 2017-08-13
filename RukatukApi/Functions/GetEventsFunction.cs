using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Autofac;
using RukatukApi.Services;
using RukatukApi.IOC;
using System.Threading;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Net.Http.Headers;
using System;

namespace RukatukApi.Functions
{
    public static class GetEventsFunction
    {
        private static readonly IEventService _eventService = Container.Instance.Resolve<IEventService>();

        [FunctionName("events")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequestMessage req,
            TraceWriter log,
            CancellationToken cancellationToken)
        {
            var events = await _eventService.GetEventsAsync(cancellationToken);

            var json = JsonConvert.SerializeObject(
                        events,
                        Formatting.None,
                        new JsonSerializerSettings {DateFormatHandling = DateFormatHandling.IsoDateFormat});

            var response = req.CreateResponse(
                            HttpStatusCode.OK,
                            events,
                            new MediaTypeHeaderValue("application/json"));
            response.Headers.CacheControl = new CacheControlHeaderValue { MaxAge = TimeSpan.FromHours(1.0) };

            return response;
        }
    }
}