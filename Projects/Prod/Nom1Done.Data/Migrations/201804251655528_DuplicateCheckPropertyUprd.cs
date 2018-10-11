namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DuplicateCheckPropertyUprd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OACYPerTransactions", "IsExistCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.UnscPerTransactions", "IsExistCheck", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UnscPerTransactions", "IsExistCheck");
            DropColumn("dbo.OACYPerTransactions", "IsExistCheck");
        }
    }
}
