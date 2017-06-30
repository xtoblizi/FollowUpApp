using FollowUpWebApp.Models;
using FollowUpWebApp.SMS_Service;
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace FollowUpWebApp.Controllers
{
    public class SmsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly SmsServiceTemp _smsService = new SmsServiceTemp();
        // GET: Sms
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to SMS Web Application!";

            return View();
        }

        public ActionResult Sendms()
        {

            try
            {
                StringBuilder builder = new StringBuilder();
                var absentee = _db.Attendances.AsNoTracking().Where(x => x.AttendanceDate.Year == DateTime.Now.Year
                                            && x.AttendanceDate.Month == DateTime.Now.Month
                                            && x.AttendanceDate.Day == DateTime.Now.Day
                                                           && x.IsPresent.Equals(false))
                                       .Select(s => s.MemberId)
                                       .ToList();
                //var memberList = _db.Members.ToList()
                foreach (var item in absentee)
                {
                    string phoneNo = _db.Members.Where(x => x.MemberId.Equals(item))
                                        .Select(s => s.PhoneNumber).FirstOrDefault();
                    builder.Append(phoneNo).Append(",");


                }
                var body = _db.MessageTemplates.Where(x => x.MessagingType.ToString().Equals(MessageType.Absent.ToString()))
                                    .Select(s => s.MessageBody).FirstOrDefault();

                SendMessage(body, builder);
               
                //Sms sms = new Sms()
                //{
                //    Sender = "FollowUp",
                //    Message = body,
                //    Recipient = builder.ToString()
                //};
                //bool isSuccess = false;
                //string errMsg;
                //string response = _smsService.Send(sms); //Send sms

                //string code = _smsService.GetResponseMessage(response, out isSuccess, out errMsg);

                //if (!isSuccess)
                //{
                //    ViewBag.Message = errMsg;
                //}
                //else
                //{
                //    ViewBag.Message = "Message was successfully sent.";
                //}
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View("Index");
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