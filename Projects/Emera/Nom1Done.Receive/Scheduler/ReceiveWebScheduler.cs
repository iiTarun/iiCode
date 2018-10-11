using Ninject;
using Nom1Done.Data.Repositories;
using Nom1Done.DTO;
using Nom1Done.Infrastructure;
using Nom1Done.Service;
using Nom1Done.Service.Interface;
using Quartz;
using Quartz.Impl;
using System;
using System.Reflection;

namespace Nom1Done.Receive.Scheduler
{
    public class ReceiveWebScheduler : Ninject.Modules.NinjectModule
    {
        #region Engine Inventory
        #region services Obj
        static ISettingRepository _serviceSetting = null;
        #endregion
        #region Quartz Troops
        public static StdSchedulerFactory _scheduleFactory;
        public static IScheduler _jobScheduler;
        #endregion
        #region Receive Inventory
        static string TimeAndFreqForReceiveFileProcess;
        #endregion
        #endregion
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
            
            Bind<IUPRDStatuRepository>().To<UPRDStatuRepository>();
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
            Bind<ITransportationServiceProviderRepository>().To<TransportationServiceProviderRepository>();
            Bind<IShipperCompanyRepository>().To<ShipperCompanyRepository>();
            Bind<ITradingPartnerWorksheetRepository>().To<TradingPartnerWorksheetRepository>();
            Bind<ImetadataDatasetRepository>().To<metadataDatasetRepository>();
            Bind<IGISBOutboxRepository>().To<GISBOutboxRepository>();
            Bind<INominationStatusRepository>().To<NominationStatusRepository>();
            Bind<ISQTSTrackOnNomRepository>().To<SQTSTrackOnNomRepository>();
            Bind<ImetadataCycleRepository>().To<metadataCycleRepository>();
            Bind<IOutbox_MultipartFormRepository>().To<Outbox_MultipartFormRepository>();

            Bind<IFileSysIncomingDataRepository>().To<FileSysIncomingDataRepository>();
            Bind<IIncomingDataRepository>().To<IncomingDataRepository>();
            Bind<ITaskMgrReceiveMultipuleFileRepository>().To<TaskMgrReceiveMultipuleFileRepository>();
          
            Bind<IRURDRepository>().To<RURDRepository>();
            Bind<IRURD_DateTime_HeadRepository>().To<RURD_DateTime_HeadRepository>();
            Bind<IRURD_LINDetail_RefrenceNumberRepository>().To<RURD_LINDetail_RefrenceNumberRepository>();
            Bind<IRURD_LINDetailSegmentRepository>().To<RURD_LINDetailSegmentRepository>();
            Bind<IRURD_Name_HeadRepository>().To<RURD_Name_HeadRepository>();
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

        public static void start()
        {
            #region settings initialize for sending file 
            StandardKernel Kernal = new StandardKernel();
            Kernal.Load(Assembly.GetExecutingAssembly());
            _serviceSetting = Kernal.Get<SettingRepository>();
            #endregion
            #region Quartz Servcie Initialize Job
            _scheduleFactory = new StdSchedulerFactory();
            _jobScheduler = _scheduleFactory.GetScheduler();
            _jobScheduler.Start();
            #endregion
            #region job for receive files processing
            ProcessReceiveFile_JobCreation(_jobScheduler);
            #endregion
        }
        #region Job Creation for processing the receive files 
        private static void ProcessReceiveFile_JobCreation(IScheduler _jobScheduler)
        {
            try
            {
                TimeAndFreqForReceiveFileProcess = _serviceSetting.GetById((int)Settings.TimeAndFreqForReceiveFileProcess).Value;
                IJobDetail EncEDIGenerationJobDetail = JobBuilder.Create<JobManagerReceiveFileProcessing>()
                                                    .WithIdentity(string.Format("{0}", "ReceiveFileJob"))
                                                    .Build();
                ITrigger EncEDIGenerationJobTrigger = TriggerBuilder.Create()
                                                    .WithIdentity(string.Format("{0}", "ReceiveFileJob"))
                                                    .StartNow()
                                                    .WithCronSchedule("0/5 0/1 * 1/1 * ? *")
                                                    .Build();
                _jobScheduler.ScheduleJob(EncEDIGenerationJobDetail, EncEDIGenerationJobTrigger);
            }
            catch (Exception ex)
            {
                
            }
        }
        #endregion
    }
    public class JobManagerReceiveFileProcessing : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                ReceivePartModified obj = new ReceivePartModified();
                obj.ProcessFiles();
            }
            catch (Exception ex)
            {
            }

        }
    }
}