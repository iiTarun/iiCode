namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNameForLocCounterpartiesInNomTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.V4_Nomination", "DownstreamName", c => c.String());
            AddColumn("dbo.V4_Nomination", "UpstreamName", c => c.String());
            AddColumn("dbo.V4_Nomination", "ReceiptLocationName", c => c.String());
            AddColumn("dbo.V4_Nomination", "DeliveryLocationName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.V4_Nomination", "DeliveryLocationName");
            DropColumn("dbo.V4_Nomination", "ReceiptLocationName");
            DropColumn("dbo.V4_Nomination", "UpstreamName");
            DropColumn("dbo.V4_Nomination", "DownstreamName");
        }
    }
}
