using System.Runtime.Serialization;

namespace RukatukApi.Models
{
    [DataContract]
    public class EventbriteImage
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}