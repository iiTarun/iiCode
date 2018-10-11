namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WatchlistTbls : DbMigration
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
                "dbo.WatchlistRules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ColumnId = c.Int(nullable: false),
                        OperatorId = c.Int(nullable: false),
                        RuleValue = c.String(),
                        WatchlistId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LogicalOperators", t => t.OperatorId, cascadeDelete: true)
                .ForeignKey("dbo.MasterColumns", t => t.ColumnId, cascadeDelete: false)
                .ForeignKey("dbo.Watchlists", t => t.WatchlistId, cascadeDelete: true)
                .Index(t => t.ColumnId)
                .Index(t => t.OperatorId)
                .Index(t => t.WatchlistId);
            
            CreateTable(
                "dbo.Watchlists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DataSetId = c.Int(nullable: false),
                        AlertSent = c.Boolean(nullable: false),
                        UserId = c.String(),
                        CreatedDate = c.String(),
                        ModifiedDate = c.String(),
                        ExecutionDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WatchlistRules", "WatchlistId", "dbo.Watchlists");
            DropForeignKey("dbo.WatchlistRules", "ColumnId", "dbo.MasterColumns");
            DropForeignKey("dbo.WatchlistRules", "OperatorId", "dbo.LogicalOperators");
            DropForeignKey("dbo.MasterColumns", "DataTypeId", "dbo.DataTypeGroupings");
            DropForeignKey("dbo.LogicalOperators", "DataTypeId", "dbo.DataTypeGroupings");
            DropIndex("dbo.WatchlistRules", new[] { "WatchlistId" });
            DropIndex("dbo.WatchlistRules", new[] { "OperatorId" });
            DropIndex("dbo.WatchlistRules", new[] { "ColumnId" });
            DropIndex("dbo.MasterColumns", new[] { "DataTypeId" });
            DropIndex("dbo.LogicalOperators", new[] { "DataTypeId" });
            DropTable("dbo.Watchlists");
            DropTable("dbo.WatchlistRules");
            DropTable("dbo.MasterColumns");
            DropTable("dbo.LogicalOperators");
            DropTable("dbo.DataTypeGroupings");
        }
    }
}
