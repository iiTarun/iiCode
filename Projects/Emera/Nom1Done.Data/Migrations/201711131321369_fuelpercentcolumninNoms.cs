namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fuelpercentcolumninNoms : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.V4_Nomination", "FuelPercentage", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.V4_Nomination", "MaxDeliveredQty", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.V4_Nomination", "MaxDeliveredQty");
            DropColumn("dbo.V4_Nomination", "FuelPercentage");
        }
    }
}
