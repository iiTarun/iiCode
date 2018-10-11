namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_PipeDuns_in_TaskMgrXmls : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskMgrXmls", "PipeDuns", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaskMgrXmls", "PipeDuns");
        }
    }
}
