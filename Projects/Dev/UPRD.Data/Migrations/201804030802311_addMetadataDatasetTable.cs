namespace UPRD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMetadataDatasetTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.metadataDatasets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Identifier = c.String(),
                        Code = c.String(),
                        Description = c.String(),
                        CategoryID = c.Int(nullable: false),
                        Direction = c.String(),
                        IsUPRDAttribute = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.metadataDatasets");
        }
    }
}
