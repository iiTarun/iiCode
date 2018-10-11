namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedWatchListModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Watchlists", "PipelineId", c => c.Int(nullable: false));
            AlterColumn("dbo.Watchlists", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Watchlists", "ModifiedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Watchlists", "ModifiedDate", c => c.String());
            AlterColumn("dbo.Watchlists", "CreatedDate", c => c.String());
            DropColumn("dbo.Watchlists", "PipelineId");
        }
    }
}
