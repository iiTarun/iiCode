namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPipelineDunsInBatchNOutboxNUploadFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Outboxes", "PipelineDuns", c => c.String());
            AddColumn("dbo.UploadedFiles", "PipelineDuns", c => c.String());
            AddColumn("dbo.V4_Batch", "PipelineDuns", c => c.String());
            DropColumn("dbo.UploadedFiles", "PipelineId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UploadedFiles", "PipelineId", c => c.Int());
            DropColumn("dbo.V4_Batch", "PipelineDuns");
            DropColumn("dbo.UploadedFiles", "PipelineDuns");
            DropColumn("dbo.Outboxes", "PipelineDuns");
        }
    }
}
