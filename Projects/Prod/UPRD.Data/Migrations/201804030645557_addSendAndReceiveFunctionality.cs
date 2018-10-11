namespace UPRD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSendAndReceiveFunctionality : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailQueues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ToUserID = c.String(),
                        Subject = c.String(),
                        Email = c.String(),
                        Recipient = c.String(),
                        CC = c.String(),
                        Bcc = c.String(),
                        IsSent = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        SentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmailTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Subject = c.String(),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Shippers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ShipperCompanyID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ShipperCompanies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DUNS = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        SubscriptionID = c.Int(),
                        ShipperAddress = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ShipperCompanies");
            DropTable("dbo.Shippers");
            DropTable("dbo.EmailTemplates");
            DropTable("dbo.EmailQueues");
        }
    }
}
