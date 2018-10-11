namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NomSubsequentCycle_Add_in_Batch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.V4_Batch", "NomSubCycle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.V4_Batch", "NomSubCycle");
        }
    }
}
