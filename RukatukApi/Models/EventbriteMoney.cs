using System.Runtime.Serialization;

namespace RukatukApi.Models
{
    [DataContract]
    public class EventbriteMoney
    {
        [DataMember(Name = "display")]
        public string Display { get; set; }

        [DataMember(Name = "value")]
        public int? Value { get; set; }
    }
}
