namespace UPRD.DTO
{
    public class NomEnums
    {

    }
    public enum ShipperComps
    {
        Shell=1,
        Enercross=2,
        Amera=4,
        Emera=5
    }
    public enum statusBatch
    {
        XML = 1,
        EDI = 2,
        Encrypted = 3,
        Decrypted = 4,
        Success_Gisb = 5,
        Success_Ack = 6,
        Success_NMQR = 7,
        Failure_Gisb = 8,
        Failure_Ack = 9,
        Failure_NMQR = 10,
        Draft = 11,
        Replaced = 12
    }       
    public enum ErrorTypes
    {
        Missing_from_Common_Code_Identifier_code = 1,
        Missing_to_Common_CodeIdentifier = 2,
        Missing_input_format = 3,
        Missing_data_file = 4,
        Missing_transaction_set = 5,
        Invalid_from_Common_CodeIdentifier = 6,
        Invalid_to_Common_CodeIdentifier = 7,
        Invalid_input_format = 8,
        Invalid_transaction_set = 9,
        No_parameters_supplied = 10,
        Invalid_version = 11,
        Missing_version = 12,
        Invalid_receipt_security_selection = 13,
        Missing_receipt_disposition_to = 14,
        Invalid_receipt_disposition_to = 15,
        Missing_receipt_report_type = 16,
        Invalid_receipt_report_type = 17,
        Missing_receipt_security_selection = 18,
        Mutually_agreed_element_refnum_not_present = 19,
        Mutually_agreed_element_refnum_orig_not_present = 20,
        Duplicate_refnum_received = 21,
        Public_key_invalid = 22,
        File_not_encrypted = 23,
        Encrypted_file_truncated = 24,
        Encrypted_file_not_signed_orsignature_not_matched = 25,
        Decryption_Error = 26,
        Sending_party_not_associated_with_Receiving_party = 27,
        Package_file_format_not_recognized_by_Receiving_party = 28,
        Data_set_exchange_not_established_for_Trading_Partner = 29,
        System_error = 30,
        Transaction_set_sent_not_mutually_agreed = 31,
        Missing_receipt_security_selection_WEDM103 = 32,
        Element_refnum_received_not_mutually_agreed_ignored = 33,
        Refnum_orig_received_but_not_mutually_agreed_ignored = 34,
    }
    public enum NomStatus
    {
        Draft=11,
        Rejected = 10,
        Accepted=7,
        Submitted=5,
        InProcess=2,
        Error=8,
        GISBError=0,
        Replaced=12
    }
    public enum NomType
    {
        Pathed=1,
        PNT=2,
        NonPathed=3,
        HyPathedNonPathed = 4,
        HyPathedPNT = 5,
        HyNonPathedPNT = 6
    }
    public enum NomPath
    {
        Pathed='P',
        PNTTransport='T',
        PNTUnthreaded='U',
        NonPathed='N'
    }
    public enum DataSet
    {
        Operational_Capacity = 1,
        Storage_Information = 2,
        Unsubscribed_Capacity = 3,
        Award_Download = 4,
        Bid = 5,
        Note_Special_Instruction_Used_only_if_other_dataset_characters_exceed_4000_ = 6,
        Offer = 7,
        Pre_approved_Bidders_List = 8,
        Response_to_Upload_of_Request_for_Download = 9,
        System_Wide_Notices = 10,
        Upload_of_Requests_for_Download_of_Posted_Datasets = 11,
        Withdrawal_Download = 12,
        Allocation = 13,
        Measured_Volume_Audit_Statement = 14,
        Measurement_Events_Alarms = 15,
        Measurement_Information = 16,
        Pre_determined_Allocation = 17,
        Pre_determined_Allocation_Quick_Response = 18,
        Producer_Imbalance_Statement = 19,
        Request_for_Information = 20,
        Response_to_Request_for_Information = 21,
        Shipper_Imbalance = 22,
        Payment_Remittance = 23,
        Service_Requester_Charge_Allowance_Invoice = 24,
        Statement_of_Account = 25,
        Transportation_Sales_Invoice = 26,
        Confirmation_Response = 27,
        Confirmation_Response_Quick_Response = 28,
        Nomination = 29,
        Nomination_Quick_Response = 30,
        Request_for_Confirmation = 31,
        Scheduled_Quantity = 32,
        Scheduled_Quantity_for_Operator = 33,
        Ack_997 = 34,
        Nomination_PNT = 35
    }
    public enum JobStatus
    {
        NotProcessed = 1,
        InProgress = 2,
        Completed = 3,
        Error = 4
    }
    public class Constants
    {
        public const string emailSupport = "noreply@invertedi.com";
        public const string emailEDITeam = "editeam@invertedi.com";
    }
    public enum TemplateType
    {
        UserRegisteration = 1,
        PasswordReset = 2,
        PasswordChange = 3,
        AccountUnlock = 4,
        FileSentReceipt = 5,
        FileReceivedReceipt = 6,
        BusinessRequest = 7,
        AutoResponse = 8,
        SubscribePipeline = 9,
        BrowserTestResult = 10,
        NominationStatusResult = 11,
        ErrorNotification = 12
    }
    public enum Settings
    {
        Passphrase = 1,
        Gpgpath = 2,
        From_Key = 3,
        To_Key = 4,
        EDI_Transaction_Number = 5,
        EDI_Reference_No_Prefix = 6,
        EDI_Package_ID_Prefix = 7,
        EDI_Nominatorstracking_ID_Prefix = 8,
        Socketip = 9,
        KochPassphrase = 10,
        KochPrivateKey = 11,
        KochPublicKey = 12,
        Pathreceivededi = 13,
        Pathreceivedencrypted = 14,
        Pathreceivedxml = 15,
        Pathsentarchived = 16,
        Pathsentedi = 17,
        Pathsentencrypted = 18,
        Pathsentxml = 19,
        Domain_Store = 20,
        Domain_Transaction = 21,
        pathBiztakSentInXML = 22,
        Pathbiztaksentoutedi = 23,
        Pathbiztakreceiveinedi = 24,
        Pathbiztakreceiveoutxml = 25,
        Pathbiztakreceiveinedi_Check = 26,
        Pathautomatedack = 27,
        Pathbiztalk_In_Ack_EDI = 28,
        Pathbiztalk_In_Ack_XML = 29,
        PathPublicKeys = 30,
        InitialXMLFilesPath = 31,
        UprdReqTimeForOacy=32,
        UprdReqTimeForUnsc=33,
        UprdReqTimeForSwnt=34,
        iiTestOn = 35,
        iiPublicKey = 36,
        TimeAndFreqForManualCheck = 37,
        TimeAndFreqForReceiveFileProcess = 38,
        UprdEDIForTest = 39,
        NomEDIForTest = 40,
        sendMailTo = 41,
        emeraPublicKey = 42,
        releaseToShipper = 43,
        EmeraPrivateKey = 44,
        EnecrossPrivateKey = 45,
        EmeraPassphrase = 46,
        CrashFilesPath = 47,
        AckFiles = 48,
        UnknownFiles = 49,
        EngineFilesPath = 50,
        WebCrashFilesPath = 51,
        WebRecSchedFilePath = 52,
        Emera2048_PubKey = 53,
        Emera2048_PvtKey = 54,
        Emera2048_Pass = 55
    }
    public enum Dataset
    {
        NMQR=1,
        SQTS=2,
        SQOP=3
    }
    public enum Statuses
    {
        NotProcessed = 1,
        InProgress = 2,
        Completed = 3,
        Error = 4
    }
    public enum SendingStages
    {
        Initial = 1,
        XML = 2,
        EDI = 3,
        EncryptedEDI = 4,
        SendingFile = 5,
        GISB = 6
    }
    public enum ReceivingStages
    {
        Initial = 1,
        EncEDI_ToDB = 2,
        SendGisb = 3,
        EncEDIFile = 4,
        Decryption = 5,
        XML = 6,
        DBMapping = 7
    }
}
