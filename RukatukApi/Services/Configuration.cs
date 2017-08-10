using System;

namespace RukatukApi.Services
{
    public class Configuration : IConfiguration
    {
        public string EventbriteOAuthToken => GetEnvironmentVariable("RUKATUKAPI_EVENTBRITE_OAUTH_TOKEN");

        private string GetEnvironmentVariable(string name)
        {
            var valueInAzure = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
            return valueInAzure ?? Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
        }
    }
}
