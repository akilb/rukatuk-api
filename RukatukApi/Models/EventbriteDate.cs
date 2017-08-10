using System;
using System.Runtime.Serialization;

namespace RukatukApi.Models
{
    [DataContract]
    public class EventbriteDate
    {
        [DataMember(Name = "timezone")]
        public string TimeZone { get; set; }

        [DataMember(Name = "local")]
        public DateTime Local { get; set; }

        [DataMember(Name = "utc")]
        public DateTime Utc { get; set; }
    }
}
