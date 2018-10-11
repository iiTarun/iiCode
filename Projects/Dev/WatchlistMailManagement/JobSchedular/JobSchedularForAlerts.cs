using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UPRD.Data;
using UPRD.Enums;

namespace WatchlistMailManagement.JobSchedular
{
    public class JobSchedularForAlerts
    {
      
       // static IUprdSettingRepository _serviceSetting = null;


        public async static void Start()
        {
            UPRDEntities DbContext = new UPRDEntities();

            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            var oacyid = (int)Settings.oacyWatchlistJob;
            var oacyTiming= DbContext.Setting.Where(a => a.ID == oacyid).FirstOrDefault().Value;


            var oacymailid = (int)Settings.oacyWatchlistJobSendMail;
            var oacyMailTiming = DbContext.Setting.Where(a => a.ID == oacymailid).FirstOrDefault().Value;

            var unscid = (int)Settings.unscWatchlistJob;
            var unscTiming = DbContext.Setting.Where(a => a.ID == unscid).FirstOrDefault().Value;

            var unscmailid = (int)Settings.unscWatchlistJobSendMail;
            var unscMailTiming = DbContext.Setting.Where(a => a.ID == unscmailid).FirstOrDefault().Value;

            var swntid = (int)Settings.swntWatchlistJob;
            var swntTiming = DbContext.Setting.Where(a => a.ID == swntid).FirstOrDefault().Value;

            var swntmailid = (int)Settings.swntWatchlistJobSendMail;
            var swntMailTiming = DbContext.Setting.Where(a => a.ID == swntmailid).FirstOrDefault().Value;


            #region Daily Mail Alert Job

            //IJobDetail job = JobBuilder.Create<WatchListMailAlertDailyJob>().Build();

            //ITrigger jobtrigger = TriggerBuilder.Create()
            //  .WithIdentity("trigger2", "group1")
            //  .WithDailyTimeIntervalSchedule(
            //    s => s.WithIntervalInHours(24)
            //    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(11, 00))      // UTC=11:00AM ==> CST=05:00AM (Central America)
            //    .InTimeZone(TimeZoneInfo.Utc))
            //  .Build();


            #endregion




            #region WatchListAvailAlertOACY Job 

            IJobDetail WatchListAvailAlertOACYJob = JobBuilder.Create<WatchListAvailAlertOACY>().Build();

            ITrigger WatchListAvailAlertOACYtrigger = TriggerBuilder.Create()
              .WithIdentity("trigger3", "group2")
              .StartNow()
              .WithCronSchedule(oacyTiming)              
              .Build();

            IJobDetail WatchListAvailAlertOACYSendMailJob = JobBuilder.Create<WatchListAvailAlertOACYSendMail>().Build();

            ITrigger WatchListAvailAlertOACYSendMailtrigger = TriggerBuilder.Create()
              .WithIdentity("trigger6", "group6")
              .StartNow()
              .WithCronSchedule(oacyMailTiming)
              .Build();


            #endregion


            #region WatchListAvailAlertUNSC Job 

            IJobDetail WatchListAvailAlertUNSCJob = JobBuilder.Create<WatchListAvailAlertUNSC>().Build();

            ITrigger WatchListAvailAlertUNSCtrigger = TriggerBuilder.Create()
              .WithIdentity("trigger4", "group3")
              .StartNow()
              .WithCronSchedule(unscTiming)
              .Build();

            IJobDetail WatchListAvailAlertUNSCJobMailSend = JobBuilder.Create<WatchListAvailAlertUNSCSendMail>().Build();

            ITrigger WatchListAvailAlertUNSCtriggerMailSend = TriggerBuilder.Create()
              .WithIdentity("trigger7", "group7")
              .StartNow()
              .WithCronSchedule(unscMailTiming)
              .Build();
            #endregion


            #region WatchListAvailAlertSWNT Job 

            IJobDetail WatchListAvailAlertSWNTJob = JobBuilder.Create<WatchListAvailAlertSWNT>().Build();

            ITrigger WatchListAvailAlertSWNTtrigger = TriggerBuilder.Create()
              .WithIdentity("trigger5", "group4")
              .StartNow()
              .WithCronSchedule(swntTiming)
              .Build();

            IJobDetail WatchListAvailAlertSWNTJobSendMail = JobBuilder.Create<WatchListAvailAlertSWNTSendMail>().Build();

            ITrigger WatchListAvailAlertSWNTtriggerSendMail = TriggerBuilder.Create()
              .WithIdentity("trigger8", "group8")
              .StartNow()
              .WithCronSchedule(swntTiming)
              .Build();

            #endregion




            #region WatchListAvailAlertOACY Job 


            #endregion

            await scheduler.Start();
           // await scheduler.ScheduleJob(job, jobtrigger);
            await scheduler.ScheduleJob(WatchListAvailAlertOACYJob, WatchListAvailAlertOACYtrigger);
            await scheduler.ScheduleJob(WatchListAvailAlertOACYSendMailJob, WatchListAvailAlertOACYSendMailtrigger);

            await scheduler.ScheduleJob(WatchListAvailAlertUNSCJob, WatchListAvailAlertUNSCtrigger);
            await scheduler.ScheduleJob(WatchListAvailAlertUNSCJobMailSend, WatchListAvailAlertUNSCtriggerMailSend);

            await scheduler.ScheduleJob(WatchListAvailAlertSWNTJob, WatchListAvailAlertSWNTtrigger);
            await scheduler.ScheduleJob(WatchListAvailAlertSWNTJobSendMail, WatchListAvailAlertSWNTtriggerSendMail);
            

        }

    }
}