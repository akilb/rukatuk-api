using System;
using System.Runtime.Serialization;

namespace RukatukApi.Models
{
    [DataContract]
    public class Event
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "descriptionText")]
        public string DescriptionText { get; set; }

        [DataMember(Name = "descriptionHtml")]
        public string DescriptionHtml { get; set; }

        [DataMember(Name = "vanityUrl")]
        public string VanityUrl { get; set; }

        [DataMember(Name = "createdDate")]
        public DateTimeOffset CreatedDate { get; set; }

        [DataMember(Name = "startDate")]
        public DateTimeOffset StartDate { get; set; }

        [DataMember(Name = "endDate")]
        public DateTimeOffset EndDate { get; set; }

        [DataMember(Name = "imageUrl")]
        public string ImageUrl { get; set; }

        [DataMember(Name = "smallImageUrl")]
        public string SquareImageUrl { get; set; }

        [DataMember(Name = "minTicketPrice")]
        public string MinTicketPrice { get; set; }

        [DataMember(Name = "venue")]
        public Venue Venue { get; set; }
    }
}
