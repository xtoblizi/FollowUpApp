using FollowUpWebApp.Models;
using FollowUpWebApp.SMS_Service;
using Quartz;
using System;
using System.Linq;
using System.Text;

namespace FollowUpWebApp.Schedullar
{
    public class SmsJob : IJob
    {
        private readonly SmsServiceTemp _smsService = new SmsServiceTemp();
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                //var birthday = _db.Members.AsNoTracking().Where(x => DateTime.Compare(x.DateOfBirth, DateTime.Now.Date) == 0)
                //                       .Select(s => s.PhoneNumber).ToList();
                var birthday = _db.Members.AsNoTracking().Where(x => x.DateOfBirth.Year == DateTime.Now.Year
                                            && x.DateOfBirth.Month == DateTime.Now.Month
                                            && x.DateOfBirth.Day == DateTime.Now.Day)
                                      .Select(s => s.PhoneNumber).ToList();

                foreach (var item in birthday)
                {
                    builder.Append(item).Append(",");

                }
                var body = _db.MessageTemplates.Where(x => x.MessagingType.ToString().Equals(MessageType.Birthday.ToString()))
                                    .Select(s => s.MessageBody).FirstOrDefault();

                SendMessage(body, builder);
                //StringBuilder builder = new StringBuilder();
                ////builder.Append("07032661755").Append(",");
                ////builder.Append("09061770201").Append(",");
                ////builder.Append("08068111435").Append(",");
                //builder.Append("07036927669").Append(",");
                //Sms sms = new Sms()
                //{
                //    Sender = "FollowUp",
                //    Message = "Testing the Integration of SMS Api to the App",
                //    Recipient = builder.ToString()
                //};
                //bool isSuccess = false;
                //string errMsg;
                //string response = _smsService.Send(sms); //Send sms

                //string code = _smsService.GetResponseMessage(response, out isSuccess, out errMsg);

                //if (!isSuccess)
                //{
                //    //ViewBag.Message = errMsg;
                //}
                //else
                //{
                //    // ViewBag.Message = "Message was successfully sent.";
                //}
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