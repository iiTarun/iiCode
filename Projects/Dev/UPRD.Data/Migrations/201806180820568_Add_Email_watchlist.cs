namespace UPRD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Email_watchlist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Watchlists", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Watchlists", "Email");
        }
    }
}
