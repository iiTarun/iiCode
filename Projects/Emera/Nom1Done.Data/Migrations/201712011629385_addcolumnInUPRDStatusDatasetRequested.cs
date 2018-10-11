namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcolumnInUPRDStatusDatasetRequested : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UPRDStatus", "DatasetRequested", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UPRDStatus", "DatasetRequested");
        }
    }
}
