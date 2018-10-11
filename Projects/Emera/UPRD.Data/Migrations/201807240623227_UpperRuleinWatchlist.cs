namespace UPRD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpperRuleinWatchlist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WatchlistRules", "UpperRuleValue", c => c.String());
            AddColumn("dbo.Watchlists", "MoreDetailURLinAlert", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Watchlists", "MoreDetailURLinAlert");
            DropColumn("dbo.WatchlistRules", "UpperRuleValue");
        }
    }
}
