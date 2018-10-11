//using Nom1Done.Service;
//using Nom1Done.Service.Interface;
using UPRD.Data.Repositories;
using UPRD.Infrastructure;
using UPRD.Service;
using UPRD.Service.Interface;

namespace Nom1Done.EDIEngineSendAndReceive
{
    public class NinjectBindings: Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IDbFactory>().To<DbFactory>();
            Bind<IModalFactory>().To<ModalFactory>();
            //Bind<IPipelineService>().To<PipelineService>();            
            Bind<IUPRDApplicationLogRepository>().To<UPRDApplicationLogRepository>();
            //Bind<IApplicationLogService>().To<ApplicationLogService>();
            //Bind<ISettingService>().To<SettingService>();
            Bind<IUnitOfWork>().To<UnitOfWork>();

            Bind<IUprdOutboxRepository>().To<UprdOutboxRepository>();
            Bind<IUprdStatuRepository>().To<UprdStatuRepository>();
            Bind<IUprdTaskMgrJobsRepository>().To<UprdTaskMgrJobsRepository>();
            //Bind<IUprdOutboxRepository>().To<UprdOutboxRepository>();
            Bind<IUprdTransactionLogRepository>().To<UprdTransactionLogRepository>();
            Bind<IUprdJobWorkflowRepository>().To<UprdJobWorkflowRepository>();
            Bind<IUprdJobStackErrorLogRepository>().To<UprdJobStackErrorLogRepository>();
            Bind<IUprdTaskMgrXmlRepository>().To<UprdTaskMgrXmlRepository>();
            Bind<IUprdmetadataPipelineEncKeyInfoRepository>().To<UprdmetadataPipelineEncKeyInfoRepository>();
            Bind<IUprdTradingPartnerWorksheetRepository>().To<UprdTradingPartnerWorksheetRepository>();
            Bind<IUprdGISBOutboxRepository>().To<UprdGISBOutboxRepository>();
            Bind<IUprdOutbox_MultipartFormRepository>().To<UprdOutbox_MultipartFormRepository>();
            Bind<IUprdIncomingDataRepository>().To<UprdIncomingDataRepository>();
            Bind<IUprdOACYRepository>().To<UprdOACYRepository>();
            Bind<IUprdUNSCRepository>().To<UprdUNSCRepository>();
            Bind<IUprdSWNTPerTransactionRepository>().To<UprdSWNTPerTransactionRepository>();
            Bind<IUprdPipelineEDISettingRepository>().To<UprdPipelineEDISettingRepository>();
     
            Bind<IUprdSettingRepository>().To<UprdSettingRepository>();
            Bind<IUprdFileSysIncomingDataRepository>().To<UprdFileSysIncomingDataRepository>();
            Bind<IUPRDCounterPartyRepository>().To<UprdCounterPartyRepository>();
            Bind<IUPRDLocationRepository>().To<UPRDLocationRepository>();
            Bind<IUprdPipelineRepository>().To<UprdPipelineRepository>();
            Bind<IUPRDTransactionTypeRepository>().To<UprdTransactionTypeRepository>();
            Bind<IUprdPipelineTransactionTypeMapRepository>().To<UprdPipelineTransactionTypeMapRepository>();
            Bind<ICrashFileRepository>().To<CrashFileRepository>();
            Bind<IClientEnvironmentSettingsRepository>().To<ClientEnvironmentSettingsRepository>();
        }
    }
}
