namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAlertFrequencyInWatchlist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Watchlists", "AlertFrequency", c => c.Int(nullable: false));
            AlterColumn("dbo.Watchlists", "AlertSent", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Watchlists", "AlertSent", c => c.Boolean(nullable: false));
            DropColumn("dbo.Watchlists", "AlertFrequency");
        }
    }
}
