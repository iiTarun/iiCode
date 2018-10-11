namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiySWNTModelAddDateTimeType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SwntPerTransactions", "NoticeEffectiveDateTime", c => c.DateTime());
            AddColumn("dbo.SwntPerTransactions", "NoticeEndDateTime", c => c.DateTime());
            AddColumn("dbo.SwntPerTransactions", "PostingDateTime", c => c.DateTime());
            AddColumn("dbo.SwntPerTransactions", "ResponseDateTime", c => c.DateTime());
            AlterColumn("dbo.SwntPerTransactions", "CreatedDate", c => c.DateTime());
            DropColumn("dbo.SwntPerTransactions", "NoticeEffectiveDate");
            DropColumn("dbo.SwntPerTransactions", "NoticeEffectiveTime");
            DropColumn("dbo.SwntPerTransactions", "NoticeEndDate");
            DropColumn("dbo.SwntPerTransactions", "NoticeEndTime");
            DropColumn("dbo.SwntPerTransactions", "PostingDate");
            DropColumn("dbo.SwntPerTransactions", "PostingTime");
            DropColumn("dbo.SwntPerTransactions", "ResponseDate");
            DropColumn("dbo.SwntPerTransactions", "ResponseTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SwntPerTransactions", "ResponseTime", c => c.String());
            AddColumn("dbo.SwntPerTransactions", "ResponseDate", c => c.String());
            AddColumn("dbo.SwntPerTransactions", "PostingTime", c => c.String());
            AddColumn("dbo.SwntPerTransactions", "PostingDate", c => c.String());
            AddColumn("dbo.SwntPerTransactions", "NoticeEndTime", c => c.String());
            AddColumn("dbo.SwntPerTransactions", "NoticeEndDate", c => c.String());
            AddColumn("dbo.SwntPerTransactions", "NoticeEffectiveTime", c => c.String());
            AddColumn("dbo.SwntPerTransactions", "NoticeEffectiveDate", c => c.String());
            AlterColumn("dbo.SwntPerTransactions", "CreatedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.SwntPerTransactions", "ResponseDateTime");
            DropColumn("dbo.SwntPerTransactions", "PostingDateTime");
            DropColumn("dbo.SwntPerTransactions", "NoticeEndDateTime");
            DropColumn("dbo.SwntPerTransactions", "NoticeEffectiveDateTime");
        }
    }
}
