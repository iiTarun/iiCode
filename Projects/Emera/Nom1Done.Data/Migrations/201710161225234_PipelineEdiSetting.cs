namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PipelineEdiSetting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PipelineEDISettings",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PipeDuns = c.String(),
                        ISA11_Segment = c.String(),
                        ISA12_Segment = c.String(),
                        ISA16_Segment = c.String(),
                        GS01_Segment = c.String(),
                        GS03_Segment = c.String(),
                        GS07_Segment = c.String(),
                        GS08_Segment = c.String(),
                        ST01_Segment = c.String(),
                        DataSeparator = c.String(),
                        SegmentSeperator = c.String(),
                        DatasetId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PipelineEDISettings");
        }
    }
}
