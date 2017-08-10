using System.Runtime.Serialization;

namespace RukatukApi.Models
{
    [DataContract]
    public class EventbriteLogo
    {
        [DataMember(Name = "original")]
        public EventbriteImage Original { get; set; }
    }
}