namespace UPRD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResolvedMigrationConflicts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DashNominationStatus", "ShipperDUNS", c => c.String());
            AlterColumn("dbo.ClientEnvironmentSettings", "ShipperDuns", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ClientEnvironmentSettings", "ShipperDuns", c => c.Int(nullable: false));
            DropColumn("dbo.DashNominationStatus", "ShipperDUNS");
        }
    }
}
