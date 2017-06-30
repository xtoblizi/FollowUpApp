using FollowUpWebApp.Models;
using FollowUpWebApp.SMS_Service;
using FollowUpWebApp.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FollowUpWebApp.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly SmsServiceTemp _smsService;

        public AttendanceController()
        {
            _db = new ApplicationDbContext();
            _smsService = new SmsServiceTemp();
        }

        // GET: Attendance
        public async Task<ActionResult> Index()
        {
            int userId = Convert.ToInt32(User.Identity.GetUserId());
            var user = await _db.Members.AsNoTracking().Where(x => x.MemberId.Equals(userId))
                        .Select(s => s.ChurchDataId).FirstOrDefaultAsync();
            var mList = new List<MemberList>();
            var members = await _db.Members.Where(s => s.ChurchDataId.Equals(user)).ToListAsync();

            var attendanceid = await _db.Attendances.CountAsync();
            foreach (var item in members)
            {
                mList.Add(new MemberList()
                {
                    Id = ++attendanceid,
                    MemberId = item.MemberId,
                    FullName = item.FullName,
                    IsPresent = false
                });
            }

            AttendanceVm attendanceList = new AttendanceVm { MemberLists = mList };

            return View(attendanceList);
        }


        [HttpPost]
        public async Task<ActionResult> Index(AttendanceVm model)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in model.MemberLists)
                {
                    var myId = await _db.Attendances.Where(x => x.AttendanceDate.Year == DateTime.Now.Year
                                                               && x.AttendanceDate.Month == DateTime.Now.Month
                                                               && x.AttendanceDate.Day == DateTime.Now.Day
                                                               && x.MemberId.Equals(item.MemberId))
                                                                .Select(s => s.AttendanceId).FirstOrDefaultAsync();
                    var count = await _db.Attendances.Where(x => x.AttendanceDate.Year == DateTime.Now.Year
                                                           && x.AttendanceDate.Month == DateTime.Now.Month
                                                           && x.AttendanceDate.Day == DateTime.Now.Day
                                                           && x.MemberId.Equals(item.MemberId)).CountAsync();
                    //var count = await _db.Attendances.Where(x => x.AttendanceId.Equals(item.Id)).CountAsync();
                    if (count >= 1)
                    {
                        Attendance attended = await _db.Attendances.FindAsync(myId);

                        if (attended != null)
                        {
                            if (attended.IsPresent.Equals(true))
                            {
                                attended.IsPresent = true;
                            }
                            else
                            {
                                attended.IsPresent = item.IsPresent;
                            }

                            _db.Entry(attended).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        var attendance = new Attendance()
                        {
                            AttendanceId = item.Id,
                            MemberId = item.MemberId,
                            IsPresent = item.IsPresent,
                            AttendanceDate = Convert.ToDateTime(DateTime.Now.ToShortDateString())

                        };
                        _db.Attendances.AddOrUpdate(attendance);
                    }

                }
                await _db.SaveChangesAsync();
            }
            ViewBag.Message = "Successfully Addded";
            return View(model);
        }


        public async Task<ActionResult> SendMessage(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        public async Task<ActionResult> SendBirthdayMessage()
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
            var mymessage = $"Message has been sent to {birthday.Count} Celebrants";
            return RedirectToAction("SendMessage", new { message = mymessage });
        }

        public async Task<ActionResult> SendAttendanceMessage()
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
            return RedirectToAction("SendMessage");
        }

        public async Task<ActionResult> SendFirstTimerMessage()
        {
            int userId = Convert.ToInt32(User.Identity.GetUserId());
            var user = await _db.Members.AsNoTracking().Where(x => x.MemberId.Equals(userId))
                .Select(s => s.ChurchDataId).FirstOrDefaultAsync();
            StringBuilder builder = new StringBuilder();
            var newMember = _db.Members.AsNoTracking().Where(x => x.IsANewMember.Equals(true)
                                && x.ChurchDataId.Equals(user)).ToList();
            //var memberList = _db.Members.ToList()
            foreach (var item in newMember)
            {
                builder.Append(item.PhoneNumber).Append(",");

                item.IsANewMember = false;
                _db.Entry(item).State = EntityState.Modified;
                await _db.SaveChangesAsync();


            }
            var body = _db.MessageTemplates.Where(x => x.MessagingType.ToString().Equals(MessageType.FirstTimer.ToString()))
                .Select(s => s.MessageBody).FirstOrDefault();

            SendMessage(body, builder);
            var mymessage = $"Message has been sent to {newMember.Count} Member(s)";
            return RedirectToAction("SendMessage", new { message = mymessage });
        }

        public async Task<ActionResult> SendSecondTimerMessage()
        {
            int counter = 0;
            int userId = Convert.ToInt32(User.Identity.GetUserId());
            var user = await _db.Members.AsNoTracking().Where(x => x.MemberId.Equals(userId))
                .Select(s => s.ChurchDataId).FirstOrDefaultAsync();
            StringBuilder builder = new StringBuilder();
            var newMember = _db.Members.AsNoTracking().Where(x => x.IsANewMember.Equals(true)
                                                               && x.ChurchDataId.Equals(user)).ToList();
            //var memberList = _db.Members.ToList()
            foreach (var item in newMember)
            {
                var attendanceCount = _db.Attendances.Count(x => x.MemberId.Equals(item.MemberId));
                if (attendanceCount == 2)
                {
                    builder.Append(item.PhoneNumber).Append(",");
                    ++counter;
                }

            }
            var body = _db.MessageTemplates.Where(x => x.MessagingType.ToString().Equals(MessageType.SecondTimer.ToString()))
                .Select(s => s.MessageBody).FirstOrDefault();

            SendMessage(body, builder);
            var mymessage = $"Message has been sent to {counter} Member(s)";
            return RedirectToAction("SendMessage", new { message = mymessage });
        }
        public async Task<ActionResult> SendThirdTimerMessage()
        {
            int counter = 0;
            int userId = Convert.ToInt32(User.Identity.GetUserId());
            var user = await _db.Members.AsNoTracking().Where(x => x.MemberId.Equals(userId))
                .Select(s => s.ChurchDataId).FirstOrDefaultAsync();
            StringBuilder builder = new StringBuilder();
            var newMember = _db.Members.AsNoTracking().Where(x => x.IsANewMember.Equals(true)
                                                                  && x.ChurchDataId.Equals(user)).ToList();
            //var memberList = _db.Members.ToList()
            foreach (var item in newMember)
            {
                var attendanceCount = _db.Attendances.Count(x => x.MemberId.Equals(item.MemberId));
                if (attendanceCount == 3)
                {
                    builder.Append(item.PhoneNumber).Append(",");
                    ++counter;
                }

            }
            var body = _db.MessageTemplates.Where(x => x.MessagingType.ToString().Equals(MessageType.ThirdTimer.ToString()))
                .Select(s => s.MessageBody).FirstOrDefault();

            SendMessage(body, builder);
            var mymessage = $"Message has been sent to {counter} Member(s)";
            return RedirectToAction("SendMessage", new { message = mymessage });
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