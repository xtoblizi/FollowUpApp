using Quartz;
using Quartz.Impl;

namespace FollowUpWebApp.Schedullar
{
    public class Jobschedular
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<SmsJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                (s => s.WithIntervalInHours(24)
                    .OnEveryDay().StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(21, 20))
                ).Build();

            scheduler.ScheduleJob(job, trigger);

            IJobDetail job1 = JobBuilder.Create<AttendanceJob>().Build();

            ITrigger trigger1 = TriggerBuilder.Create()
                .WithCronSchedule("0 30 22 ? * SUN,WED,THU,FRI").StartNow().Build();

            scheduler.ScheduleJob(job1, trigger1);


        }
    }

    //public class Jobschedular2
    //{
    //    public static void Start()
    //    {
    //        IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
    //        scheduler.Start();

    //        IJobDetail job = JobBuilder.Create<AttendanceJob>().Build();

    //        ITrigger trigger = TriggerBuilder.Create()
    //            .WithCronSchedule("0 45 9 ? * SUN,WED").StartNow().Build();

    //        scheduler.ScheduleJob(job, trigger);

    //    }
    //}
}