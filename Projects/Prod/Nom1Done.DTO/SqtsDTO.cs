using System;

namespace Nom1Done.DTO
{
    public class SqtsDTO
    {
        public string RecLoc { get; set; }
        public string DelLoc { get; set; }

        public DateTime StatementDatetime { get; set; }
        public DateTime BeginingDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Cycle { get; set; }
        public string ReductionReason { get; set; }
        public int RecQty { get; set; }
        public int DelQty { get; set; }
    }
}
