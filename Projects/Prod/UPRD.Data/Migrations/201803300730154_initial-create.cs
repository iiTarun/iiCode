namespace UPRD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialcreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationLogs",
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
                "dbo.GISBInboxes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageID = c.String(),
                        Gisb = c.String(),
                        ErrorCode = c.Int(nullable: false),
                        ErrorDescription = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GISBOutboxes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageID = c.String(),
                        Gisb = c.String(),
                        ErrorCode = c.Int(nullable: false),
                        ErrorDescription = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inboxes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageID = c.Guid(nullable: false),
                        DatasetID = c.Int(nullable: false),
                        StatusID = c.Int(nullable: false),
                        GISB = c.String(),
                        IsTest = c.Boolean(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        PipelineID = c.Int(nullable: false),
                        RecipientCompanyID = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IncomingDatas",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        MessageId = c.String(),
                        DataReceived = c.String(),
                        ReceivedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JobStackErrorLogs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        WorkflowId = c.Long(nullable: false),
                        TranactionId = c.Guid(nullable: false),
                        StageId = c.Int(nullable: false),
                        ErrorMessage = c.String(),
                        ErrorType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JobWorkflows",
                c => new
                    {
                        WorkflowId = c.Long(nullable: false, identity: true),
                        TransactionId = c.Guid(),
                        StatusId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        StageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WorkflowId);
            
            CreateTable(
                "dbo.metadataPipelineEncKeyInfoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PipelineId = c.Int(),
                        KeyName = c.String(),
                        PipeDuns = c.String(),
                        PgpKey = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OACYPerTransactions",
                c => new
                    {
                        OACYID = c.Long(nullable: false, identity: true),
                        TransactionID = c.Guid(nullable: false),
                        ReceiceFileID = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        TransactionServiceProviderPropCode = c.String(),
                        TransactionServiceProvider = c.String(),
                        PostingDateTime = c.DateTime(),
                        EffectiveGasDayTime = c.DateTime(),
                        Loc = c.String(),
                        LocName = c.String(),
                        LocZn = c.String(),
                        FlowIndicator = c.String(),
                        LocPropDesc = c.String(),
                        LocQTIDesc = c.String(),
                        MeasurementBasis = c.String(),
                        ITIndicator = c.String(),
                        AllQtyAvailableIndicator = c.String(),
                        DesignCapacity = c.Long(nullable: false),
                        OperatingCapacity = c.Long(nullable: false),
                        TotalScheduleQty = c.Long(nullable: false),
                        OperationallyAvailableQty = c.Long(nullable: false),
                        PipelineID = c.Int(),
                        CycleIndicator = c.String(),
                        AvailablePercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OACYID);
            
            CreateTable(
                "dbo.Outboxes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MessageID = c.Guid(nullable: false),
                        DatasetID = c.Int(nullable: false),
                        StatusID = c.Int(nullable: false),
                        GISB = c.String(),
                        IsTest = c.Boolean(nullable: false),
                        PipelineID = c.Int(nullable: false),
                        CompanyID = c.Int(nullable: false),
                        IsScheduled = c.Boolean(nullable: false),
                        ScheduledDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Outbox_MultipartForm",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OutboxID = c.Int(nullable: false),
                        Data = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pipelines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DUNSNo = c.String(),
                        TSPId = c.Int(nullable: false),
                        ModelTypeID = c.Int(nullable: false),
                        ToUseTSPDUNS = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsUprdActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PipelineEDISettings",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PipeDuns = c.String(),
                        ISA08_segment = c.String(),
                        ISA06_Segment = c.String(),
                        ISA11_Segment = c.String(),
                        ISA12_Segment = c.String(),
                        ISA16_Segment = c.String(),
                        GS01_Segment = c.String(),
                        GS02_Segment = c.String(),
                        GS03_Segment = c.String(),
                        GS07_Segment = c.String(),
                        GS08_Segment = c.String(),
                        ST01_Segment = c.String(),
                        DataSeparator = c.String(),
                        SegmentSeperator = c.String(),
                        DatasetId = c.Int(nullable: false),
                        ShipperCompDuns = c.String(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        SendManually = c.Boolean(nullable: false),
                        ForOacy = c.Boolean(nullable: false),
                        ForUnsc = c.Boolean(nullable: false),
                        ForSwnt = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
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
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskMgrJobs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TransactionId = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        StatusId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsSending = c.Boolean(nullable: false),
                        StageId = c.Int(),
                        DatasetId = c.Int(),
                        EDICheckCount = c.Int(),
                        NoOfXmlInEDI = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskMgrXmls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionId = c.String(),
                        XmlData = c.String(),
                        PipelineId = c.Int(nullable: false),
                        AddedOn = c.DateTime(),
                        IsProccessed = c.Boolean(),
                        EDIData = c.String(),
                        EncEDIData = c.String(),
                        SendUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TradingPartnerWorksheets",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PipelineID = c.Int(nullable: false),
                        UsernameLive = c.String(),
                        PasswordLive = c.String(),
                        URLLive = c.String(),
                        KeyLive = c.String(),
                        UsernameTest = c.String(),
                        PasswordTest = c.String(),
                        URLTest = c.String(),
                        KeyTest = c.String(),
                        ReceiveSubSeperator = c.String(),
                        ReceiveDataSeperator = c.String(),
                        ReceiveSegmentSeperator = c.String(),
                        SendSubSeperator = c.String(),
                        SendDataSeperator = c.String(),
                        SendSegmentSeperator = c.String(),
                        IsTest = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        PipeDuns = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TransactionLogs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TransactionId = c.String(),
                        StatusId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransportationServiceProviders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DUNSNo = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
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
                    })
                .PrimaryKey(t => t.UnscID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UnscPerTransactions");
            DropTable("dbo.TransportationServiceProviders");
            DropTable("dbo.TransactionLogs");
            DropTable("dbo.TradingPartnerWorksheets");
            DropTable("dbo.TaskMgrXmls");
            DropTable("dbo.TaskMgrJobs");
            DropTable("dbo.SwntPerTransactions");
            DropTable("dbo.Settings");
            DropTable("dbo.PipelineEDISettings");
            DropTable("dbo.Pipelines");
            DropTable("dbo.Outbox_MultipartForm");
            DropTable("dbo.Outboxes");
            DropTable("dbo.OACYPerTransactions");
            DropTable("dbo.metadataPipelineEncKeyInfoes");
            DropTable("dbo.JobWorkflows");
            DropTable("dbo.JobStackErrorLogs");
            DropTable("dbo.IncomingDatas");
            DropTable("dbo.Inboxes");
            DropTable("dbo.GISBOutboxes");
            DropTable("dbo.GISBInboxes");
            DropTable("dbo.ApplicationLogs");
        }
    }
}
