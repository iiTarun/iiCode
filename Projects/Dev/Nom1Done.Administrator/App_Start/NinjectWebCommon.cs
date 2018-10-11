[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Nom1Done.Admin.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Nom1Done.Admin.App_Start.NinjectWebCommon), "Stop")]

namespace Nom1Done.Admin.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using System.Web.Http;

    using UPRD.Infrastructure;
    using UPRD.Data.Repositories;
    using UPRD.Service.Interface;
    using UPRD.Service;
    using UPRD.Services.Interface;
    using UPRD.Services.Services;
    using Ninject.Web.Common.WebHost;

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

            kernel.Bind<IModalFactory>().To<ModalFactory>();
            kernel.Bind<IClientEnvironmentSettingsRepository>().To<ClientEnvironmentSettingsRepository>();
            kernel.Bind<IClientEnvironmentSettingsService>().To<ClientEnvironmentSettingsService>();
            kernel.Bind<IDashNominationStatusService>().To<DashNominationStatusService>();
            kernel.Bind<IUprdDashNominationStatus>().To<UprdDashNominationStatus>();
            kernel.Bind<INotifierEntityService>().To<NotifierEntityService>();
            kernel.Bind<IUPRDCounterPartyRepository>().To<UprdCounterPartyRepository>();
            kernel.Bind<IUPRDCounterPartyService>().To<CounterPartyService>();
            kernel.Bind<IUPRDLocationRepository>().To<UPRDLocationRepository>();
            kernel.Bind<IUprdLocationService>().To<UprdLocationService>();
            kernel.Bind<IUprdPipelineRepository>().To<UprdPipelineRepository>();
            kernel.Bind<IUprdPipelineService>().To<UprdPipelineService>();
            kernel.Bind<IUprdmetadataDatasetRepository>().To<UprdmetadataDatasetRepository>();
            kernel.Bind<IUprdTransactionTypeService>().To<UprdTransactionTypeService>();
            kernel.Bind<IUPRDTransactionTypeRepository>().To<UprdTransactionTypeRepository>();
            
            kernel.Bind<ICrashFileRepository>().To<CrashFileRepository>();
            kernel.Bind<ICrashFileService>().To<CrashFileService>();
            kernel.Bind<IUprdPipelineTransactionTypeMapRepository>().To<UprdPipelineTransactionTypeMapRepository>();
            kernel.Bind<IUprdPipeTransTypeMapService>().To<UprdPipeTransTypeMapService>();
            kernel.Bind<IUPRDUserRegistrationRepository>().To<UPRDUserRegistrationRepository>();
            kernel.Bind<IUprdUserRegistrationService>().To<UprdUserRegistrationService>();
        }        
    }
}
