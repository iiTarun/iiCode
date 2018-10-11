using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nom1Done.Model
{
    public class V4_Nomination
    {
        public V4_Nomination()
        {
            this.FuelPercentage = 0;
            this.MaxDeliveredQty = 0;
        }
        public long ID { get; set; }

        // FK   
        [ForeignKey("Batch")]
        public Guid TransactionID { get; set; }
        //  public V4_Batch V4_Batch { get; set; }   // Nevigation Property

        public string ContractNumber { get; set; }
        public string NominatorTrackingId { get; set; }
        public string BidTransportationRate { get; set; }
        public string ImbalancePeriod { get; set; }
        public string QuantityTypeIndicator { get; set; }
        public string TransactionType { get; set; }
        public string PackageId { get; set; }
        public string PackageId2 { get; set; }
        public string AssociatedContract { get; set; }
        public string ServiceProviderActivityCode { get; set; }
        public string DealType { get; set; }
        public string NominationUserData1 { get; set; }
        public string NominationUserData2 { get; set; }
        public string DownstreamIdentifier { get; set; }

        public string DownstreamName { get; set; }
        public string DownstreamPropCode { get; set; }
        public string UpstreamIdentifier { get; set; }
        public string UpstreamName { get; set; }
        public string UpstreamPropCode { get; set; }
        public string AssignIdentification { get; set; }
        public string ReceiptLocationIdentifier { get; set; }
        public string ReceiptLocationName { get; set; }
        public string receiptLocationPropCode { get; set; }
        public string DeliveryLocationIdentifer { get; set; }
        public string DeliveryLocationName { get; set; }
        public string DeliveryLocationPropCode { get; set; }
        public string UpstreamContractIdentifier { get; set; }
        public string DownstreamContractIdentifier { get; set; }
        public string UpstreamPackageId { get; set; }
        public string DownstreamPackageId { get; set; }
        public string ReceiptRank { get; set; }
        public string DeliveryRank { get; set; }
        public string UpstreamRank { get; set; }
        public string DownstreamRank { get; set; }
        public int? Quantity { get; set; }
        public int? DelQuantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public string PathType { get; set; }
        public string CapacityTypeIndicator { get; set; }
        public string NominationSubCycleIndicator { get; set; }
        public string ExportDecleration { get; set; }
        public string BidupIndicator { get; set; }
        public string ProcessingRightIndicator { get; set; }
        public string MaxRateIndicator { get; set; }
        public string Route { get; set; }
        public string PathRank { get; set; }
        public string TransactionTypeDesc { get; set; }
        public decimal? FuelPercentage { get; set; }
        public decimal? MaxDeliveredQty { get; set; }

        public virtual V4_Batch Batch { get; set; }


    }
}
