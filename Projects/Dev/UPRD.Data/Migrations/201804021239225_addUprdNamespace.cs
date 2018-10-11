namespace UPRD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUprdNamespace : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UPRDStatus",
                c => new
                    {
                        UPRD_ID = c.Guid(nullable: false, identity: true),
                        RequestID = c.String(),
                        RURD_ID = c.Guid(),
                        CreatedDate = c.DateTime(),
                        TransactionId = c.Guid(),
                        DatasetSummary = c.String(),
                        DatasetRequested = c.Int(),
                        IsRURDReceived = c.Boolean(nullable: false),
                        IsDataSetAvailable = c.Boolean(nullable: false),
                        IsDatasetReceived = c.Boolean(nullable: false),
                        PipeDuns = c.String(),
                    })
                .PrimaryKey(t => t.UPRD_ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UPRDStatus");
        }
    }
}
