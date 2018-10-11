namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeUNSCfromTradingPart : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.UnscPerTransactions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UnscPerTransactions",
                c => new
                    {
                        UnscID = c.Long(nullable: false, identity: true),
                        TransactionID = c.Guid(nullable: false),
                        ReceiveFileID = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        PipelineID = c.Int(nullable: false),
                        TransactionServiceProvider = c.String(),
                        TransactionServiceProviderPropCode = c.String(),
                        Loc = c.String(),
                        LocName = c.String(),
                        LocZn = c.String(),
                        LocPurpDesc = c.String(),
                        LocQTIDesc = c.String(),
                        MeasBasisDesc = c.String(),
                        TotalDesignCapacity = c.Long(nullable: false),
                        UnsubscribeCapacity = c.Long(nullable: false),
                        PostingDateTime = c.DateTime(),
                        EffectiveGasDayTime = c.DateTime(),
                        EndingEffectiveDay = c.DateTime(),
                        ChangePercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsExistCheck = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UnscID);
            
        }
    }
}
