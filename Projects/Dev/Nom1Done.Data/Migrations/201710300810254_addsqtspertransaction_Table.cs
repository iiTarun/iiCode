namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsqtspertransaction_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SQTSPerTransactions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TranasactionId = c.Guid(nullable: false),
                        StatementDate = c.DateTime(nullable: false),
                        TSPCode = c.String(),
                        ServiceRequestor = c.String(),
                        BeginingDateTime = c.DateTime(nullable: false),
                        EndingDateTime = c.DateTime(nullable: false),
                        CycleIndicator = c.String(),
                        ServiceRequestorContract = c.String(),
                        ModelType = c.String(),
                        NomTrackingId = c.String(),
                        BidTransportationRate = c.String(),
                        FuelQuantity = c.String(),
                        TransactionType = c.String(),
                        ReductionReason = c.String(),
                        PackageId = c.String(),
                        UpstreamId = c.String(),
                        ReceiptLocation = c.String(),
                        ReceiptRank = c.String(),
                        ReceiptQuantity = c.Int(nullable: false),
                        DownstreamID = c.String(),
                        DeliveryLocation = c.String(),
                        DeliveryQuantity = c.Int(nullable: false),
                        pipelineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SQTSPerTransactions");
        }
    }
}
