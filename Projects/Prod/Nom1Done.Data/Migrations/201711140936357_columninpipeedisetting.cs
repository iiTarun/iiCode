namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class columninpipeedisetting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PipelineEDISettings", "GS02_Segment", c => c.String());
            AddColumn("dbo.PipelineEDISettings", "ShipperCompDuns", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PipelineEDISettings", "ShipperCompDuns");
            DropColumn("dbo.PipelineEDISettings", "GS02_Segment");
        }
    }
}
