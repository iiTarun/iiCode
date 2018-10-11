namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeUPRDrelatedUnwantedTablesfromTradingPart : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.UPRD_DataRequestCode");
            DropTable("dbo.UPRD_DateTimeRef");
            DropTable("dbo.UPRD_NameHead");
            DropTable("dbo.UPRDs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UPRDs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionID = c.Guid(nullable: false),
                        TransactionSetControlNumbar = c.String(),
                        RequestID = c.String(),
                        Date = c.DateTime(),
                        IsSend = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UPRD_NameHead",
                c => new
                    {
                        NameHeadID = c.Guid(nullable: false, identity: true),
                        TransactionID = c.Guid(nullable: false),
                        TransportationServiceProvider = c.String(),
                        TransportationServiceProviderPropCode = c.String(),
                        RequesterCompanyCode = c.String(),
                        RequesterCompanyCodePropCode = c.String(),
                    })
                .PrimaryKey(t => t.NameHeadID);
            
            CreateTable(
                "dbo.UPRD_DateTimeRef",
                c => new
                    {
                        DateTimeRefID = c.Guid(nullable: false, identity: true),
                        TransactionID = c.Guid(nullable: false),
                        BeginDate = c.DateTime(),
                        BeginTime = c.Time(precision: 7),
                        EndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DateTimeRefID);
            
            CreateTable(
                "dbo.UPRD_DataRequestCode",
                c => new
                    {
                        RequestCodeID = c.Guid(nullable: false, identity: true),
                        TransactionID = c.Guid(nullable: false),
                        RequestCode = c.String(),
                    })
                .PrimaryKey(t => t.RequestCodeID);
            
        }
    }
}
