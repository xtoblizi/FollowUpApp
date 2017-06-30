using System;

namespace FollowUpWebApp.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public int MemberId { get; set; }
        public bool IsPresent { get; set; }
        public DateTime AttendanceDate { get; set; }
        public virtual Member Member { get; set; }


    }
}