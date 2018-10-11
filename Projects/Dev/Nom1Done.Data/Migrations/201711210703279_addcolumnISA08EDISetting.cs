namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcolumnISA08EDISetting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PipelineEDISettings", "ISA08_segment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PipelineEDISettings", "ISA08_segment");
        }
    }
}
