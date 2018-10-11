namespace UPRD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShipperDunsColInDashNomStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DashNominationStatus", "ShipperDUNS", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DashNominationStatus", "ShipperDUNS");
        }
    }
}
