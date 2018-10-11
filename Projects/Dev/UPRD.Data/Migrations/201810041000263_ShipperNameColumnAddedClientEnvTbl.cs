namespace UPRD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShipperNameColumnAddedClientEnvTbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientEnvironmentSettings", "ShipperName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientEnvironmentSettings", "ShipperName");
        }
    }
}
