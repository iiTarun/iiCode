namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPipeDunsInSwntPerTransactions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SwntPerTransactions", "PipeDuns", c => c.String());
            AddColumn("dbo.SwntPerTransactions", "PipeDunsAndNoticeIdCombination", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SwntPerTransactions", "PipeDunsAndNoticeIdCombination");
            DropColumn("dbo.SwntPerTransactions", "PipeDuns");
        }
    }
}
