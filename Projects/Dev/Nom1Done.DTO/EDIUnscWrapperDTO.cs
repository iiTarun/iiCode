using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.DTO
{
    public class EDIUnscWrapperDTO
    {
        public long UnscID { get; set; }
        public Guid TransactionID { get; set; }
        public Guid ReceiveFileID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PipelineID { get; set; }
        public string TransactionServiceProvider { get; set; }
        public string TransactionServiceProviderPropCode { get; set; }
        public string Loc { get; set; }
        public string LocName { get; set; }
        public string LocZn { get; set; }
        public string LocPurpDesc { get; set; }
        public string LocQTIDesc { get; set; }
        public string MeasBasisDesc { get; set; }
        public long TotalDesignCapacity { get; set; } = 0;
        public long UnsubscribeCapacity { get; set; }
        public DateTime? PostingDateTime { get; set; }
        public DateTime? EffectiveGasDayTime { get; set; }
        public DateTime? EndingEffectiveDay { get; set; }
        public decimal AvailablePercentage { get; set; }
    }
}
