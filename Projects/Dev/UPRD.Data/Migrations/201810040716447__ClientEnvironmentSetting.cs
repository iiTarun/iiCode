namespace UPRD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _ClientEnvironmentSetting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientEnvironmentSettings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ShipperDuns = c.Int(nullable: false),
                        Environment = c.String(),
                        FolderPath = c.String(),
                        ConnectionString = c.String(),
                        Enginestatus = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ClientEnvironmentSettings");
        }
    }
}
