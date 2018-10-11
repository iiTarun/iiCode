namespace UPRD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyShipperDunsColInTbl : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClientEnvironmentSettings", "ShipperDuns", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ClientEnvironmentSettings", "ShipperDuns", c => c.Int(nullable: false));
        }
    }
}
