namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSQTSReductionReason : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SQTSPerTransactions", "ReductionReasonDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SQTSPerTransactions", "ReductionReasonDescription");
        }
    }
}
