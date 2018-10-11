using Ninject;
using Ninject.Syntax;
using Ninject.Web.Common;
using Nom1Done.Data.Repositories;
using Nom1Done.DTO;
using Nom1Done.Infrastructure;
using Nom1Done.Service;
using Nom1Done.Service.Interface;
using Quartz;
using Quartz.Impl;
using System;
using System.Web;


namespace Nom1Done.Schedular
{
    public class WatchlistdailyExecutionJobScheduler:Ninject.Modules.NinjectModule
    {
        #region Engine Inventory
        #region services Obj
        static ISettingRepository _serviceSetting = null;
        #endregion
        #region Quartz Troops
        public static StdSchedulerFactory _scheduleFactory;
        public static IScheduler _jobScheduler;
        #endregion
        #region Send Inventory
        static string TimeAndFreqForManualCheck, UprdReqTimeForOacy, UprdReqTimeForUnsc, UprdReqTimeForSwnt;
        #endregion
        #region Receive Inventory
        static string TimeAndFreqForReceiveFileProcess;
        #endregion
        #endregion
        static IWatchlistService watchlistservice;  
        public static void Start()
        {
            var kernal = InitializeKernal();
            watchlistservice= kernal.Get<WatchlistService>();
            _serviceSetting = kernal.Get<SettingRepository>();
            IScheduler scheduler = GetSchedule(kernal); 
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<WatchlistdailyExecutionJob>().Build();
            IJobDetail jobWhenAvailData = JobBuilder.Create<WatchlistWhenAvailableExecutionJob>().Build();

            //uprd
            IJobDetail UprdjobDaily = JobBuilder.Create<CustomUPRDJob>().Build();

            ITrigger uprdTrigger=TriggerBuilder.Create()
                .WithIdentity("uprdTrigger","uprdGroup").StartNow()
                .WithDailyTimeIntervalSchedule(
                s => s.WithIntervalInHours(24)
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(23, 00))
                .InTimeZone(TimeZoneInfo.Utc))
              .Build();

            scheduler.ScheduleJob(UprdjobDaily, uprdTrigger);

            #region match nomination job
            IJobDetail matchNomJob = JobBuilder.Create<MatchNomination>().Build();
        
            ITrigger matchNomTrigger = TriggerBuilder.Create()
                .WithIdentity("matchNomTrigger", "matchNomGroup")
                .StartNow()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(1)
                    .OnEveryDay()
                  )
                .Build();

            scheduler.ScheduleJob(matchNomJob, matchNomTrigger);
            #endregion

            #region Sqts job for 2nd scenerio
            IJobDetail SQTS2ndScenerioJob = JobBuilder.Create<SqtsScenerio2nd>().Build();

            ITrigger SQTS2ndScenerioTrigger = TriggerBuilder.Create()
                .WithIdentity("SQTS2ndScenerioTrigger", "SQTS2ndScenerioGroup")
                .StartNow()
                .WithDailyTimeIntervalSchedule(
                a=>a.WithIntervalInHours(1)
                )
                .Build();

            scheduler.ScheduleJob(SQTS2ndScenerioJob, SQTS2ndScenerioTrigger);
            #endregion


            ITrigger Testtrigger = TriggerBuilder.Create()
            .WithIdentity("trigger1", "group1")                     
            .StartNow()          
            .Build();


            ITrigger realtrigger = TriggerBuilder.Create()
              .WithIdentity("trigger2", "group2")
              .WithDailyTimeIntervalSchedule(
                s => s.WithIntervalInHours(24)
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(11,00))      // UTC=11:00AM ==> CST=05:00AM (Central America)
                .InTimeZone(TimeZoneInfo.Utc))
              .Build();

             scheduler.ScheduleJob(job, realtrigger);
             scheduler.ScheduleJob(jobWhenAvailData, Testtrigger);
            
  
            // for testing only
            ITrigger Test1 = TriggerBuilder.Create()
            .WithIdentity("trigger3", "group3")
            .StartNow()
            .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(900)
            .RepeatForever())
            .Build();
            // scheduler.ScheduleJob(job, Test1);

            #region Engine job for nomination send

            #endregion
            #region Engine job for UPRD send

            #endregion

            #region Quartz Servcie Initialize Job
            _scheduleFactory = new StdSchedulerFactory();
            _jobScheduler = _scheduleFactory.GetScheduler();
            _jobScheduler.Start();
            #endregion
            //#region job for receive files processing
            //ProcessReceiveFile_JobCreation(_jobScheduler);
            //#endregion
            #region Jobs for file sending
            #region job for Nomination
            //job for Nomination
            NominationJobCreation(_jobScheduler);
            #endregion
            //#region job for Uprd (OACY)
            ////job for Uprd (OACY)
            //UprdOacyJobCreation(_jobScheduler);
            //#endregion
            //#region Job for Uprd (UNSC)
            ////Job for Uprd (UNSC)
            //UprdUnscJobCreation(_jobScheduler);
            //#endregion
            //#region Job for Uprd (SWNT)
            ////Job for Uprd (SWNT)
            //UprdSwntJobCreation(_jobScheduler);
            //#endregion
            #endregion
        }


        private static IKernel InitializeKernal()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            kernel.Bind<IDbFactory>().To<DbFactory>();

            kernel.Bind<IOACYService>().To<OACYService>();
            kernel.Bind<IOACYRepository>().To<OACYRepository>();

            kernel.Bind<IUNSCRepository>().To<UNSCRepository>();
            kernel.Bind<IUNSCService>().To<UNSCService>();

            kernel.Bind<INoticesRepository>().To<NoticesRepository>();
            kernel.Bind<INoticesService>().To<NoticesService>();

            kernel.Bind<IPipelineRepository>().To<PipelineRepository>();
            kernel.Bind<IShipperCompanyRepository>().To<ShipperCompanyRepository>();

            kernel.Bind<INominationsRepository>().To<NominationsRepository>();
            kernel.Bind<IBatchRepository>().To<BatchRepository>();

            kernel.Bind<IDashboardService>().To<DashboardService>();
            kernel.Bind<ILocationRepository>().To<LocationRepository>();
            kernel.Bind<ILocationService>().To<LocationService>();

            kernel.Bind<IContractRepository>().To<ContractRepository>();
            kernel.Bind<ImetadataRequestTypeRepository>().To<metadataRequestTypeRepository>();
            kernel.Bind<ImetadataRequestTypeService>().To<metadataRequestTypeService>();

            kernel.Bind<IContractService>().To<ContractService>();
            kernel.Bind<IPipelineService>().To<PipelineService>();

            kernel.Bind<IPipelineStatusRepository>().To<PipelineStatusRepository>();
            kernel.Bind<IPipelineStatusService>().To<PipelineStatusService>();

            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IModalFactory>().To<ModalFactory>();
            kernel.Bind<IShipperRepository>().To<ShipperRepository>();

            kernel.Bind<ISWNTPerTransactionRepository>().To<SWNTPerTransactionRepository>();
            kernel.Bind<ISWNTPerTransactionService>().To<SWNTPerTransactionService>();

            kernel.Bind<IPathedNominationService>().To<PathedNominationService>();
            kernel.Bind<IPNTNominationService>().To<PNTNominationService>();

            kernel.Bind<ICounterPartyRepository>().To<CounterPartyRepository>();
            kernel.Bind<ImetadataCapacityTypeIndicatorRepository>().To<metadataCapacityTypeIndicatorRepository>();
            kernel.Bind<ImetadataCycleRepository>().To<metadataCycleRepository>();
            kernel.Bind<ImetadataTransactionTypeRepository>().To<metadataTransactionTypeRepository>();
            kernel.Bind<INMQRPerTransactionRepository>().To<NMQRPerTransactionRepository>();

            kernel.Bind<INominationStatusRepository>().To<NominationStatusRepository>();
            kernel.Bind<IOutboxRepository>().To<OutboxRepository>();
            kernel.Bind<IPipelineTransactionTypeMapRepository>().To<PipelineTransactionTypeMapRepository>();
            kernel.Bind<ITransactionLogRepository>().To<TransactionLogRepository>();

            kernel.Bind<ITaskMgrJobsRepository>().To<TaskMgrJobsRepository>();
            kernel.Bind<ISQTSTrackOnNomRepository>().To<SQTSTrackOnNomRepository>();
            kernel.Bind<ImetadataFileStatusRepositpry>().To<metadataFileStatusRepository>();
            kernel.Bind<ImetadataQuantityTypeRepository>().To<metadataQuantityTypeIndicatorRepository>();
            kernel.Bind<ImetadataBidUpIndicatorRepository>().To<metadataBidUpIndicatorRepository>();
            kernel.Bind<ImetadataExportDeclarationRepository>().To<metadataExportDeclarationRepository>();
            kernel.Bind<ImetadataExportDeclarationService>().To<metadataExportDeclarationService>();
            kernel.Bind<ImetadataBidUpIndicatorService>().To<metadataBidUpIndicatorService>();
            kernel.Bind<ImetadataCapacityTypeIndicatorService>().To<metadataCapacityTypeIndicatorService>();

            kernel.Bind<ImetadataFileStatusService>().To<metadataFileStatusService>();
            kernel.Bind<ImetadataQuantityTypeIndicatorService>().To<metadataQuantityTypeIndicatorService>();
            kernel.Bind<ICycleIndicator>().To<CycleIndicatorService>();
            kernel.Bind<IEmailQueueRepository>().To<EmailQueueRepository>();
            kernel.Bind<IEmailTemplateRepository>().To<EmailTemplateRepository>();
            kernel.Bind<IEmailQueueService>().To<EmailQueuService>();
            kernel.Bind<IEmailTemplateService>().To<EmailTemplateService>();

            kernel.Bind<IUploadedNominationRepository>().To<UploadNominationRepository>();
            kernel.Bind<IUploadNominationService>().To<UploadNominationService>();
            kernel.Bind<IBatchService>().To<BatchService>();
            kernel.Bind<ITaskMgrReceiveMultipuleFileRepository>().To<TaskMgrReceiveMultipuleFileRepository>();
            kernel.Bind<ICounterPartiesService>().To<CounterPartyService>();

            kernel.Bind<ITransactionalReportingService>().To<TransactionalReportingService>();
            kernel.Bind<ITransactionalReportingRepository>().To<TransactionalReportingRepository>();

            kernel.Bind<ISettingRepository>().To<SettingRepository>();
            kernel.Bind<IWatchlistService>().To<WatchlistService>();
            kernel.Bind<IDataTypeGroupingRepository>().To<DataTypeGroupingRepository>();
            kernel.Bind<ILogicalOperatorRepository>().To<LogicalOperatorRepository>();
            kernel.Bind<IMasterColumnRepository>().To<MasterColumnRepository>();
            kernel.Bind<IWatchlistRepository>().To<WatchlistRepository>();
            kernel.Bind<IWatchlistRuleRepository>().To<WatchlistRuleRepository>();
            kernel.Bind<IWatchListPipelineMappingRepository>().To<WatchListPipelineMappingRepository>();
            kernel.Bind<IWatchListMailAlertUPRDMappingRepo>().To<WatchListMailAlertUPRDMappingRepo>();

            kernel.Bind<INotifierEntityService>().To<NotifierEntityService>();

            kernel.Bind<IPipelineEDISettingRepository>().To<PipelineEDISettingRepository>();
            kernel.Bind<IPipelineEDISettingService>().To<PipelineEDISettingService>();
            kernel.Bind<IUprdStatusService>().To<UprdStatusService>();
            kernel.Bind<IUPRDStatuRepository>().To<UPRDStatuRepository>();
            kernel.Bind<IIdentityUsersRepo>().To<IdentityUsersRepo>();
            kernel.Bind<IRouteRepository>().To<RouteRepository>();
            kernel.Bind<ISQTSPerTransactionRepository>().To<SQTSPerTransactionRepository>();

            #region Engine bindings
            //kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            //kernel.Bind<IDbFactory>().To<DbFactory>();
            //kernel.Bind<IModalFactory>().To<ModalFactory>();
            
            //kernel.Bind<IPipelineRepository>().To<PipelineRepository>();
            //kernel.Bind<IPipelineService>().To<PipelineService>();
            //kernel.Bind<ISettingRepository>().To<SettingRepository>();
            kernel.Bind<IApplicationLogRepository>().To<ApplicationLogRepository>();
            kernel.Bind<IApplicationLogService>().To<ApplicationLogService>();
            kernel.Bind<ISettingService>().To<SettingService>();
            //kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            //kernel.Bind<IOutboxRepository>().To<OutboxRepository>();
            //kernel.Bind<IUPRDRepository>().To<UPRDRepository>();
            //kernel.Bind<IUPRDStatuRepository>().To<UPRDStatuRepository>();
            //kernel.Bind<IUPRD_DataRequestCodeRepository>().To<UPRD_DataRequestCodeRepository>();
            //kernel.Bind<IUPRD_DateTimeRefRepository>().To<UPRD_DateTimeRefRepository>();
            //kernel.Bind<IUPRD_NameHeadRepository>().To<UPRD_NameHeadRepository>();
            //kernel.Bind<ITaskMgrJobsRepository>().To<TaskMgrJobsRepository>();
            kernel.Bind<ITPWBrowserTestResultRepository>().To<TPWBrowserTestResultRepository>();
            
            //kernel.Bind<IBatchRepository>().To<BatchRepository>();
            //kernel.Bind<IOutboxRepository>().To<OutboxRepository>();
            //kernel.Bind<ITransactionLogRepository>().To<TransactionLogRepository>();
            kernel.Bind<IJobWorkflowRepository>().To<JobWorkflowRepository>();
            //kernel.Bind<INominationsRepository>().To<NominationsRepository>();
            kernel.Bind<IJobStackErrorLogRepository>().To<JobStackErrorLogRepository>();
            kernel.Bind<ITaskMgrXmlRepository>().To<TaskMgrXmlRepository>();
            kernel.Bind<ImetadataPipelineEncKeyInfoRepository>().To<metadataPipelineEncKeyInfoRepository>();
            kernel.Bind<IUPRDRepository>().To<UPRDRepository>();
            kernel.Bind<ITransportationServiceProviderRepository>().To<TransportationServiceProviderRepository>();
            //kernel.Bind<IShipperCompanyRepository>().To<ShipperCompanyRepository>();
            kernel.Bind<ITradingPartnerWorksheetRepository>().To<TradingPartnerWorksheetRepository>();
            kernel.Bind<IUPRD_DateTimeRefRepository>().To<UPRD_DateTimeRefRepository>();
            kernel.Bind<IUPRD_NameHeadRepository>().To<UPRD_NameHeadRepository>();
            kernel.Bind<IUPRD_DataRequestCodeRepository>().To<UPRD_DataRequestCodeRepository>();
            kernel.Bind<ImetadataDatasetRepository>().To<metadataDatasetRepository>();
            kernel.Bind<IGISBOutboxRepository>().To<GISBOutboxRepository>();
            //kernel.Bind<INominationStatusRepository>().To<NominationStatusRepository>();
            //kernel.Bind<ISQTSTrackOnNomRepository>().To<SQTSTrackOnNomRepository>();
            //kernel.Bind<ImetadataCycleRepository>().To<metadataCycleRepository>();
            kernel.Bind<IOutbox_MultipartFormRepository>().To<Outbox_MultipartFormRepository>();
            
            kernel.Bind<IFileSysIncomingDataRepository>().To<FileSysIncomingDataRepository>();
            kernel.Bind<IIncomingDataRepository>().To<IncomingDataRepository>();
            //kernel.Bind<ITaskMgrReceiveMultipuleFileRepository>().To<TaskMgrReceiveMultipuleFileRepository>();
            //kernel.Bind<IOACYRepository>().To<OACYRepository>();
            //kernel.Bind<IUNSCRepository>().To<UNSCRepository>();
            kernel.Bind<IRURDRepository>().To<RURDRepository>();
            //kernel.Bind<IRURD_DateTime_HeadRepository>().To<RURD_DateTime_HeadRepository>();
            //kernel.Bind<IRURD_LINDetail_RefrenceNumberRepository>().To<RURD_LINDetail_RefrenceNumberRepository>();
            //kernel.Bind<IRURD_LINDetailSegmentRepository>().To<RURD_LINDetailSegmentRepository>();
            //kernel.Bind<IRURD_Name_HeadRepository>().To<RURD_Name_HeadRepository>();
            //kernel.Bind<ISWNTPerTransactionRepository>().To<SWNTPerTransactionRepository>();
            //kernel.Bind<INMQR_HeaderRepository>().To<NMQR_HeaderRepository>();
            //kernel.Bind<INMQR_NameHeadRepository>().To<NMQR_NameHeadRepository>();
            //kernel.Bind<INMQR_InformationRepository>().To<NMQR_InformationRepository>();
            //kernel.Bind<INMQR_ReferenceIdentification_N9Repository>().To<NMQR_ReferenceIdentification_N9Repository>();
            //kernel.Bind<INMQR_RefIdentificationRepository>().To<NMQR_RefIdentificationRepository>();
            //kernel.Bind<INMQR_SublineItemDetailRepository>().To<NMQR_SublineItemDetailRepository>();
            //kernel.Bind<INMQRPerTransactionRepository>().To<NMQRPerTransactionRepository>();
            kernel.Bind<ISQTSFileRepository>().To<SQTSFileRepository>();
            //kernel.Bind<ISQTSPerTransactionRepository>().To<SQTSPerTransactionRepository>();
            //kernel.Bind<ITransactionalReportingRepository>().To<TransactionalReportingRepository>();
            //kernel.Bind<ICounterPartyRepository>().To<CounterPartyRepository>();
            //kernel.Bind<ILocationRepository>().To<LocationRepository>();
            //kernel.Bind<IContractRepository>().To<ContractRepository>();
            //kernel.Bind<ImetadataRequestTypeRepository>().To<metadataRequestTypeRepository>();
            //kernel.Bind<ImetadataTransactionTypeRepository>().To<metadataTransactionTypeRepository>();
            //kernel.Bind<IPipelineEDISettingRepository>().To<PipelineEDISettingRepository>();
            kernel.Bind<ISQTSOPPerTransactionRepository>().To<SQTSOPPerTransactionRepository>();
            #endregion


        }


        private static IScheduler GetSchedule(IResolutionRoot root)
        {
            var schedule = new StdSchedulerFactory().GetScheduler();

            schedule.JobFactory = new NInjectJobFactory(root);

            return schedule;
        }

        #region Job Creation for Dataset Sending
        #region Nomination Job Creation
        private static void NominationJobCreation(IScheduler _jobScheduler)
        {
            try
            {
                TimeAndFreqForManualCheck = _serviceSetting.GetById((int)Settings.TimeAndFreqForManualCheck).Value;
                Console.WriteLine("Nomination JobScheduler Start (check every 10 sec).");
                IJobDetail XMLGenerationJobDetail = JobBuilder.Create<JobManagerNomination>()
                                                    .WithIdentity(string.Format("{0}", "XMLGenerationJob"))
                                                    .Build();
                ITrigger XMLGenerationJobTrigger = TriggerBuilder.Create()
                                                    .WithIdentity(string.Format("{0}", "XMLGenerationJob"))
                                                    .StartNow()
                                                    .WithCronSchedule(TimeAndFreqForManualCheck)
                                                    .Build();
                _jobScheduler.ScheduleJob(XMLGenerationJobDetail, XMLGenerationJobTrigger);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in EDIGenerationJobCreation:- " + ex.Message);
            }
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IDbFactory>().To<DbFactory>();
            Bind<IModalFactory>().To<ModalFactory>();

            Bind<IPipelineRepository>().To<PipelineRepository>();
            Bind<IPipelineService>().To<PipelineService>();
            Bind<ISettingRepository>().To<SettingRepository>();
            Bind<IApplicationLogRepository>().To<ApplicationLogRepository>();
            Bind<IApplicationLogService>().To<ApplicationLogService>();
            Bind<ISettingService>().To<SettingService>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IOutboxRepository>().To<OutboxRepository>();
            Bind<IUPRDRepository>().To<UPRDRepository>();
            Bind<IUPRDStatuRepository>().To<UPRDStatuRepository>();
            Bind<IUPRD_DataRequestCodeRepository>().To<UPRD_DataRequestCodeRepository>();
            Bind<IUPRD_DateTimeRefRepository>().To<UPRD_DateTimeRefRepository>();
            Bind<IUPRD_NameHeadRepository>().To<UPRD_NameHeadRepository>();
            Bind<ITaskMgrJobsRepository>().To<TaskMgrJobsRepository>();
            Bind<ITPWBrowserTestResultRepository>().To<TPWBrowserTestResultRepository>();

            Bind<IBatchRepository>().To<BatchRepository>();
            Bind<IOutboxRepository>().To<OutboxRepository>();
            Bind<ITransactionLogRepository>().To<TransactionLogRepository>();
            Bind<IJobWorkflowRepository>().To<JobWorkflowRepository>();
            Bind<INominationsRepository>().To<NominationsRepository>();
            Bind<IJobStackErrorLogRepository>().To<JobStackErrorLogRepository>();
            Bind<ITaskMgrXmlRepository>().To<TaskMgrXmlRepository>();
            Bind<ImetadataPipelineEncKeyInfoRepository>().To<metadataPipelineEncKeyInfoRepository>();
            Bind<IUPRDRepository>().To<UPRDRepository>();
            Bind<ITransportationServiceProviderRepository>().To<TransportationServiceProviderRepository>();
            Bind<IShipperCompanyRepository>().To<ShipperCompanyRepository>();
            Bind<ITradingPartnerWorksheetRepository>().To<TradingPartnerWorksheetRepository>();
            Bind<IUPRD_DateTimeRefRepository>().To<UPRD_DateTimeRefRepository>();
            Bind<IUPRD_NameHeadRepository>().To<UPRD_NameHeadRepository>();
            Bind<IUPRD_DataRequestCodeRepository>().To<UPRD_DataRequestCodeRepository>();
            Bind<ImetadataDatasetRepository>().To<metadataDatasetRepository>();
            Bind<IGISBOutboxRepository>().To<GISBOutboxRepository>();
            Bind<INominationStatusRepository>().To<NominationStatusRepository>();
            Bind<ISQTSTrackOnNomRepository>().To<SQTSTrackOnNomRepository>();
            Bind<ImetadataCycleRepository>().To<metadataCycleRepository>();
            Bind<IOutbox_MultipartFormRepository>().To<Outbox_MultipartFormRepository>();

            Bind<IFileSysIncomingDataRepository>().To<FileSysIncomingDataRepository>();
            Bind<IIncomingDataRepository>().To<IncomingDataRepository>();
            Bind<ITaskMgrReceiveMultipuleFileRepository>().To<TaskMgrReceiveMultipuleFileRepository>();
            Bind<IOACYRepository>().To<OACYRepository>();
            Bind<IUNSCRepository>().To<UNSCRepository>();
            Bind<IRURDRepository>().To<RURDRepository>();
            Bind<IRURD_DateTime_HeadRepository>().To<RURD_DateTime_HeadRepository>();
            Bind<IRURD_LINDetail_RefrenceNumberRepository>().To<RURD_LINDetail_RefrenceNumberRepository>();
            Bind<IRURD_LINDetailSegmentRepository>().To<RURD_LINDetailSegmentRepository>();
            Bind<IRURD_Name_HeadRepository>().To<RURD_Name_HeadRepository>();
            Bind<ISWNTPerTransactionRepository>().To<SWNTPerTransactionRepository>();
            Bind<INMQR_HeaderRepository>().To<NMQR_HeaderRepository>();
            Bind<INMQR_NameHeadRepository>().To<NMQR_NameHeadRepository>();
            Bind<INMQR_InformationRepository>().To<NMQR_InformationRepository>();
            Bind<INMQR_ReferenceIdentification_N9Repository>().To<NMQR_ReferenceIdentification_N9Repository>();
            Bind<INMQR_RefIdentificationRepository>().To<NMQR_RefIdentificationRepository>();
            Bind<INMQR_SublineItemDetailRepository>().To<NMQR_SublineItemDetailRepository>();
            Bind<INMQRPerTransactionRepository>().To<NMQRPerTransactionRepository>();
            Bind<ISQTSFileRepository>().To<SQTSFileRepository>();
            Bind<ISQTSPerTransactionRepository>().To<SQTSPerTransactionRepository>();
            Bind<ITransactionalReportingRepository>().To<TransactionalReportingRepository>();
            Bind<ICounterPartyRepository>().To<CounterPartyRepository>();
            Bind<ILocationRepository>().To<LocationRepository>();
            Bind<IContractRepository>().To<ContractRepository>();
            Bind<ImetadataRequestTypeRepository>().To<metadataRequestTypeRepository>();
            Bind<ImetadataTransactionTypeRepository>().To<metadataTransactionTypeRepository>();
            Bind<IPipelineEDISettingRepository>().To<PipelineEDISettingRepository>();
            Bind<ISQTSOPPerTransactionRepository>().To<SQTSOPPerTransactionRepository>();
        }
        #endregion
        #endregion
    }
}