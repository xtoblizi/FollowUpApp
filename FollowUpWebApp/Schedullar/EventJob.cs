using FollowUpWebApp.Models;
using FollowUpWebApp.SMS_Service;
using System;
using System.Linq;
using System.Text;

namespace FollowUpWebApp.Schedullar
{

    public class EventJob
    {
        private readonly SmsServiceTemp _smsService;
        private readonly ApplicationDbContext _db;

        public EventJob()
        {
            _db = new ApplicationDbContext();
            _smsService = new SmsServiceTemp();
        }

        public void Execute()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                //var birthday = _db.Members.AsNoTracking().Where(x => DateTime.Compare(x.DateOfBirth, DateTime.Now.Date) == 0)
                //                       .Select(s => s.PhoneNumber).ToList();
                var birthday = _db.Members.AsNoTracking().Where(x => x.DateOfBirth.Month == DateTime.Now.Month
                                                                     && x.DateOfBirth.Day == DateTime.Now.Day)
                    .Select(s => s.PhoneNumber).ToList();

                foreach (var item in birthday)
                {
                    builder.Append(item).Append(",");

                }
                var body = _db.MessageTemplates.Where(x => x.MessagingType.ToString().Equals(MessageType.Birthday.ToString()))
                    .Select(s => s.MessageBody).FirstOrDefault();

                SendMessage(body, builder);
            }
            catch (Exception ex)
            {

            }
        }
        private void SendMessage(string body, StringBuilder builder)
        {
            Sms sms = new Sms()
            {
                Sender = "FollowUp",
                Message = body,
                Recipient = builder.ToString()
            };
            string response = _smsService.Send(sms);
        }
    }
}