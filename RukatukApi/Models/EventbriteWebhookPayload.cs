using System.Runtime.Serialization;

namespace RukatukApi.Models
{
    [DataContract]
    public class EventbriteWebhookPayload
    {
        [DataMember(Name = "config")]
        public EventbriteWebhookConfig Config { get; set; }

        [DataMember(Name = "api_url")]
        public string ApiUrl { get; set; }
    }
}
