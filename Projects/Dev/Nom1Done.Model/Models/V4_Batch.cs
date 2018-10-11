using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nom1Done.Model
{
    public class V4_Batch //: EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TransactionID { get; set; }
        public DateTime FlowStartDate { get; set; }
        public DateTime FlowEndDate { get; set; }
        public int CycleId { get; set; }
        public int StatusID { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public int PipelineID { get; set; }
        public string PipelineDuns { get; set; }
        public string ServiceRequester { get; set; }
        public string Description { get; set; }
        public bool ShowZeroCheck { get; set; }
        public bool RankingCheck { get; set; }
        public bool PakageCheck { get; set; }
        public bool UpDnContractCheck { get; set; }
        public bool ShowZeroUp { get; set; }
        public bool ShowZeroDn { get; set; }
        public bool UpDnPkgCheck { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ReferenceNumber { get; set; }
        public string TransactionSetControlNumber { get; set; }
        public int? NomTypeID { get; set; }
        public string NomSubCycle { get; set; }        

        public virtual List<V4_Nomination> Nominations { get; set; }

    }
}
