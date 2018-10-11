namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionTb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransactionalReports",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        PipelineId = c.Int(nullable: false),
                        PipeLineDuns = c.String(),
                        TSP = c.String(),
                        TSPName = c.String(),
                        PostingDateTime = c.DateTime(nullable: false),
                        ContractHolderName = c.String(),
                        ContractHolderIdentifier = c.String(),
                        ContractHolderProp = c.String(),
                        AffiliateIndicatorDesc = c.String(),
                        RateSchedule = c.String(),
                        ServiceRequesterContract = c.String(),
                        ContactStatus = c.String(),
                        AmendmentReporting = c.String(),
                        ContractBeginDate = c.DateTime(nullable: false),
                        ContractEndDate = c.DateTime(nullable: false),
                        ContractEntitlementBeginDate = c.DateTime(nullable: false),
                        ContractEntitlementEndDate = c.DateTime(nullable: false),
                        SurchargeIndicator = c.Int(nullable: false),
                        RateIdentificationCode = c.String(),
                        RateCharged = c.Double(nullable: false),
                        RateChargedReference = c.String(),
                        MaximumTariffRate = c.Double(nullable: false),
                        MaximumTariffRateReference = c.String(),
                        MarketBasedRateIndicator = c.String(),
                        ReservationRateBasis = c.String(),
                        ContractualQuantityContract = c.Double(nullable: false),
                        NegotiatedRateIndicator = c.String(),
                        TermsNotesIndicator = c.String(),
                        LocQTIPurpDesc1 = c.String(),
                        Location1 = c.Double(nullable: false),
                        Location1Name = c.String(),
                        LocationZone1 = c.String(),
                        LocQTIPurpDesc2 = c.String(),
                        Location2 = c.Double(nullable: false),
                        Location2Name = c.String(),
                        LocationZone2 = c.String(),
                        CapacityTypeIndicator = c.String(),
                        Comments = c.String(),
                        PeakSeasonalStartDate = c.DateTime(nullable: false),
                        PeakSeasonalEndDate = c.DateTime(nullable: false),
                        OffPeakSeasonalStartDate = c.DateTime(nullable: false),
                        OffPeakSeasonalEndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.NMQRPerTransactions", "ReferenceNumber", c => c.String());
            AddColumn("dbo.NMQRPerTransactions", "StatusCode", c => c.String());
            AddColumn("dbo.OACYPerTransactions", "CycleIndicator", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OACYPerTransactions", "CycleIndicator");
            DropColumn("dbo.NMQRPerTransactions", "StatusCode");
            DropColumn("dbo.NMQRPerTransactions", "ReferenceNumber");
            DropTable("dbo.TransactionalReports");
        }
    }
}
