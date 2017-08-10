using System;
using System.Runtime.Serialization;

namespace RukatukApi.Models
{
    [DataContract]
    public class EventbriteEvent
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public EventbriteContent Name { get; set; }

        [DataMember(Name = "description")]
        public EventbriteContent Description { get; set; }

        [DataMember(Name = "vanity_url")]
        public string VanityUrl { get; set; }

        [DataMember(Name = "created")]
        public DateTimeOffset Created { get; set; }

        [DataMember(Name = "start")]
        public EventbriteDate Start { get; set; }

        [DataMember(Name = "end")]
        public EventbriteDate End { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "logo")]
        public EventbriteLogo Logo { get; set; }

        [DataMember(Name = "venue_id")]
        public string VenueId { get; set; }
    }
}
