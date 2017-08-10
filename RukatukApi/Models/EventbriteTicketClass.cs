using System.Runtime.Serialization;

namespace RukatukApi.Models
{
    [DataContract]
    public class EventbriteTicketClass
    {
        [DataMember(Name = "actual_cost")]
        public EventbriteMoney ActualCost { get; set; }

        [DataMember(Name = "free")]
        public bool? IsFree { get; set; }

        [DataMember(Name = "event_id")]
        public string EventId { get; set; }
    }
}
