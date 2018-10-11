namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeSWNTfromTradingPart : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.SwntPerTransactions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SwntPerTransactions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TransactionId = c.Guid(nullable: false),
                        ReceiveFileId = c.Guid(nullable: false),
                        PipelineId = c.Int(nullable: false),
                        TransactionIdentifierCode = c.String(),
                        TransactionControlNumber = c.String(),
                        TransactionSetPurposeCode = c.String(),
                        Description = c.String(),
                        NoticeEffectiveDateTime = c.DateTime(),
                        NoticeEndDateTime = c.DateTime(),
                        PostingDateTime = c.DateTime(),
                        ResponseDateTime = c.DateTime(),
                        TransportationserviceProvider = c.String(),
                        TransportationServiceProviderPropCode = c.String(),
                        CriticalNoticeIndicator = c.String(),
                        FreeFormMessageText = c.String(),
                        CreatedDate = c.DateTime(),
                        IsActive = c.Boolean(),
                        Message = c.String(),
                        NoticeId = c.String(),
                        NoticeTypeDesc1 = c.String(),
                        NoticeTypeDesc2 = c.String(),
                        ReqrdResp = c.String(),
                        Subject = c.String(),
                        PriorNotice = c.String(),
                        NoticeStatusDesc = c.String(),
                        PipeDuns = c.String(),
                        PipeDunsAndNoticeIdCombination = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
