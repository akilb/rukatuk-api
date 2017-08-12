using Microsoft.Azure;
using System;
using System.Configuration;

namespace RukatukApi.Services
{
    public class Configuration : IConfiguration
    {
        public string AzureStorageConnectionString => GetSetting("AzureWebJobsStorage");

        public string EventbriteOAuthToken => GetSetting("EventbriteOAuthToken");

        public string FlickrApiKey => GetSetting("FlickrApiKey");

        public string FlickrApiSecret => GetSetting("FlickrApiSecret");

        public string FlickrPhotoSetId => "72157687357180146";

        private string GetSetting(string name)
        {
            return ConfigurationManager.AppSettings[name] ?? CloudConfigurationManager.GetSetting(name);
        }
    }
}
