using System.Runtime.Serialization;

namespace RukatukApi.Models
{
    [DataContract]
    public class EventbriteWebhookConfig
    {
        [DataMember(Name = "action")]
        public string Action { get; set; }

        [DataMember(Name = "user_id")]
        public string UserId { get; set; }

        [DataMember(Name = "webhook_id")]
        public string WebhookId { get; set; }
    }
}
