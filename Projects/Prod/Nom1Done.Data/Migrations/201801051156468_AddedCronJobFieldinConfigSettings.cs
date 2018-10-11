namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCronJobFieldinConfigSettings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataSetConfigurationSettings", "SchedularCronJobTime", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DataSetConfigurationSettings", "SchedularCronJobTime");
        }
    }
}
