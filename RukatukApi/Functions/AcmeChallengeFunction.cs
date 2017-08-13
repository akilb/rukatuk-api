using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Threading;
using System.IO;

namespace RukatukApi.Functions
{
    public static class AcmeChallengeFunction
    {
        [FunctionName("letsencrypt")]

        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "letsencrypt/{code}")]HttpRequestMessage req,
            string code,
            TraceWriter log,
            CancellationToken cancellationToken)
        {
            log.Info($"Processing LetsEncrypt ACME challenge {code}...");

            var content = File.ReadAllText(@"D:\home\site\wwwroot\.well-known\acme-challenge\" + code);
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(content, System.Text.Encoding.UTF8, "text/plain");
            return resp;
        }
    }
}