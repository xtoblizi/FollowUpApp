using System.ComponentModel.DataAnnotations;

namespace FollowUpWebApp.SMS_Service
{
    public class Sms
    {
        public string Recipient { get; set; }

        public string Sender { get; set; }

        [Display(Name = "Message")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}