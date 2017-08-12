using Microsoft.Azure;
using System;

namespace RukatukApi.Services
{
    public class Configuration : IConfiguration
    {
        public string AzureStorageConnectionString => CloudConfigurationManager.GetSetting("AzureWebJobsStorage");

        public string EventbriteOAuthToken => CloudConfigurationManager.GetSetting("EventbriteOAuthToken");

        public string FlickrApiKey => CloudConfigurationManager.GetSetting("FlickrApiKey");

        public string FlickrApiSecret => CloudConfigurationManager.GetSetting("FlickrApiSecret");

        public string FlickrPhotoSetId => "72157687357180146";
    }
}
