namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RateScheduledInContract : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contracts", "RateSchedule", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contracts", "RateSchedule");
        }
    }
}
