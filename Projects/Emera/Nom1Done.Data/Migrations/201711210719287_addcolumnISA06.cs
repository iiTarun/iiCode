namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcolumnISA06 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PipelineEDISettings", "ISA06_Segment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PipelineEDISettings", "ISA06_Segment");
        }
    }
}
