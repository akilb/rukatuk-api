using System.Runtime.Serialization;

namespace RukatukApi.Models
{
    [DataContract]
    public class EventbriteContent
    {
        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "html")]
        public string Html { get; set; }
    }
}
