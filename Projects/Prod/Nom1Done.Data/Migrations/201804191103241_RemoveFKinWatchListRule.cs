namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFKinWatchListRule : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WatchlistRules", "OperatorId", "dbo.LogicalOperators");
            DropForeignKey("dbo.WatchlistRules", "ColumnId", "dbo.MasterColumns");
            DropIndex("dbo.WatchlistRules", new[] { "ColumnId" });
            DropIndex("dbo.WatchlistRules", new[] { "OperatorId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.WatchlistRules", "OperatorId");
            CreateIndex("dbo.WatchlistRules", "ColumnId");
            AddForeignKey("dbo.WatchlistRules", "ColumnId", "dbo.MasterColumns", "Id", cascadeDelete: true);
            AddForeignKey("dbo.WatchlistRules", "OperatorId", "dbo.LogicalOperators", "Id", cascadeDelete: true);
        }
    }
}
