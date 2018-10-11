namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CofigDataSetSetting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataSetConfigurationSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataSet = c.Int(nullable: false),
                        Frequency = c.Int(),
                        SchedularStartTime = c.Time(nullable: false, precision: 7),
                        IsTestMode = c.Boolean(nullable: false),
                        EmailId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Pipelines", "IsUprdActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pipelines", "IsUprdActive");
            DropTable("dbo.DataSetConfigurationSettings");
        }
    }
}
