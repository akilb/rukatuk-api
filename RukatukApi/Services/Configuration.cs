using System;

namespace RukatukApi.Services
{
    public class Configuration : IConfiguration
    {
        public string EventbriteOAuthToken => GetEnvironmentVariable("RUKATUKAPI_EVENTBRITE_OAUTH_TOKEN");

        public string FlickrApiKey => GetEnvironmentVariable("RUKATUKAPI_FLICKR_API_KEY");

        public string FlickrApiSecret => GetEnvironmentVariable("RUKATUKAPI_FLICKR_API_SECRET");

        public string FlickrPhotoSetId => "72157687357180146";

        private string GetEnvironmentVariable(string name)
        {
            var valueInAzure = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
            return valueInAzure ?? Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
        }
    }
}
