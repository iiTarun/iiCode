using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Nom1Done.Model;
using Nom1Done.Model.Models;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Nom1Done.Data
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

    }
    public class NomEntities : IdentityDbContext<ApplicationUser>
    {
        public NomEntities() : base("NomEntities")
        {

        }

        public DbSet<UserPipelineMapping> UserPipelineMapping { get; set; }
        public DbSet<Contract> Contract { get; set; }
        public DbSet<CounterParty> CounterParty { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<metadataBidUpIndicator> BidUpIndicators { get; set; }
        public DbSet<metadataCapacityTypeIndicator> metadataCapacityTypeIndicator { get; set; }
        public DbSet<metadataCycle> metadataCycle { get; set; }
        public DbSet<metadataExportDeclaration> metadataExportDeclaration { get; set; }
        public DbSet<metadataFileStatu> metadataFileStatu { get; set; }
        public DbSet<metadataQuantityTypeIndicator> metadataQuantityTypeIndicator { get; set; }
        public DbSet<metadataRequestType> metadataRequestType { get; set; }
        public DbSet<metadataTransactionType> metadataTransactionType { get; set; }
        public DbSet<Outbox> Outbox { get; set; }
        public DbSet<Pipeline> Pipeline { get; set; }
        public DbSet<ShipperCompany> ShipperCompany { get; set; }
        //public DbSet<SwntPerTransaction> SwntPerTransaction { get; set; }
        public DbSet<TaskMgrJob> TaskMgrJob { get; set; }
        public DbSet<TaskMgrReceiveMultipuleFile> TaskMgrReceiveMultipuleFile { get; set; }
        public DbSet<TPWBrowserTestResult> TPWBrowserTestResult { get; set; }
        public DbSet<TransactionLog> TransactionLog { get; set; }
        public DbSet<V4_Batch> V4_Batch { get; set; }
        public DbSet<V4_Nomination> V4_Nomination { get; set; }
        public DbSet<NominationStatu> NominationStatus { get; set; }
        public DbSet<Shipper> Shipper { get; set; }
       
        public DbSet<TransportationServiceProvider> TransportationServiceProvider { get; set; }
        public DbSet<metadataPipelineEncKeyInfo> metadataPipelineEncKeyInfo { get; set; }
        public DbSet<Pipeline_TransactionType_Map> Pipeline_TransactionType_Map { get; set; }
        public DbSet<TradingPartnerWorksheet> TradingPartnerWorksheet { get; set; }
        public DbSet<metadataErrorCode> metadataErrorCode { get; set; }
        public DbSet<metadataDataset> metadataDataset { get; set; }
        public DbSet<Setting> Setting { get; set; }
        public DbSet<PipelineEDISetting> PipelineEDISetting { get; set; }
        public DbSet<SQTSPerTransaction> SQTSPerTransaction { get; set; }

        public DbSet<Route> Routes { get; set; }
        public DbSet<FileSysIncomingData> FileSysIncomingData { get; set; }
        public DbSet<SQTSOPPerTransaction> SQTSOPPerTransaction { get; set; }
        public DbSet<ShipperCompSubShipperComp> ShipperCompSubShipperComp { get;set; }

        #region TransactionalReport

        public DbSet<TransactionalReport> TransactionalReport { get; set; }

        #endregion

        #region Engine DbSets
        public DbSet<ApplicationLog> ApplicationLogs { get; set; }
        public DbSet<EmailQueue> EmailQueues { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<ErrorCode> ErrorCodes { get; set; }
        public DbSet<GISBInbox> GISBInboxes { get; set; }
        public DbSet<GISBOutbox> GISBOutboxes { get; set; }
        public DbSet<Inbox> Inboxes { get; set; }
        public DbSet<IncomingData> IncomingDatas { get; set; }
        public DbSet<JobStackErrorLog> JobStackErrorLogs { get; set; }
        public DbSet<JobWorkflow> JobWorkflows { get; set; }
        public DbSet<NMQR_Header> NMQR_Header { get; set; }
        public DbSet<NMQR_Information> NMQR_Information { get; set; }
        public DbSet<NMQR_NameHead> NMQR_NameHead { get; set; }
        public DbSet<NMQR_ReferenceIdentification_N9> NMQR_ReferenceIdentification_N9 { get; set; }
        public DbSet<NMQR_RefIdentification> NMQR_RefIdentification { get; set; }
        public DbSet<NMQR_SublineItemDetail> NMQR_SublineItemDetail { get; set; }
        public DbSet<NMQRPerTransaction> NMQRPerTransactions { get; set; }
        public DbSet<Outbox_MultipartForm> Outbox_MultipartForm { get; set; }
        public DbSet<ReceivingStage> ReceivingStages { get; set; }
        public DbSet<RURD> RURDs { get; set; }
        public DbSet<RURD_DateTime_Head> RURD_DateTime_Head { get; set; }
        public DbSet<RURD_LINDetail_RefrenceNumber> RURD_LINDetail_RefrenceNumber { get; set; }
        public DbSet<RURD_LINDetailSegment> RURD_LINDetailSegment { get; set; }
        public DbSet<RURD_Name_Head> RURD_Name_Head { get; set; }
        public DbSet<SendingStage> SendingStages { get; set; }

        public DbSet<ShipperCompany_Pipeline_Config> ShipperCompany_Pipeline_Config { get; set; }
        public DbSet<ShipperCompany_Pipeline_Map> ShipperCompany_Pipeline_Map { get; set; }
        public DbSet<ShipperCompany_Pipeline_UPRD_Map> ShipperCompany_Pipeline_UPRD_Map { get; set; }
        public DbSet<SQTSFile> SQTSFiles { get; set; }
        public DbSet<SQTSTrackOnNom> SQTSTrackOnNoms { get; set; }
        public DbSet<TaskMgrXml> TaskMgrXmls { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        //public DbSet<UPRD> UPRDs { get; set; }
        //public DbSet<UPRD_DataRequestCode> UPRD_DataRequestCode { get; set; }
        //public DbSet<UPRD_DateTimeRef> UPRD_DateTimeRef { get; set; }
        //public DbSet<UPRD_NameHead> UPRD_NameHead { get; set; }
        public DbSet<UPRDStatu> UPRDStatus { get; set; }
        public DbSet<OutboxMultipartForm> OutboxMultipartForms { get; set; }
        public DbSet<TestedXMLFile> TestedXMLFiles { get; set; }

        public DbSet<PasswordHistory> passwordHistories { get; set; }
        #endregion

       

        #region Schedular Setting 
        public DbSet<DataSetConfigurationSetting> DataSetConfigurationSettings { get; set; }
        #endregion

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        //NomEntities dbbContext;
        public static NomEntities Create() => new NomEntities();
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Database.SetInitializer<NomEntities>(new MigrateDatabaseToLatestVersion<NomEntities, Configuration>());
            base.OnModelCreating(modelBuilder);
        }
    }
}
