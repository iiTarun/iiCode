namespace Nom1Done.Model
{
    using System;
    
    public partial class SQTSTrackOnNom
    {
        public long Id { get; set; }
        public Guid SqtsFileId { get; set; }
        public Guid NomTransactionID { get; set; }
        public string NomTrackingId { get; set; }
        public string ReceiptPointQuantity { get; set; }
        public string DeliveryPointQuantity { get; set; }
        public string ReductionReason { get; set; }
    }
}
