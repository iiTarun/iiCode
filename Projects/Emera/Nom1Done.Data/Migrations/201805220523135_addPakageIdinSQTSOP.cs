namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPakageIdinSQTSOP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SQTSOPPerTransactions", "TransactionId", c => c.Guid(nullable: false));
            AddColumn("dbo.SQTSOPPerTransactions", "PackageId", c => c.String());
            AddColumn("dbo.SQTSOPPerTransactions", "ConfirmationUserData1", c => c.String());
            AddColumn("dbo.SQTSOPPerTransactions", "ConfirmationUserData2", c => c.String());
            AlterColumn("dbo.SQTSOPPerTransactions", "ReductionQuantity", c => c.Long(nullable: false));
            DropColumn("dbo.SQTSOPPerTransactions", "TranssactionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SQTSOPPerTransactions", "TranssactionId", c => c.Guid(nullable: false));
            AlterColumn("dbo.SQTSOPPerTransactions", "ReductionQuantity", c => c.String());
            DropColumn("dbo.SQTSOPPerTransactions", "ConfirmationUserData2");
            DropColumn("dbo.SQTSOPPerTransactions", "ConfirmationUserData1");
            DropColumn("dbo.SQTSOPPerTransactions", "PackageId");
            DropColumn("dbo.SQTSOPPerTransactions", "TransactionId");
        }
    }
}
