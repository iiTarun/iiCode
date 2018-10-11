namespace UPRD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileSysIncomingDataTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileSysIncomingDatas",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MessageId = c.String(),
                        DataReceived = c.String(),
                        ReceivedAt = c.DateTime(nullable: false),
                        IsProcessed = c.Boolean(nullable: false),
                        DecryptedData = c.String(),
                        PipeDuns = c.String(),
                        DatasetType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FileSysIncomingDatas");
        }
    }
}
