using System;
using System.Collections.Generic;

namespace Nom1Done.DTO
{
    public class SummaryDTO
    {
        public string Username { get; set; }
        public string Cycle { get; set; }
        public string UpStreamName { get; set; }
        public string DownStreamName { get; set; }
        public string PkgId { get; set; }
        public string NomTrackingId { get; set; }
        public DateTime SubmittedDate { get; set; }
        public DateTime StatementDate { get; set; }
        public int? Qty { get; set; }
        public string RecLoc { get; set; }
        public string DelLoc { get; set; }
        public DateTime nomStartDate { get; set; }
        public DateTime nomEndDate { get; set; }
        public string UpIdentifier { get; set; }
        public string DwnIdentifier { get; set; }
        public string ContractSVC { get; set; }
        public int RecPointQty { get; set; }
        public int DelPointQty { get; set; }
        public string ReductionReason { get; set; }
        public string ReductionReasonDetail { get; set; }
    }

    public class SummaryTestDTO
    {
        public string NomTrackingId { get; set; }
        public string upname { get; set; }
        public string downname { get; set; }
        public string cycle { get; set; }
        public string pkgId { get; set; }
        public string RecLoc { get; set; }
        public string DelLoc { get; set; }
        public string statementDate { get; set; }
        public int? Qty { get; set; }

    }

    public class SummaryValues
    {
        public DateTime? date;
        public int? Qty;
    }

    public class DummyDatesModel
    {





    }


}
