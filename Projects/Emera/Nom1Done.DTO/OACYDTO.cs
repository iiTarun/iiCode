using System;

namespace Nom1Done.DTO
{
    public class OACYDTO
    {
        public DateTime? PostingDateTime { get; set; }
        public string Loc { get; set; }
        public DateTime? EffectiveGasDayTime { get; set; }
        public string TransactionServiceProvider { get; set; }
        public string FlowIndicator { get; set; }
        public string ITIndicator { get; set; }
        public string CycleIndicator { get; set; }
        public long TotalScheduleQty { get; set; }
    }
}
