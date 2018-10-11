namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSummaryColumnInUprdStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UPRDStatus", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.UPRDStatus", "DatasetSummary", c => c.String());
            DropColumn("dbo.UPRDStatus", "OACY_ID");
            DropColumn("dbo.UPRDStatus", "ModifiedDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UPRDStatus", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.UPRDStatus", "OACY_ID", c => c.Guid());
            DropColumn("dbo.UPRDStatus", "DatasetSummary");
            DropColumn("dbo.UPRDStatus", "CreatedDate");
        }
    }
}
