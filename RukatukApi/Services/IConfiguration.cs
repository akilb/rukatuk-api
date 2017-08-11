namespace RukatukApi.Services
{
    public interface IConfiguration
    {
        string EventbriteOAuthToken { get; }
        string FlickrApiKey { get; }
        string FlickrApiSecret { get; }
        string FlickrPhotoSetId { get; }
    }
}
