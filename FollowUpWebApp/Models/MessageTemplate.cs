namespace FollowUpWebApp.Models
{
    public class MessageTemplate
    {
        public int MessageTemplateId { get; set; }

        public MessageType MessagingType { get; set; }

        public string MessageBody { get; set; }

        //public virtual MessagingType MessagingType { get; set; }
    }
}