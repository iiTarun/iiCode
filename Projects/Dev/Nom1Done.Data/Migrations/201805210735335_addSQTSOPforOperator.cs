namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSQTSOPforOperator : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SQTSOPPerTransactions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TranssactionId = c.Guid(nullable: false),
                        StatementDate = c.DateTime(nullable: false),
                        PreparerID = c.String(),
                        Statement_ReceipentID = c.String(),
                        EffectiveStartDate = c.DateTime(nullable: false),
                        EffectiveEndDate = c.DateTime(nullable: false),
                        CycleIndicator = c.String(),
                        LocationCapacityFlowIndicator = c.String(),
                        ConfirmationRole = c.String(),
                        Location = c.String(),
                        LocationNetCapacity = c.Long(nullable: false),
                        ServiceContract = c.String(),
                        ServiceIdentifierCode = c.String(),
                        ConfirmationTrackingID = c.String(),
                        Quantity = c.Long(nullable: false),
                        ContractualFLowIndicator = c.String(),
                        ConfirmationSusequenceCycleIndicator = c.String(),
                        ReductionReason = c.String(),
                        SchedulingStatus = c.String(),
                        ReductionQuantity = c.String(),
                        ServiceRequester = c.String(),
                        DownstreamParty = c.String(),
                        UpstreamParty = c.String(),
                        ServiceRequesterContract = c.String(),
                        DwnStreamShipperContract = c.String(),
                        UpstrmShipperContract = c.String(),
                        DownPkgId = c.String(),
                        UpstrmPkgId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SQTSOPPerTransactions");
        }
    }
}
