namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_WatchListMailAlertUPRDMapping : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WatchListMailAlertUPRDMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        WatchListId = c.Int(nullable: false),
                        DataSetId = c.Int(nullable: false),
                        UprdID = c.Long(nullable: false),
                        EmailSentDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WatchListMailAlertUPRDMappings");
        }
    }
}
