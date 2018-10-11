namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NavigationalPropInNomsAndEngineUpgrade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IncomingDatas", "IsProcessed", c => c.Boolean(nullable: false));
            AddColumn("dbo.IncomingDatas", "DecryptedData", c => c.String());
            AddColumn("dbo.IncomingDatas", "PipeDuns", c => c.String());
            AddColumn("dbo.IncomingDatas", "DatasetType", c => c.String());
            CreateIndex("dbo.V4_Nomination", "TransactionID");
            AddForeignKey("dbo.V4_Nomination", "TransactionID", "dbo.V4_Batch", "TransactionID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.V4_Nomination", "TransactionID", "dbo.V4_Batch");
            DropIndex("dbo.V4_Nomination", new[] { "TransactionID" });
            DropColumn("dbo.IncomingDatas", "DatasetType");
            DropColumn("dbo.IncomingDatas", "PipeDuns");
            DropColumn("dbo.IncomingDatas", "DecryptedData");
            DropColumn("dbo.IncomingDatas", "IsProcessed");
        }
    }
}
