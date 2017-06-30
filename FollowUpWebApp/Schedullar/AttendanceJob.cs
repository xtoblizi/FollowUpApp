using FollowUpWebApp.Models;
using FollowUpWebApp.SMS_Service;
using Quartz;
using System;
using System.Linq;
using System.Text;

namespace FollowUpWebApp.Schedullar
{
    public class AttendanceJob : IJob
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly SmsServiceTemp _smsService = new SmsServiceTemp();
        public void Execute(IJobExecutionContext context)
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

            //var groupId = _db.GroupCellLists.Where(x => x.Department.DepartmenType.Equals(DeptType.Group))
            //                        .Distinct().ToList();
            //foreach (var item in groupId)
            //{
            //    foreach (var member in absentee)
            //    {
            //       // if()
            //    }
            //}

            //var groupleader = _db.Users.Where(x => x.DepartmentId.Equals(groupId))
            //                        .Select(s => s.MemberId).ToList();
            //foreach (var leader in groupleader)
            //{
            //    string leaderno = _db.Members.Where(x => x.MemberId.Equals(leader))
            //                     .Select(s => s.PhoneNumber).FirstOrDefault();
            //    builder2.Append(leaderno).Append(",");
            //}
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