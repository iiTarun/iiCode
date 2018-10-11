namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShipperCompSubShipperComp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShipperCompSubShipperComps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShipperCompDuns = c.String(),
                        SubShipperCompDuns = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ShipperCompSubShipperComps");
        }
    }
}
