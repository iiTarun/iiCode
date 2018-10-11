namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserPipelineMappingTb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserPipelineMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        userId = c.String(),
                        shipperId = c.Int(nullable: false),
                        pipelineId = c.Int(nullable: false),
                        createdBy = c.String(),
                        createdDate = c.DateTime(),
                        modifiedBy = c.String(),
                        modifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserPipelineMappings");
        }
    }
}
