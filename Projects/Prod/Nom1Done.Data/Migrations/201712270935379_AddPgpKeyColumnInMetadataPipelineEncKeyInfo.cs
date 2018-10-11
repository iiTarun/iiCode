namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPgpKeyColumnInMetadataPipelineEncKeyInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.metadataPipelineEncKeyInfoes", "PgpKey", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.metadataPipelineEncKeyInfoes", "PgpKey");
        }
    }
}
