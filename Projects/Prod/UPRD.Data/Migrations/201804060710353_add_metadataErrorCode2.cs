namespace UPRD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_metadataErrorCode2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.metadataErrorCodes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        DataElement = c.String(),
                        Description = c.String(),
                        IsRequired = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.metadataErrorCodes");
        }
    }
}
