namespace UPRD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class resolve_migration_issue_Merge_NomDasBoard_UserPipelineMap : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DashNominationStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionId = c.Guid(nullable: false),
                        Environment = c.String(),
                        ShipperCompany = c.String(),
                        UserName = c.String(),
                        PipelineName = c.String(),
                        PipeDuns = c.String(),
                        NomTrackingId = c.String(),
                        SubmittedDate = c.DateTime(nullable: false),
                        NomStatus = c.String(),
                        StatusId = c.Int(nullable: false),
                        AlertTrigger = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserPipelineMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        userId = c.String(),
                        shipperId = c.Int(nullable: false),
                        PipeDuns = c.String(),
                        IsNoms = c.Boolean(nullable: false),
                        IsUPRD = c.Boolean(nullable: false),
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
            DropTable("dbo.DashNominationStatus");
        }
    }
}
