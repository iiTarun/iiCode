namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocNamesInContract : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contracts", "LocFromName", c => c.String());
            AddColumn("dbo.Contracts", "LocFromIdentifier", c => c.String());
            AddColumn("dbo.Contracts", "LocToName", c => c.String());
            AddColumn("dbo.Contracts", "LocToIdentifier", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contracts", "LocToIdentifier");
            DropColumn("dbo.Contracts", "LocToName");
            DropColumn("dbo.Contracts", "LocFromIdentifier");
            DropColumn("dbo.Contracts", "LocFromName");
        }
    }
}
