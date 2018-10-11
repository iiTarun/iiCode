namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addWatchlistPipelineMapping : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WatchListPipelineMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WatchListId = c.Int(nullable: false),
                        PipelineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Watchlists", "PipelineId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Watchlists", "PipelineId", c => c.Int(nullable: false));
            DropTable("dbo.WatchListPipelineMappings");
        }
    }
}
