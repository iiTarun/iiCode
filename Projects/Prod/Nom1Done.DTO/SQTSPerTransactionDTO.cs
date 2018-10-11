using System;

namespace Nom1Done.DTO
{
    public class SQTSPerTransactionDTO
    {
        public long Id { get; set; }
        public Guid TranasactionId { get; set; }
        public DateTime StatementDate { get; set; }
        public string TSPCode { get; set; }
        public string ServiceRequestor { get; set; }
        public DateTime BeginingDateTime { get; set; }
        public DateTime EndingDateTime { get; set; }
        public string CycleIndicator { get; set; }
        public string ServiceRequestorContract { get; set; }
        public string ModelType { get; set; }
        public string NomTrackingId { get; set; }
        public string BidTransportationRate { get; set; }
        public string FuelQuantity { get; set; }
        public string TransactionType { get; set; }
        public string ReductionReason { get; set; }
        public string PackageId { get; set; }
        public string UpstreamId { get; set; }
        public string ReceiptLocation { get; set; }
        public string ReceiptRank { get; set; }
        public int ReceiptQuantity { get; set; }
        public string DownstreamID { get; set; }
        public string DeliveryLocation { get; set; }
        public int DeliveryQuantity { get; set; }
        public string DeliveryRank { get; set; }
        public string CapacityTypeIndicator { get; set; }
        public string ExportDecleration { get; set; }
        public string NomSubsequentCycleIndicator { get; set; }
        public string ProcessingRightsIndicator { get; set; }
        public string Route { get; set; }
        public string DealType { get; set; }
        public string AssociatedContract { get; set; }
        public string ServiceProviderActivityCode { get; set; }
        public string NominationUserData1 { get; set; }
        public string NominationUserData2 { get; set; }
        public string DownstreamContractIdentifier { get; set; }
        public string UpstreamContractIdentifier { get; set; }
        public string DownstreamPackageId { get; set; }
        public string UpstreamPackageId { get; set; }
        public int pipelineId { get; set; }
        public string ReductionReasonDescription { get; set; }
    }
}
