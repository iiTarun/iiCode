namespace Nom1Done.Model
{
    using System;
    public class SwntPerTransaction:EntityBase
    {
        public long Id { get; set; }
        public Guid TransactionId { get; set; }
        public Guid ReceiveFileId { get; set; }
        public int PipelineId { get; set; }
        public string TransactionIdentifierCode { get; set; }
        public string TransactionControlNumber { get; set; }
        public string TransactionSetPurposeCode { get; set; }
        public string Description { get; set; }
        public DateTime? NoticeEffectiveDateTime { get; set; }       
        public DateTime? NoticeEndDateTime { get; set; }        
        public DateTime? PostingDateTime { get; set; }      
        public DateTime? ResponseDateTime { get; set; }       
        public string TransportationserviceProvider { get; set; }
        public string TransportationServiceProviderPropCode { get; set; }
        public string CriticalNoticeIndicator { get; set; }
        public string FreeFormMessageText { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public string Message { get; set; }
        public string NoticeId { get; set; }       
        public string NoticeTypeDesc1 { get; set; }
        public string NoticeTypeDesc2 { get; set; }     

        public string ReqrdResp { get; set; }
        public string Subject { get; set; }
        public string PriorNotice { get; set; }
        public string NoticeStatusDesc { get; set; }
        public string PipeDuns { get; set; }
        public string PipeDunsAndNoticeIdCombination { get; set; }

    }
}
