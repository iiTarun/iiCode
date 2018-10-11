using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatchlistMailManagement.UPRD.DTO
{
    public class OACYPerTransactionDTO
    {
        public long OACYID { get; set; }

        public System.Guid TransactionID { get; set; }
        public System.Guid ReceiceFileID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string TransactionServiceProviderPropCode { get; set; }
        public string TransactionServiceProvider { get; set; }
        public string DUNSNo { get; set; } //TransactionServiceProvider
        public DateTime? PostingDate { get; set; }
        public string PostingTime { get; set; }
        public DateTime? EffectiveGasDay { get; set; }
        public string EffectiveTime { get; set; }
        public string Loc { get; set; }
        public string LocName { get; set; }
        public string LocZn { get; set; }
        public string FlowIndicator { get; set; }
        public string LocPropDesc { get; set; }
        public string LocQTIDesc { get; set; }
        public string MeasurementBasis { get; set; }
        public string ITIndicator { get; set; }
        public string AllQtyAvailableIndicator { get; set; }
        public long DesignCapacity { get; set; }
        public long OperatingCapacity { get; set; }
        public long TotalScheduleQty { get; set; }
        public long OperationallyAvailableQty { get; set; }
        public Nullable<int> PipelineID { get; set; }
        public string PipelineNameDuns { get; set; }
        public string keyword { get; set; }
        public int? page { get; set; }
        public int? size { get; set; }
        public string sort { get; set; }
        public DateTime postStartDate { get; set; }
        public DateTime postEndDate { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime EffectiveEndDate { get; set; }
        public string userId { get; set; }
        public string CycleIndicator { get; set; }
        public decimal AvailablePercentage { get; set; }
        public bool IsThresholdHit { get; set; }
        public DateTime? PostingDateTime { get; set; }
        public DateTime? EffectiveGasDayTime { get; set; }
    }
}