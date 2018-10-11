namespace Nom1Done.DTO
{
    public class NMQRPerTransactionDTO
    {
        public long ID { get; set; }
        public System.Guid Transactionid { get; set; }
        public string NominationTrackingId { get; set; }
        public string ValidationCode { get; set; }
        public string ValidationMessage { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ReferenceNumber { get; set; }
        public string StatusCode { get; set; }
    }
}
