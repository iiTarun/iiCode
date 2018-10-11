namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TitleInLogOpeTb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogicalOperators", "Title", c => c.String());
            AddColumn("dbo.UPRDStatus", "IsRURDReceived", c => c.Boolean(nullable: false));
            AddColumn("dbo.UPRDStatus", "IsDataSetAvailable", c => c.Boolean(nullable: false));
            AddColumn("dbo.UPRDStatus", "IsDatasetReceived", c => c.Boolean(nullable: false));
            AddColumn("dbo.UPRDStatus", "PipeDuns", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UPRDStatus", "PipeDuns");
            DropColumn("dbo.UPRDStatus", "IsDatasetReceived");
            DropColumn("dbo.UPRDStatus", "IsDataSetAvailable");
            DropColumn("dbo.UPRDStatus", "IsRURDReceived");
            DropColumn("dbo.LogicalOperators", "Title");
        }
    }
}
