namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_TransactionTypeInLocation_PackageIdInV4Nomination : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "TransactionTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.V4_Nomination", "PackageId2", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.V4_Nomination", "PackageId2");
            DropColumn("dbo.Locations", "TransactionTypeId");
        }
    }
}
