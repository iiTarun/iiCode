namespace UPRD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pipeline_TransType_Map : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pipeline_TransactionType_Map",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PipelineID = c.Int(nullable: false),
                        TransactionTypeID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastModifiedBy = c.String(),
                        LastModifiedDate = c.DateTime(nullable: false),
                        PathType = c.String(),
                        PipeDuns = c.String(),
                        IsSpecialLocs = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Pipeline_TransactionType_Map");
        }
    }
}
