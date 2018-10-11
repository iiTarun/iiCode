using CentralisedUprd.Api.Models;
using Quartz;
using Quartz.Impl;

namespace CentralisedUprd.Api.JobSchedular
{
    public class JobSchedularForAlerts
    {


        public async static void Start()
        {
            UprdDbEntities1 DbContext = new UprdDbEntities1();

            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            #region Uprd Status Daily Alert
            IJobDetail jobUprdStatusAlert = JobBuilder.Create<UprdStatusResultDailyAlert>().Build();
            ITrigger triggerUprdStatusAlert = TriggerBuilder.Create()
            .WithIdentity("UprdStatusAlert", "groupUprdStatusAlert")
            .StartNow()
            .WithDailyTimeIntervalSchedule(s => s.WithIntervalInHours(24)
            .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(23, 00)))
            //.WithDailyTimeIntervalSchedule(s => s.WithIntervalInMinutes(3))
            .Build();
            #endregion 

            await scheduler.Start();

            await scheduler.ScheduleJob(jobUprdStatusAlert, triggerUprdStatusAlert);

        }

    }
}