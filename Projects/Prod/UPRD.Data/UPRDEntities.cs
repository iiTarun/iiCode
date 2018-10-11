using System.Data.Entity;
using UPRD.Data.Migrations;
using UPRD.Model;

namespace UPRD.Data
{
    //public class ApplicationUser : IdentityUser
    //{
    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    //    {
    //        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
    //        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
    //        // Add custom user claims here
    //        return userIdentity;
    //    }

    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
    //    {
    //        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
    //        var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
    //        // Add custom user claims here
    //        return userIdentity;
    //    }

    //}
    public class UPRDEntities : DbContext
    {
        public UPRDEntities():base("UPRDEntities")
        {

        }
        public DbSet<Outbox> Outbox { get; set; }
        public DbSet<Pipeline> Pipeline { get; set; }     
        public DbSet<SwntPerTransaction> SwntPerTransaction { get; set; }
        public DbSet<TaskMgrJob> TaskMgrJob { get; set; }
        public DbSet<TransactionLog> TransactionLog { get; set; }
        public DbSet<UnscPerTransaction> UnscPerTransaction { get; set; }
        public DbSet<OACYPerTransaction> OACYPerTransaction { get; set; }
        public DbSet<TransportationServiceProvider> TransportationServiceProvider { get; set; }
        public DbSet<metadataPipelineEncKeyInfo> metadataPipelineEncKeyInfo { get; set; }
        public DbSet<TradingPartnerWorksheet> TradingPartnerWorksheet { get; set; }
        public DbSet<Setting> Setting { get; set; }
        public DbSet<PipelineEDISetting> PipelineEDISetting { get; set; }        
        public DbSet<ApplicationLog> ApplicationLogs { get; set; }
        public DbSet<GISBInbox> GISBInboxes { get; set; }
        public DbSet<GISBOutbox> GISBOutboxes { get; set; }
        public DbSet<Inbox> Inboxes { get; set; }
        public DbSet<IncomingData> IncomingDatas { get; set; }
        public DbSet<JobStackErrorLog> JobStackErrorLogs { get; set; }
        public DbSet<JobWorkflow> JobWorkflows { get; set; }
        public DbSet<Outbox_MultipartForm> Outbox_MultipartForm { get; set; }
        public DbSet<TaskMgrXml> TaskMgrXmls { get; set; }
        public DbSet<UPRDStatus> UPRDStatus { get; set; }
        public DbSet<ShipperCompany> ShipperCompany { get; set; }
        public DbSet<Shipper> Shipper { get; set; }
        public DbSet<EmailQueue> EmailQueue { get; set; }
        public DbSet<EmailTemplate> EmailTemplate { get; set; }
        public DbSet<metadataDataset> metadataDataset { get; set; }
        public DbSet<metadataErrorCode> metadataErrorCode { get; set; }
     
        public DbSet<FileSysIncomingData> FileSysIncomingData { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<CounterParty> CounterParty { get; set; }
        public DbSet<SQTSReductionReason> SQTSReductionReason { get; set; }

        #region WatchList Tables
        public DbSet<DataTypeGrouping> DataTypeGroupings { get; set; }
        public DbSet<LogicalOperator> LogicalOperators { get; set; }
        public DbSet<MasterColumn> MasterColumns { get; set; }
        public DbSet<Watchlist> Watchlists { get; set; }
        public DbSet<WatchlistRule> WatchlistRules { get; set; }
        public DbSet<WatchListPipelineMapping> WatchListPipelineMappings { get; set; }

        public DbSet<WatchListMailAlertUPRDMapping> WatchListMailAlertUPRDMappings { get; set; }

        public DbSet<WatchlistMailMappingOACY> WatchlistMailMappingOACYs { get; set; }
        public DbSet<WatchlistMailMappingSWNT> WatchlistMailMappingSWNTs { get; set; }
        public DbSet<WatchlistMailMappingUNSC> WatchlistMailMappingUNSCs { get; set; }
        public DbSet<WatchListLog> WatchListLogs { get; set; }



        #endregion

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        //NomEntities dbbContext;
        public static UPRDEntities Create()
        {
            return new UPRDEntities();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<UPRDEntities>(new MigrateDatabaseToLatestVersion<UPRDEntities, Configuration>());
            
        }
    }
}
