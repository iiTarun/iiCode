namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsSpecialCaseForLoc_Add_In_PipeTransTypeMapeTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pipeline_TransactionType_Map", "IsSpecialLocs", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pipeline_TransactionType_Map", "IsSpecialLocs");
        }
    }
}
