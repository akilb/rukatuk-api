namespace RukatukApi.Services
{
    public interface IConfiguration
    {
        string AzureStorageConnectionString { get; }
        string EventbriteOAuthToken { get; }
        string FlickrApiKey { get; }
        string FlickrApiSecret { get; }
        string FlickrPhotoSetId { get; }
    }
}
