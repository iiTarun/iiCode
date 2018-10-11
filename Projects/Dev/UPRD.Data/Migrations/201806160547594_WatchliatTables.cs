namespace UPRD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WatchliatTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataTypeGroupings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DataTypeGroupings", t => t.DataTypeId, cascadeDelete: true)
                .Index(t => t.DataTypeId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DataTypeGroupings", t => t.DataTypeId, cascadeDelete: true)
                .Index(t => t.DataTypeId);
            
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
                "dbo.WatchListPipelineMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WatchListId = c.Int(nullable: false),
                        PipelineId = c.Int(nullable: false),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Watchlists", t => t.WatchlistId, cascadeDelete: true)
                .Index(t => t.WatchlistId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WatchlistRules", "WatchlistId", "dbo.Watchlists");
            DropForeignKey("dbo.MasterColumns", "DataTypeId", "dbo.DataTypeGroupings");
            DropForeignKey("dbo.LogicalOperators", "DataTypeId", "dbo.DataTypeGroupings");
            DropIndex("dbo.WatchlistRules", new[] { "WatchlistId" });
            DropIndex("dbo.MasterColumns", new[] { "DataTypeId" });
            DropIndex("dbo.LogicalOperators", new[] { "DataTypeId" });
            DropTable("dbo.Watchlists");
            DropTable("dbo.WatchlistRules");
            DropTable("dbo.WatchListPipelineMappings");
            DropTable("dbo.WatchListMailAlertUPRDMappings");
            DropTable("dbo.MasterColumns");
            DropTable("dbo.LogicalOperators");
            DropTable("dbo.DataTypeGroupings");
        }
    }
}
