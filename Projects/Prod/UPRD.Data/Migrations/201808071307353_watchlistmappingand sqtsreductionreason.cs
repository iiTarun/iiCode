namespace UPRD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class watchlistmappingandsqtsreductionreason : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SQTSReductionReasons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WatchListLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Source = c.String(),
                        Type = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WatchlistMailMappingOACies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        UserEmail = c.String(),
                        WatchListId = c.Int(nullable: false),
                        OACYID = c.Long(nullable: false),
                        EmailSentDateTime = c.DateTime(),
                        IsMailSent = c.Boolean(nullable: false),
                        WatchlistName = c.String(),
                        MoreDetailUrl = c.String(),
                        CycleIndicator = c.String(),
                        PipelineID = c.Int(nullable: false),
                        PipelineName = c.String(),
                        Loc = c.String(),
                        LocName = c.String(),
                        OperatingCapacity = c.Long(nullable: false),
                        TotalScheduleQty = c.Long(nullable: false),
                        AvailablePercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PostingDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WatchlistMailMappingSWNTs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        WatchListId = c.Int(nullable: false),
                        SWNTID = c.Long(nullable: false),
                        EmailSentDateTime = c.DateTime(),
                        IsMailSent = c.Boolean(nullable: false),
                        UserEmail = c.String(),
                        WatchlistName = c.String(),
                        MoreDetailUrl = c.String(),
                        PipelineID = c.Int(nullable: false),
                        PipelineName = c.String(),
                        Loc = c.String(),
                        LocName = c.String(),
                        CriticalNoticeIndicator = c.String(),
                        NoticeId = c.String(),
                        NoticeEffectiveDateTime = c.DateTime(),
                        PostingDateTime = c.DateTime(),
                        Category = c.String(),
                        Subject = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WatchlistMailMappingUNSCs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        WatchListId = c.Int(nullable: false),
                        UNSCID = c.Long(nullable: false),
                        EmailSentDateTime = c.DateTime(),
                        IsMailSent = c.Boolean(nullable: false),
                        UserEmail = c.String(),
                        WatchlistName = c.String(),
                        MoreDetailUrl = c.String(),
                        PipelineID = c.Int(nullable: false),
                        PipelineName = c.String(),
                        Loc = c.String(),
                        LocName = c.String(),
                        UnsubscribeCapacity = c.Long(nullable: false),
                        ChangePercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PostingDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WatchlistMailMappingUNSCs");
            DropTable("dbo.WatchlistMailMappingSWNTs");
            DropTable("dbo.WatchlistMailMappingOACies");
            DropTable("dbo.WatchListLogs");
            DropTable("dbo.SQTSReductionReasons");
        }
    }
}
