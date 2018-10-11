namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveWatchListModelsOACYModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LogicalOperators", "DataTypeId", "dbo.DataTypeGroupings");
            DropForeignKey("dbo.MasterColumns", "DataTypeId", "dbo.DataTypeGroupings");
            DropForeignKey("dbo.WatchlistRules", "WatchlistId", "dbo.Watchlists");
            DropIndex("dbo.LogicalOperators", new[] { "DataTypeId" });
            DropIndex("dbo.MasterColumns", new[] { "DataTypeId" });
            DropIndex("dbo.WatchlistRules", new[] { "WatchlistId" });
            DropTable("dbo.DataTypeGroupings");
            DropTable("dbo.LogicalOperators");
            DropTable("dbo.MasterColumns");
            DropTable("dbo.OACYPerTransactions");
            DropTable("dbo.WatchListMailAlertUPRDMappings");
            DropTable("dbo.WatchListPipelineMappings");
            DropTable("dbo.WatchlistRules");
            DropTable("dbo.Watchlists");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Watchlists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DataSetId = c.Int(nullable: false),
                        UserId = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        ExecutionDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WatchlistRules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ColumnId = c.Int(nullable: false),
                        OperatorId = c.Int(nullable: false),
                        RuleValue = c.String(),
                        WatchlistId = c.Int(nullable: false),
                        PipelineDuns = c.String(),
                        LocationIdentifier = c.String(),
                        AlertSent = c.Boolean(nullable: false),
                        AlertFrequency = c.Int(nullable: false),
                        IsCriticalNotice = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WatchListPipelineMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WatchListId = c.Int(nullable: false),
                        PipelineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WatchListMailAlertUPRDMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        WatchListId = c.Int(nullable: false),
                        DataSetId = c.Int(nullable: false),
                        UprdID = c.Long(nullable: false),
                        EmailSentDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OACYPerTransactions",
                c => new
                    {
                        OACYID = c.Long(nullable: false, identity: true),
                        TransactionID = c.Guid(nullable: false),
                        ReceiceFileID = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        TransactionServiceProviderPropCode = c.String(),
                        TransactionServiceProvider = c.String(),
                        PostingDateTime = c.DateTime(),
                        EffectiveGasDayTime = c.DateTime(),
                        Loc = c.String(),
                        LocName = c.String(),
                        LocZn = c.String(),
                        FlowIndicator = c.String(),
                        LocPropDesc = c.String(),
                        LocQTIDesc = c.String(),
                        MeasurementBasis = c.String(),
                        ITIndicator = c.String(),
                        AllQtyAvailableIndicator = c.String(),
                        DesignCapacity = c.Long(nullable: false),
                        OperatingCapacity = c.Long(nullable: false),
                        TotalScheduleQty = c.Long(nullable: false),
                        OperationallyAvailableQty = c.Long(nullable: false),
                        PipelineID = c.Int(),
                        CycleIndicator = c.String(),
                        AvailablePercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsExistCheck = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OACYID);
            
            CreateTable(
                "dbo.MasterColumns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Name = c.String(),
                        DataSetId = c.Int(nullable: false),
                        DataTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LogicalOperators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Name = c.String(),
                        DataTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DataTypeGroupings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.WatchlistRules", "WatchlistId");
            CreateIndex("dbo.MasterColumns", "DataTypeId");
            CreateIndex("dbo.LogicalOperators", "DataTypeId");
            AddForeignKey("dbo.WatchlistRules", "WatchlistId", "dbo.Watchlists", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MasterColumns", "DataTypeId", "dbo.DataTypeGroupings", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LogicalOperators", "DataTypeId", "dbo.DataTypeGroupings", "Id", cascadeDelete: true);
        }
    }
}
