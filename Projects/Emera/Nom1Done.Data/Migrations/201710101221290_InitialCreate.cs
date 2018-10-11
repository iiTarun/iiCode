namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
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
                "dbo.metadataBidUpIndicators",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Contracts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RequestNo = c.String(),
                        RequestTypeID = c.Int(nullable: false),
                        FuelPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MDQ = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LocationFromID = c.Int(nullable: false),
                        LocationToID = c.Int(nullable: false),
                        ValidUpto = c.DateTime(nullable: false),
                        PipelineID = c.Int(nullable: false),
                        ShipperID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ReceiptZone = c.String(),
                        DeliveryZone = c.String(),
                        PipeDuns = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CounterParties",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Identifier = c.String(),
                        PropCode = c.String(),
                        PipelineID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        PipeDuns = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.EmailQueues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ToUserID = c.String(),
                        Subject = c.String(),
                        Email = c.String(),
                        Recipient = c.String(),
                        CC = c.String(),
                        Bcc = c.String(),
                        IsSent = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        SentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmailTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Subject = c.String(),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ErrorCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        DataElement = c.String(),
                        Description = c.String(),
                        IsRequired = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Guid(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
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
                "dbo.Locations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Identifier = c.String(),
                        PropCode = c.String(),
                        RDUsageID = c.Int(nullable: false),
                        PipelineID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        PipeDuns = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.metadataCapacityTypeIndicators",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.metadataCycles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.metadataDatasets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Identifier = c.String(),
                        Code = c.String(),
                        Description = c.String(),
                        CategoryID = c.Int(nullable: false),
                        Direction = c.String(),
                        IsUPRDAttribute = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.metadataErrorCodes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        DataElement = c.String(),
                        Description = c.String(),
                        IsRequired = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.metadataExportDeclarations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.metadataFileStatus",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.metadataPipelineEncKeyInfoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PipelineId = c.Int(),
                        KeyName = c.String(),
                        PipeDuns = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.metadataQuantityTypeIndicators",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.metadataRequestTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.metadataTransactionTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Identifier = c.String(),
                        Name = c.String(),
                        SequenceNo = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.NMQR_Header",
                c => new
                    {
                        NMQRID = c.Long(nullable: false, identity: true),
                        TransactionID = c.Guid(nullable: false),
                        TransactionSetIdentifierCode = c.String(),
                        TransactionSetControlNumber = c.String(),
                        TransactionSetPurposeCode = c.String(),
                        ReferenceIdentification = c.String(),
                        NominationIssueDate = c.String(),
                        TransactionTypeCode = c.String(),
                        ExceptionOccured = c.String(),
                        Reject_HeaderLevelError = c.String(),
                        Accept = c.String(),
                    })
                .PrimaryKey(t => t.NMQRID);
            
            CreateTable(
                "dbo.NMQR_Information",
                c => new
                    {
                        InfoID = c.Long(nullable: false, identity: true),
                        SegmentID = c.Long(nullable: false),
                        CodeQualifierCode = c.String(),
                        ValidationCode = c.String(),
                        ValidationMessage = c.String(),
                    })
                .PrimaryKey(t => t.InfoID);
            
            CreateTable(
                "dbo.NMQR_NameHead",
                c => new
                    {
                        NameheadID = c.Long(nullable: false, identity: true),
                        NMQRID = c.Long(nullable: false),
                        EntityIdentifierCode = c.String(),
                        IdentificationCodeQualifier = c.String(),
                        ServiceRequester = c.String(),
                        ServiceRequesterProprietaryCode = c.String(),
                        TransportationServiceProvider = c.String(),
                        TranportationServiceProviderProprietaryCode = c.String(),
                    })
                .PrimaryKey(t => t.NameheadID);
            
            CreateTable(
                "dbo.NMQR_ReferenceIdentification_N9",
                c => new
                    {
                        RefIdenN9ID = c.Long(nullable: false, identity: true),
                        NMQRID = c.Long(nullable: false),
                        ReferenceIdentificationQualifier = c.String(),
                        ReferenceIdentification = c.String(),
                        DateTimeQualifier = c.String(),
                        DateTimePeriodFormatQualifier = c.String(),
                        BeginningDate = c.String(),
                        BeginningTime = c.String(),
                        EndingDate = c.String(),
                        EndingTime = c.String(),
                        ServiceRequesterContract = c.String(),
                    })
                .PrimaryKey(t => t.RefIdenN9ID);
            
            CreateTable(
                "dbo.NMQR_RefIdentification",
                c => new
                    {
                        RefIdenID = c.Long(nullable: false, identity: true),
                        SublineItemID = c.Long(nullable: false),
                        ReferenceIdentificationQualifier = c.String(),
                        ServiceProviderActivityCode = c.String(),
                    })
                .PrimaryKey(t => t.RefIdenID);
            
            CreateTable(
                "dbo.NMQR_SublineItemDetail",
                c => new
                    {
                        SublineItemID = c.Long(nullable: false, identity: true),
                        RefIdenN9ID = c.Long(nullable: false),
                        NominatorTrackingID = c.String(),
                        RelationshipCode = c.String(),
                    })
                .PrimaryKey(t => t.SublineItemID);
            
            CreateTable(
                "dbo.NMQRPerTransactions",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Transactionid = c.Guid(nullable: false),
                        NominationTrackingId = c.String(),
                        ValidationCode = c.String(),
                        ValidationMessage = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.NominationStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NomStatusID = c.Long(nullable: false),
                        NOM_ID = c.Guid(nullable: false),
                        NMQR_ID = c.String(),
                        ReferenceNumber = c.String(),
                        StatusID = c.Int(nullable: false),
                        StatusDetail = c.String(),
                        CreatedDate = c.DateTime(),
                        IsAutomated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        PostingDate = c.DateTime(),
                        PostingTime = c.String(),
                        EffectiveGasDay = c.DateTime(),
                        EffectiveTime = c.String(),
                        Loc = c.String(),
                        LocName = c.String(),
                        LocZn = c.String(),
                        FlowIndicator = c.String(),
                        LocPropDesc = c.String(),
                        LocQTIDesc = c.String(),
                        MeasurementBasis = c.String(),
                        ITIndicator = c.String(),
                        AllQtyAvailableIndicator = c.String(),
                        DesignCapacity = c.String(),
                        OperatingCapacity = c.String(),
                        TotalScheduleQty = c.String(),
                        OperationallyAvailableQty = c.String(),
                        PipelineID = c.Int(),
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
                "dbo.OutboxMultipartForms",
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
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Pipeline_TransactionType_Map",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PipelineID = c.Int(nullable: false),
                        TransactionTypeID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastModifiedBy = c.String(),
                        LastModifiedDate = c.DateTime(nullable: false),
                        PathType = c.String(),
                        PipeDuns = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ReceivingStages",
                c => new
                    {
                        StageId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Predecessor = c.Int(),
                    })
                .PrimaryKey(t => t.StageId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.RURD_DateTime_Head",
                c => new
                    {
                        DateTimeHeadID = c.Long(nullable: false, identity: true),
                        TransactionID = c.Guid(nullable: false),
                        DateTimeQuailifier = c.String(),
                        DateTimePeriodFormat = c.String(),
                        DataProcessingDate = c.String(),
                        DataProcessingTime = c.String(),
                    })
                .PrimaryKey(t => t.DateTimeHeadID);
            
            CreateTable(
                "dbo.RURD_LINDetail_RefrenceNumber",
                c => new
                    {
                        ReferenceNumberID = c.Long(nullable: false, identity: true),
                        LineSegmentID = c.Long(nullable: false),
                        ReferenceNumberQualifier = c.String(),
                        DataAvailabilityCode = c.String(),
                    })
                .PrimaryKey(t => t.ReferenceNumberID);
            
            CreateTable(
                "dbo.RURD_LINDetailSegment",
                c => new
                    {
                        LINSegmentID = c.Long(nullable: false, identity: true),
                        TransactionID = c.Guid(nullable: false),
                        AssignedIdentification = c.String(),
                        ProductServiceID_02 = c.String(),
                        DatasetsRequested_03 = c.String(),
                        ProductServiceID_04 = c.String(),
                        DatasetRequested_05 = c.String(),
                        ProductServiceID_06 = c.String(),
                        DatasetRequested_07 = c.String(),
                        ProductServiceID_08 = c.String(),
                        DatasetRequested_09 = c.String(),
                        ProductServiceID_10 = c.String(),
                        DatasetRequested_11 = c.String(),
                        ProductServiceID_12 = c.String(),
                        DatasetRequested_13 = c.String(),
                        ProductServiceID_14 = c.String(),
                        DatasetRequested_15 = c.String(),
                        ProductServiceID_16 = c.String(),
                        DatasetRequested_17 = c.String(),
                    })
                .PrimaryKey(t => t.LINSegmentID);
            
            CreateTable(
                "dbo.RURD_Name_Head",
                c => new
                    {
                        Name_HeadID = c.Long(nullable: false, identity: true),
                        TransactionID = c.Guid(nullable: false),
                        EntityIdentifier = c.String(),
                        IdentifierCodeQualifier = c.String(),
                        TransportationServiceProvider = c.String(),
                        TransportationServiceProviderPropCode = c.String(),
                        RequesterCompCode = c.String(),
                        RequesterCompCodePropCode = c.String(),
                    })
                .PrimaryKey(t => t.Name_HeadID);
            
            CreateTable(
                "dbo.RURDs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionID = c.Guid(nullable: false),
                        TransactionIdentifierCode = c.String(),
                        TransactionControlNumber = c.String(),
                        TransactionPurposeCode = c.String(),
                        ReportTypeCode = c.String(),
                        RequestID = c.String(),
                        Date = c.String(),
                        Time = c.String(),
                        EntityIdentifier = c.String(),
                        IdentifierCodeQualifir = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SendingStages",
                c => new
                    {
                        StageId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Predecessor = c.Int(),
                    })
                .PrimaryKey(t => t.StageId);
            
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
                "dbo.Shippers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ShipperCompanyID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ShipperCompanies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DUNS = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        SubscriptionID = c.Int(),
                        ShipperAddress = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ShipperCompany_Pipeline_Config",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyID = c.Int(nullable: false),
                        PipelineID = c.Int(nullable: false),
                        IsLocationPropCodeRequired = c.Boolean(nullable: false),
                        IsCounterPartyPropCodeRequired = c.Boolean(nullable: false),
                        IsDeliveredQuantityRequired = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastModifiedBy = c.String(),
                        LastModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShipperCompany_Pipeline_Map",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyID = c.Int(nullable: false),
                        PipelineID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastModifiedBy = c.String(),
                        LastModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShipperCompany_Pipeline_UPRD_Map",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DatasetID = c.Int(nullable: false),
                        PipelineID = c.Int(nullable: false),
                        ShipperID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SQTSFiles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TransactionID = c.Guid(nullable: false),
                        SQTSFile1 = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SQTSTrackOnNoms",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SqtsFileId = c.Guid(nullable: false),
                        NomTransactionID = c.Guid(nullable: false),
                        NomTrackingId = c.String(),
                        ReceiptPointQuantity = c.String(),
                        DeliveryPointQuantity = c.String(),
                        ReductionReason = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        NoticeEffectiveDate = c.String(),
                        NoticeEffectiveTime = c.String(),
                        NoticeEndDate = c.String(),
                        NoticeEndTime = c.String(),
                        PostingDate = c.String(),
                        PostingTime = c.String(),
                        ResponseDate = c.String(),
                        ResponseTime = c.String(),
                        TransportationserviceProvider = c.String(),
                        TransportationServiceProviderPropCode = c.String(),
                        CriticalNoticeIndicator = c.String(),
                        FreeFormMessageText = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(),
                        Message = c.String(),
                        NoticeId = c.Long(nullable: false),
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
                "dbo.TaskMgrReceiveMultipuleFiles",
                c => new
                    {
                        ReceiveFileID = c.Guid(nullable: false),
                        TransactionId = c.String(),
                        CreatedAt = c.DateTime(),
                        FileName = c.String(),
                        IsAdd = c.Boolean(),
                        PipelineID = c.Int(),
                        DatasetId = c.Int(),
                        ReceiveData = c.String(),
                    })
                .PrimaryKey(t => t.ReceiveFileID);
            
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
                "dbo.TestedXMLFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PipelineID = c.Int(),
                        DatasetID = c.Int(),
                        TestedXMLFile1 = c.String(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TPWBrowserTestResults",
                c => new
                    {
                        BrowserTestID = c.Guid(nullable: false, identity: true),
                        PipelineID = c.Int(nullable: false),
                        PipelineName = c.String(),
                        IsProduction = c.Boolean(),
                        IsWorking = c.Boolean(),
                        ErrorMessage = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BrowserTestID);
            
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
                        TotalDesignCapacity = c.String(),
                        UnsubscribeCapacity = c.String(),
                        PostingDate = c.DateTime(),
                        PostingTime = c.String(),
                        EffectiveGasDay = c.DateTime(),
                        EndingEffectiveDay = c.DateTime(),
                    })
                .PrimaryKey(t => t.UnscID);
            
            CreateTable(
                "dbo.UploadedFiles",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        FileName = c.String(),
                        FileBytes = c.Binary(),
                        AddedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        PipelineId = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UPRD_DataRequestCode",
                c => new
                    {
                        RequestCodeID = c.Guid(nullable: false, identity: true),
                        TransactionID = c.Guid(nullable: false),
                        RequestCode = c.String(),
                    })
                .PrimaryKey(t => t.RequestCodeID);
            
            CreateTable(
                "dbo.UPRD_DateTimeRef",
                c => new
                    {
                        DateTimeRefID = c.Guid(nullable: false, identity: true),
                        TransactionID = c.Guid(nullable: false),
                        BeginDate = c.DateTime(),
                        BeginTime = c.Time(precision: 7),
                        EndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DateTimeRefID);
            
            CreateTable(
                "dbo.UPRD_NameHead",
                c => new
                    {
                        NameHeadID = c.Guid(nullable: false, identity: true),
                        TransactionID = c.Guid(nullable: false),
                        TransportationServiceProvider = c.String(),
                        TransportationServiceProviderPropCode = c.String(),
                        RequesterCompanyCode = c.String(),
                        RequesterCompanyCodePropCode = c.String(),
                    })
                .PrimaryKey(t => t.NameHeadID);
            
            CreateTable(
                "dbo.UPRDs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionID = c.Guid(nullable: false),
                        TransactionSetControlNumbar = c.String(),
                        RequestID = c.String(),
                        Date = c.DateTime(),
                        IsSend = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UPRDStatus",
                c => new
                    {
                        UPRD_ID = c.Guid(nullable: false, identity: true),
                        RequestID = c.String(),
                        RURD_ID = c.Guid(),
                        OACY_ID = c.Guid(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UPRD_ID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.V4_Batch",
                c => new
                    {
                        TransactionID = c.Guid(nullable: false, identity: true),
                        FlowStartDate = c.DateTime(nullable: false),
                        FlowEndDate = c.DateTime(nullable: false),
                        CycleId = c.Int(nullable: false),
                        StatusID = c.Int(nullable: false),
                        SubmitDate = c.DateTime(),
                        ScheduleDate = c.DateTime(),
                        PipelineID = c.Int(nullable: false),
                        ServiceRequester = c.String(),
                        Description = c.String(),
                        ShowZeroCheck = c.Boolean(nullable: false),
                        RankingCheck = c.Boolean(nullable: false),
                        PakageCheck = c.Boolean(nullable: false),
                        UpDnContractCheck = c.Boolean(nullable: false),
                        ShowZeroUp = c.Boolean(nullable: false),
                        ShowZeroDn = c.Boolean(nullable: false),
                        UpDnPkgCheck = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ReferenceNumber = c.String(),
                        TransactionSetControlNumber = c.String(),
                        NomTypeID = c.Int(),
                    })
                .PrimaryKey(t => t.TransactionID);
            
            CreateTable(
                "dbo.V4_Nomination",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        TransactionID = c.Guid(nullable: false),
                        ContractNumber = c.String(),
                        NominatorTrackingId = c.String(),
                        BidTransportationRate = c.String(),
                        ImbalancePeriod = c.String(),
                        QuantityTypeIndicator = c.String(),
                        TransactionType = c.String(),
                        PackageId = c.String(),
                        AssociatedContract = c.String(),
                        ServiceProviderActivityCode = c.String(),
                        DealType = c.String(),
                        NominationUserData1 = c.String(),
                        NominationUserData2 = c.String(),
                        DownstreamIdentifier = c.String(),
                        DownstreamPropCode = c.String(),
                        UpstreamIdentifier = c.String(),
                        UpstreamPropCode = c.String(),
                        AssignIdentification = c.String(),
                        ReceiptLocationIdentifier = c.String(),
                        receiptLocationPropCode = c.String(),
                        DeliveryLocationIdentifer = c.String(),
                        DeliveryLocationPropCode = c.String(),
                        UpstreamContractIdentifier = c.String(),
                        DownstreamContractIdentifier = c.String(),
                        UpstreamPackageId = c.String(),
                        DownstreamPackageId = c.String(),
                        ReceiptRank = c.String(),
                        DeliveryRank = c.String(),
                        UpstreamRank = c.String(),
                        DownstreamRank = c.String(),
                        Quantity = c.Int(),
                        DelQuantity = c.Int(),
                        UnitOfMeasure = c.String(),
                        PathType = c.String(),
                        CapacityTypeIndicator = c.String(),
                        NominationSubCycleIndicator = c.String(),
                        ExportDecleration = c.String(),
                        BidupIndicator = c.String(),
                        ProcessingRightIndicator = c.String(),
                        MaxRateIndicator = c.String(),
                        Route = c.String(),
                        PathRank = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.V4_Nomination");
            DropTable("dbo.V4_Batch");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UPRDStatus");
            DropTable("dbo.UPRDs");
            DropTable("dbo.UPRD_NameHead");
            DropTable("dbo.UPRD_DateTimeRef");
            DropTable("dbo.UPRD_DataRequestCode");
            DropTable("dbo.UploadedFiles");
            DropTable("dbo.UnscPerTransactions");
            DropTable("dbo.TransportationServiceProviders");
            DropTable("dbo.TransactionLogs");
            DropTable("dbo.TradingPartnerWorksheets");
            DropTable("dbo.TPWBrowserTestResults");
            DropTable("dbo.TestedXMLFiles");
            DropTable("dbo.TaskMgrXmls");
            DropTable("dbo.TaskMgrReceiveMultipuleFiles");
            DropTable("dbo.TaskMgrJobs");
            DropTable("dbo.SwntPerTransactions");
            DropTable("dbo.SQTSTrackOnNoms");
            DropTable("dbo.SQTSFiles");
            DropTable("dbo.ShipperCompany_Pipeline_UPRD_Map");
            DropTable("dbo.ShipperCompany_Pipeline_Map");
            DropTable("dbo.ShipperCompany_Pipeline_Config");
            DropTable("dbo.ShipperCompanies");
            DropTable("dbo.Shippers");
            DropTable("dbo.Settings");
            DropTable("dbo.SendingStages");
            DropTable("dbo.RURDs");
            DropTable("dbo.RURD_Name_Head");
            DropTable("dbo.RURD_LINDetailSegment");
            DropTable("dbo.RURD_LINDetail_RefrenceNumber");
            DropTable("dbo.RURD_DateTime_Head");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ReceivingStages");
            DropTable("dbo.Pipeline_TransactionType_Map");
            DropTable("dbo.Pipelines");
            DropTable("dbo.OutboxMultipartForms");
            DropTable("dbo.Outbox_MultipartForm");
            DropTable("dbo.Outboxes");
            DropTable("dbo.OACYPerTransactions");
            DropTable("dbo.NominationStatus");
            DropTable("dbo.NMQRPerTransactions");
            DropTable("dbo.NMQR_SublineItemDetail");
            DropTable("dbo.NMQR_RefIdentification");
            DropTable("dbo.NMQR_ReferenceIdentification_N9");
            DropTable("dbo.NMQR_NameHead");
            DropTable("dbo.NMQR_Information");
            DropTable("dbo.NMQR_Header");
            DropTable("dbo.metadataTransactionTypes");
            DropTable("dbo.metadataRequestTypes");
            DropTable("dbo.metadataQuantityTypeIndicators");
            DropTable("dbo.metadataPipelineEncKeyInfoes");
            DropTable("dbo.metadataFileStatus");
            DropTable("dbo.metadataExportDeclarations");
            DropTable("dbo.metadataErrorCodes");
            DropTable("dbo.metadataDatasets");
            DropTable("dbo.metadataCycles");
            DropTable("dbo.metadataCapacityTypeIndicators");
            DropTable("dbo.Locations");
            DropTable("dbo.JobWorkflows");
            DropTable("dbo.JobStackErrorLogs");
            DropTable("dbo.IncomingDatas");
            DropTable("dbo.Inboxes");
            DropTable("dbo.GISBOutboxes");
            DropTable("dbo.GISBInboxes");
            DropTable("dbo.ErrorCodes");
            DropTable("dbo.EmailTemplates");
            DropTable("dbo.EmailQueues");
            DropTable("dbo.CounterParties");
            DropTable("dbo.Contracts");
            DropTable("dbo.metadataBidUpIndicators");
            DropTable("dbo.ApplicationLogs");
        }
    }
}
