namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransDescColumnToNom : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.V4_Nomination", "TransactionTypeDesc", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.V4_Nomination", "TransactionTypeDesc");
        }
    }
}
