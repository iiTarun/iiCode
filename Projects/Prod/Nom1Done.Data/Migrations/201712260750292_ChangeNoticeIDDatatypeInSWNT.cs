namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeNoticeIDDatatypeInSWNT : DbMigration
    {
        public override void Up()
        {
           // Sql("ALTER TABLE dbo.SwntPerTransactions DROP CONSTRAINT DF__SwntPerTr__Notic__3E1D39E1");
            AlterColumn("dbo.SwntPerTransactions", "NoticeId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SwntPerTransactions", "NoticeId", c => c.Long(nullable: false));
        }
    }
}
