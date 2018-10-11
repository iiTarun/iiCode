namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyTransactionReportModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransactionalReports", "NegotId", c => c.Long(nullable: false));
            AlterColumn("dbo.TransactionalReports", "ContractualQuantityContract", c => c.Long(nullable: false));
            AlterColumn("dbo.TransactionalReports", "Location1", c => c.Long(nullable: false));
            AlterColumn("dbo.TransactionalReports", "Location2", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TransactionalReports", "Location2", c => c.Double(nullable: false));
            AlterColumn("dbo.TransactionalReports", "Location1", c => c.Double(nullable: false));
            AlterColumn("dbo.TransactionalReports", "ContractualQuantityContract", c => c.Double(nullable: false));
            DropColumn("dbo.TransactionalReports", "NegotId");
        }
    }
}
