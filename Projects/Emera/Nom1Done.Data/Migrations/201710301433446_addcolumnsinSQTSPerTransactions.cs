namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcolumnsinSQTSPerTransactions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SQTSPerTransactions", "DeliveryRank", c => c.String());
            AddColumn("dbo.SQTSPerTransactions", "CapacityTypeIndicator", c => c.String());
            AddColumn("dbo.SQTSPerTransactions", "ExportDecleration", c => c.String());
            AddColumn("dbo.SQTSPerTransactions", "NomSubsequentCycleIndicator", c => c.String());
            AddColumn("dbo.SQTSPerTransactions", "ProcessingRightsIndicator", c => c.String());
            AddColumn("dbo.SQTSPerTransactions", "Route", c => c.String());
            AddColumn("dbo.SQTSPerTransactions", "DealType", c => c.String());
            AddColumn("dbo.SQTSPerTransactions", "AssociatedContract", c => c.String());
            AddColumn("dbo.SQTSPerTransactions", "ServiceProviderActivityCode", c => c.String());
            AddColumn("dbo.SQTSPerTransactions", "NominationUserData1", c => c.String());
            AddColumn("dbo.SQTSPerTransactions", "NominationUserData2", c => c.String());
            AddColumn("dbo.SQTSPerTransactions", "DownstreamContractIdentifier", c => c.String());
            AddColumn("dbo.SQTSPerTransactions", "UpstreamContractIdentifier", c => c.String());
            AddColumn("dbo.SQTSPerTransactions", "DownstreamPackageId", c => c.String());
            AddColumn("dbo.SQTSPerTransactions", "UpstreamPackageId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SQTSPerTransactions", "UpstreamPackageId");
            DropColumn("dbo.SQTSPerTransactions", "DownstreamPackageId");
            DropColumn("dbo.SQTSPerTransactions", "UpstreamContractIdentifier");
            DropColumn("dbo.SQTSPerTransactions", "DownstreamContractIdentifier");
            DropColumn("dbo.SQTSPerTransactions", "NominationUserData2");
            DropColumn("dbo.SQTSPerTransactions", "NominationUserData1");
            DropColumn("dbo.SQTSPerTransactions", "ServiceProviderActivityCode");
            DropColumn("dbo.SQTSPerTransactions", "AssociatedContract");
            DropColumn("dbo.SQTSPerTransactions", "DealType");
            DropColumn("dbo.SQTSPerTransactions", "Route");
            DropColumn("dbo.SQTSPerTransactions", "ProcessingRightsIndicator");
            DropColumn("dbo.SQTSPerTransactions", "NomSubsequentCycleIndicator");
            DropColumn("dbo.SQTSPerTransactions", "ExportDecleration");
            DropColumn("dbo.SQTSPerTransactions", "CapacityTypeIndicator");
            DropColumn("dbo.SQTSPerTransactions", "DeliveryRank");
        }
    }
}
