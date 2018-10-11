namespace UPRD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Location_CounterParty_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CounterParties",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Identifier = c.String(),
                        PropCode = c.String(),
                        PipelineID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        PipeDuns = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Identifier = c.String(),
                        PropCode = c.String(),
                        RDUsageID = c.Int(nullable: false),
                        PipelineID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        PipeDuns = c.String(),
                        TransactionTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Locations");
            DropTable("dbo.CounterParties");
        }
    }
}
