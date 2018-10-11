namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WatchlistModifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WatchlistRules", "PipelineDuns", c => c.String());
            AddColumn("dbo.WatchlistRules", "LocationIdentifier", c => c.String());
            AddColumn("dbo.WatchlistRules", "AlertSent", c => c.Boolean(nullable: false));
            AddColumn("dbo.WatchlistRules", "AlertFrequency", c => c.Int(nullable: false));
            AddColumn("dbo.WatchlistRules", "IsCriticalNotice", c => c.Boolean(nullable: false));
            DropColumn("dbo.Watchlists", "AlertSent");
            DropColumn("dbo.Watchlists", "AlertFrequency");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Watchlists", "AlertFrequency", c => c.Int(nullable: false));
            AddColumn("dbo.Watchlists", "AlertSent", c => c.Int(nullable: false));
            DropColumn("dbo.WatchlistRules", "IsCriticalNotice");
            DropColumn("dbo.WatchlistRules", "AlertFrequency");
            DropColumn("dbo.WatchlistRules", "AlertSent");
            DropColumn("dbo.WatchlistRules", "LocationIdentifier");
            DropColumn("dbo.WatchlistRules", "PipelineDuns");
        }
    }
}
