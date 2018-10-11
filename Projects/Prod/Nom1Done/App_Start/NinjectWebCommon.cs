[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Nom1Done.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Nom1Done.App_Start.NinjectWebCommon), "Stop")]

namespace Nom1Done.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Nom1Done.Infrastructure;
    using Service;
    using Service.Interface;
    using Data.Repositories;
    using System.Web.Http;

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

                LocalNinjectDependencyResolver resolver = new LocalNinjectDependencyResolver(kernel);
                GlobalConfiguration.Configuration.DependencyResolver = resolver;

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
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            kernel.Bind<IDbFactory>().To<DbFactory>();
            
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
        

            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IModalFactory>().To<ModalFactory>();
            kernel.Bind<IShipperRepository>().To<ShipperRepository>();
            
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

          

            kernel.Bind<INotifierEntityService>().To<NotifierEntityService>();

            kernel.Bind<IPipelineEDISettingRepository>().To<PipelineEDISettingRepository>();
            kernel.Bind<IPipelineEDISettingService>().To<PipelineEDISettingService>();
            kernel.Bind<IUprdStatusService>().To<UprdStatusService>();
            kernel.Bind<IUPRDStatuRepository>().To<UPRDStatuRepository>();

            kernel.Bind<IIdentityUsersRepo>().To<IdentityUsersRepo>();
            kernel.Bind<INonPathedService>().To<NonPathedService>();

            kernel.Bind<ISQTSRepository>().To<SQTSRepository>();
            kernel.Bind<IRouteRepository>().To<RouteRepository>();
            kernel.Bind<ISQTSOPPerTransactionRepository>().To<SQTSOPPerTransactionRepository>();
            kernel.Bind<IShipperCompSubShipperCompRepository>().To<ShipperCompSubShipperCompRepository>();
            kernel.Bind<IPasswordHistoryRepository>().To<PasswordHistoryRepository>();
            kernel.Bind<IPasswordHistoryService>().To<PasswordHistoryService>();
        }
    }
}
