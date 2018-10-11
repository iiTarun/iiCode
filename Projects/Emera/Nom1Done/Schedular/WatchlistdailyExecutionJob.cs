using Nom1Done.Data;
using Nom1Done.Data.Repositories;
using Nom1Done.EDIEngineSendAndReceive;
using Nom1Done.Enums;
using Nom1Done.Service;
using Nom1Done.Service.Interface;
using Quartz;
using System;

namespace Nom1Done.Schedular
{
    public class WatchlistdailyExecutionJob : IJob
    {
        IWatchlistService watchlistservice;
        INotifierEntityService notifierEntityService;


        public  WatchlistdailyExecutionJob(INotifierEntityService notifierEntityService,IWatchlistService watchlistservice)
        {
            this.watchlistservice = watchlistservice;
            this.notifierEntityService = notifierEntityService;            
        }

        public void Execute(IJobExecutionContext context)
        {
            var resultoacy = watchlistservice.ExecuteWatchList(WatchlistAlertFrequency.Daily,EnercrossDataSets.OACY);
            var resultunsc = watchlistservice.ExecuteWatchList(WatchlistAlertFrequency.Daily, EnercrossDataSets.UNSC);
            var resultswnt = watchlistservice.ExecuteWatchList(WatchlistAlertFrequency.Daily, EnercrossDataSets.SWNT);
            //Console.Write("Watch List Alert Mail- Schedular Working.. " + DateTime.Now.ToString());

        }

    }
    /// <summary>
    /// Job for mail uprd status to the client
    /// </summary>
    public class CustomUPRDJob : IJob
    {
        IUprdStatusService UprdStatusService;

        public CustomUPRDJob(IUprdStatusService UprdStatusService)
        {
            this.UprdStatusService = UprdStatusService;
        }
        public void Execute(IJobExecutionContext context)
        {
            var IsMailSend = UprdStatusService.SendUPRDReport();
        }
    }
    /// <summary>
    /// for update nomination status with replace
    /// </summary>
    public class MatchNomination : IJob
    {
        IBatchRepository batchRepo;
        public MatchNomination(IBatchRepository batchRepo)
        {
            this.batchRepo = batchRepo;
        }
        public void Execute(IJobExecutionContext context)
        {
            batchRepo.matchPathedNomination();
            //batchRepo.matchPNTNomination();
            //batchRepo.matchNonPathed();
        }
    }
    /// <summary>
    /// Match orphan sqts with noms
    /// </summary>
    public class SqtsScenerio2nd : IJob
    {
        ISQTSPerTransactionRepository sqtsRepo;
        public SqtsScenerio2nd(ISQTSPerTransactionRepository sqtsRepo)
        {
            this.sqtsRepo = sqtsRepo;
        }
        public void Execute(IJobExecutionContext context)
        {
            sqtsRepo.Sqts2ndScenerio();
        }
    }

    #region Engine Jobs
    #region Shooting form here(send files processing job)
    public class JobManagerNomination : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                SendPartFunctions obj = new SendPartFunctions();
                obj.PendingNominationJobs();
            }
            catch(Exception ex)
            {

            }
            //Console.WriteLine("Nomination Job hit (check every 10 Sec). {0}", DateTime.Now);
            
        }
    }
    #endregion
    #endregion
    public class WatchlistWhenAvailableExecutionJob : IJob
    {
        IWatchlistService watchlistservice;
        INotifierEntityService notifierEntityService;


        public WatchlistWhenAvailableExecutionJob(INotifierEntityService notifierEntityService, IWatchlistService watchlistservice)
        {
            this.watchlistservice = watchlistservice;
            this.notifierEntityService = notifierEntityService;
        }

        public void Execute(IJobExecutionContext context)
        {
            //var resultoacy = watchlistservice.ExecuteWatchList(WatchlistAlertFrequency.Daily, EnercrossDataSets.OACY);
            //var resultunsc = watchlistservice.ExecuteWatchList(WatchlistAlertFrequency.Daily, EnercrossDataSets.UNSC);
            //var resultswnt = watchlistservice.ExecuteWatchList(WatchlistAlertFrequency.Daily, EnercrossDataSets.SWNT);
            //Console.Write("Watch List Alert Mail (when data available)- Schedular Working.. " + DateTime.Now.ToString());

            var entityoacy = notifierEntityService.GetNotifierEntityOfOACY();
            Action<String> dispatcher = (t) => { watchlistservice.ExecuteWatchList(WatchlistAlertFrequency.WhenAvailable, EnercrossDataSets.OACY); };
            PushSqlDependency.Instance(NotifierEntity.FromJson(entityoacy), dispatcher,false);

            var entityswnt = notifierEntityService.GetNotifierEntityOfSWNT();
            Action<String> dispatcherSwnt = (t) => { watchlistservice.ExecuteWatchList(WatchlistAlertFrequency.WhenAvailable, EnercrossDataSets.SWNT); };
            PushSqlDependency.Instance(NotifierEntity.FromJson(entityswnt), dispatcherSwnt,false);


            var entityunsc = notifierEntityService.GetNotifierEntityOfUNSC();
            Action<String> dispatcherUnsc = (t) => { watchlistservice.ExecuteWatchList(WatchlistAlertFrequency.WhenAvailable, EnercrossDataSets.UNSC); };
            PushSqlDependency.Instance(NotifierEntity.FromJson(entityunsc), dispatcherUnsc,false);

        }

    }
}