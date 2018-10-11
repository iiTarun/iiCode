[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Nom1Done.ReceiveUI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Nom1Done.ReceiveUI.App_Start.NinjectWebCommon), "Stop")]

namespace Nom1Done.ReceiveUI.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Nom1Done.Data.Repositories;
    using Nom1Done.Common;
    using Nom1Done.Service.Interface;
    using Nom1Done.Infrastructure;
    using UPRD.Data.Repositories;
    using Nom1Done.Service;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
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

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IDbFactory>().To<DbFactory>();
            kernel.Bind<IInboxRepository>().To<InboxRepository>();
            kernel.Bind<IJobWorkflowRepository>().To<JobWorkflowRepository>();
            kernel.Bind<ISettingRepository>().To<SettingRepository>();
            kernel.Bind<IPipelineRepository>().To<PipelineRepository>();
            kernel.Bind<IIncomingDataRepository>().To<IncomingDataRepository>();
            kernel.Bind<ITaskMgrJobsRepository>().To<TaskMgrJobsRepository>();
            kernel.Bind<IGISBInboxRepository>().To<GISBInboxRepository>();
            kernel.Bind<IApplicationLogRepository>().To<ApplicationLogRepository>();
            kernel.Bind<ImetadataErrorCodeRepository>().To<metadataErrorCodeRepository>();
            kernel.Bind<IEmailTemplateRepository>().To<EmailTemplateRepository>();
            kernel.Bind<IEmailQueueRepository>().To<EmailQueueRepository>();
            kernel.Bind<IManageIncomingRequestService>().To<ManageIncomingRequests>();
            kernel.Bind<IShipperCompanyRepository>().To<ShipperCompanyRepository>();

            kernel.Bind<UPRD.Infrastructure.IDbFactory>().To<UPRD.Infrastructure.DbFactory>();
            kernel.Bind<UPRD.Service.Interface.IModalFactory>().To<UPRD.Service.ModalFactory>();
            kernel.Bind<IUPRDApplicationLogRepository>().To<UPRDApplicationLogRepository>();
            kernel.Bind<IUprdInboxRepository>().To<UprdInboxRepository>();
            kernel.Bind<IUprdJobWorkflowRepository>().To<UprdJobWorkflowRepository>();
            kernel.Bind<IUprdSettingRepository>().To<UprdSettingRepository>();
            kernel.Bind<IUprdPipelineRepository>().To<UprdPipelineRepository>();
            kernel.Bind<IUprdIncomingDataRepository>().To<UprdIncomingDataRepository>();
            kernel.Bind<IUprdTaskMgrJobsRepository>().To<UprdTaskMgrJobsRepository>();
            kernel.Bind<IUprdGISBInboxRepository>().To<UprdGISBInboxRepository>();
            kernel.Bind<IUprdmetadataErrorCodeRepository>().To<UprdmetadataErrorCodeRepository>();
            kernel.Bind<IUprdEmailTemplateRepository>().To<UprdEmailTemplateRepository>();
            kernel.Bind<IUprdEmailQueueRepository>().To<UprdEmailQueueRepository>();
            kernel.Bind<IUprdShipperCompanyRepository>().To<UprdShipperCompanyRepository>();
            kernel.Bind<IModalFactory>().To<ModalFactory>();
            kernel.Bind<ITransactionalReportingRepository>().To<TransactionalReportingRepository>();
            kernel.Bind<ICounterPartyRepository>().To<CounterPartyRepository>();
            kernel.Bind<ILocationRepository>().To<LocationRepository>();
            kernel.Bind<IContractRepository>().To<ContractRepository>();
            kernel.Bind<ImetadataRequestTypeRepository>().To<metadataRequestTypeRepository>();
            kernel.Bind<ImetadataTransactionTypeRepository>().To<metadataTransactionTypeRepository>();
            kernel.Bind<ISQTSTrackOnNomRepository>().To<SQTSTrackOnNomRepository>();
            kernel.Bind<ImetadataCycleRepository>().To<metadataCycleRepository>();
            kernel.Bind<IPipelineEDISettingRepository>().To<PipelineEDISettingRepository>();
            kernel.Bind<INominationStatusRepository>().To<NominationStatusRepository>();
            kernel.Bind<IBatchRepository>().To<BatchRepository>();
            kernel.Bind<INMQRPerTransactionRepository>().To<NMQRPerTransactionRepository>();
            kernel.Bind<IOACYRepository>().To<OACYRepository>();
            kernel.Bind<IUNSCRepository>().To<UNSCRepository>();
            kernel.Bind<ISQTSPerTransactionRepository>().To<SQTSPerTransactionRepository>();
        }
    }
}
