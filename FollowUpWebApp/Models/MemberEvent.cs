using System;
using System.ComponentModel.DataAnnotations;

namespace FollowUpWebApp.Models
{
    public class MemberEvent
    {
        public int MemberEventId { get; set; }

        [DataType(DataType.Date)]
        public DateTime WeddingAnniversaryDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime WidowedDate { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime WidowedDate { get; set; }
    }
}