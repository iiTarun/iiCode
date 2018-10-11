namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePercentageFromYesterdayInUNSC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UnscPerTransactions", "ChangePercentage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.UnscPerTransactions", "AvailablePercentage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UnscPerTransactions", "AvailablePercentage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.UnscPerTransactions", "ChangePercentage");
        }
    }
}
