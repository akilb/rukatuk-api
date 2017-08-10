using System.Runtime.Serialization;

namespace RukatukApi.Models
{
    [DataContract]
    public class EventbriteCountry
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }
        [DataMember(Name = "label")]
        public string Label { get; set; }
    }
}
