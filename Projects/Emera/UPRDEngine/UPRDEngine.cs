using Ninject;
using Nom1Done.DTO;
using Quartz;
using Quartz.Impl;
using System.Reflection;
using System;
using UPRD.Data.Repositories;
using UPRDEngine.JobManager;
using UPRDEngine.EDIEngineSendAndReceive;

namespace UPRDEngine
{
    class UPRDEngine
    {
        #region Engine Inventory
        #region services Obj
        static IUprdSettingRepository _serviceSetting = null;

        #endregion
        #region Quartz Troops
        public static StdSchedulerFactory _scheduleFactory;
        public static IScheduler _jobScheduler;
        #endregion
        #region Send Inventory
        static string UprdReqTimeForOacy, UprdReqTimeForUnsc, UprdReqTimeForSwnt;
        #endregion
        #region Receive Inventory
        static string TimeAndFreqForReceiveFileProcess;
        #endregion
        #endregion
        static void Main(string[] args)
        {
            
            #region settings initialize for sending file 
            StandardKernel Kernal = new StandardKernel();
            Kernal.Load(Assembly.GetExecutingAssembly());
            _serviceSetting = Kernal.Get<UprdSettingRepository>();
            #endregion
            #region Quartz Servcie Initialize Job
            _scheduleFactory = new StdSchedulerFactory();
            _jobScheduler = _scheduleFactory.GetScheduler();
            _jobScheduler.Start();
            #endregion
            #region job for receive files processing
            ProcessReceiveFile_JobCreation(_jobScheduler);
            ProcessReceiveFileAgain_JobCreation(_jobScheduler);
            #endregion
            #region Jobs for file sending
            #region job for Uprd (OACY)
            //job for Uprd (OACY)
            UprdOacyJobCreation(_jobScheduler);
            #endregion
            #region Job for Uprd (UNSC)
            //Job for Uprd (UNSC)
            UprdUnscJobCreation(_jobScheduler);
            #endregion
            #region Job for Uprd (SWNT)
            //Job for Uprd (SWNT)
            UprdSwntJobCreation(_jobScheduler);
            #endregion
            #endregion
            Console.WriteLine("Press Any Key TO Exit");
            Console.Read();

        }
        private static void ProcessReceiveFile_JobCreation(IScheduler _jobScheduler)
        {
            try
            {
                TimeAndFreqForReceiveFileProcess = _serviceSetting.GetById((int)Settings.TimeAndFreqForReceiveFileProcess).Value;
                Console.WriteLine("JobScheduler Start for process receive files (check every 5 sec).");
                IJobDetail EncEDIGenerationJobDetail = JobBuilder.Create<JobManagerReceiveUprdProcessing>()
                                                    .WithIdentity(string.Format("{0}", "ReceiveUprdJob"))
                                                    .Build();
                ITrigger EncEDIGenerationJobTrigger = TriggerBuilder.Create()
                                                    .WithIdentity(string.Format("{0}", "ReceiveUprdJob"))
                                                    .StartNow()
                                                    .WithCronSchedule(TimeAndFreqForReceiveFileProcess)
                                                    .Build();
                _jobScheduler.ScheduleJob(EncEDIGenerationJobDetail, EncEDIGenerationJobTrigger);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:- " + ex.Message);
            }
        }
        private static void ProcessReceiveFileAgain_JobCreation(IScheduler _jobScheduler)
        {
            try
            {
                //TimeAndFreqForReceiveFileProcess = "";//_serviceSetting.GetById((int)Settings.TimeAndFreqForReceiveFileProcess).Value;
                IJobDetail EncEDIGenerationJobDetail = JobBuilder.Create<JobManagerReceiveUprdProcessingAgain>()
                                                    .WithIdentity(string.Format("{0}", "ReceiveUprdAgainOnCrashJob"))
                                                    .Build();
                ITrigger EncEDIGenerationJobTrigger = TriggerBuilder.Create()
                                                    .WithIdentity(string.Format("{0}", "ReceiveUprdAgainOnCrashJob"))
                                                    .StartNow()
                                                    .WithDailyTimeIntervalSchedule(s=>s.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(23,00)))
                                                    //.WithDailyTimeIntervalSchedule().
                                                    .Build();
                _jobScheduler.ScheduleJob(EncEDIGenerationJobDetail, EncEDIGenerationJobTrigger);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:- " + ex.Message);
            }
        }

        #region Job creation for Swnt request
        private static void UprdSwntJobCreation(IScheduler jobScheduler)
        {
            try
            {
                UprdReqTimeForSwnt = _serviceSetting.GetById((int)Settings.UprdReqTimeForSwnt).Value;
                IJobDetail SwntJobDetail = JobBuilder.Create<JobManagerSwntJob>()
                                                    .WithIdentity(string.Format("{0}", "SwntJob"))
                                                    .Build();
                ITrigger SwntJobTrigger = TriggerBuilder.Create()
                                                    .WithIdentity(string.Format("{0}", "SwntJob"))
                                                    .StartNow()
                                                    .WithCronSchedule(UprdReqTimeForSwnt)
                                                    .Build();
                _jobScheduler.ScheduleJob(SwntJobDetail, SwntJobTrigger);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:- ", ex.Message);
            }
        }
        #endregion
        #region job creation for Unsc request
        private static void UprdUnscJobCreation(IScheduler jobScheduler)
        {
            try
            {
                UprdReqTimeForUnsc = _serviceSetting.GetById((int)Settings.UprdReqTimeForUnsc).Value;
                IJobDetail UnscJobDetail = JobBuilder.Create<JobManagerUnscJob>()
                                                    .WithIdentity(string.Format("{0}", "UnscJob"))
                                                    .Build();
                ITrigger UnscJobTrigger = TriggerBuilder.Create()
                                                    .WithIdentity(string.Format("{0}", "UnscJob"))
                                                    .StartNow()
                                                    .WithCronSchedule(UprdReqTimeForUnsc)
                                                    .Build();
                _jobScheduler.ScheduleJob(UnscJobDetail, UnscJobTrigger);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:- ", ex.Message);
            }
        }
        #endregion
        #region job creation for Oacy request
        private static void UprdOacyJobCreation(IScheduler jobScheduler)
        {
            try
            {
                UprdReqTimeForOacy = _serviceSetting.GetById((int)Settings.UprdReqTimeForOacy).Value;
                IJobDetail OacyJobDetail = JobBuilder.Create<JobManagerOacyJob>()
                                                    .WithIdentity(string.Format("{0}", "OacyJob"))
                                                    .Build();
                ITrigger OacyJobTrigger = TriggerBuilder.Create()
                                                    .WithIdentity(string.Format("{0}", "OacyJob"))
                                                    //.StartNow()
                                                    .WithCronSchedule(UprdReqTimeForOacy)
                                                    .Build();
                _jobScheduler.ScheduleJob(OacyJobDetail, OacyJobTrigger);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:- ", ex.Message);
            }
        }
        #endregion
    }
}
