using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Model
{
    public class OACYPerTransaction : EntityBase
    {
        [Key]
        public long OACYID { get; set; }
        public System.Guid TransactionID { get; set; }
        public System.Guid ReceiceFileID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string TransactionServiceProviderPropCode { get; set; }
        public string TransactionServiceProvider { get; set; }
        public DateTime? PostingDateTime { get; set; }
        public DateTime? EffectiveGasDayTime { get; set; }
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
        public string CycleIndicator { get; set; }
        public decimal AvailablePercentage { get; set; }
        public bool IsExistCheck { get; set; }
    }
}
