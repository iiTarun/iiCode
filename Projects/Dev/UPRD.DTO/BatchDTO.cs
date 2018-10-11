using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPRD.DTO
{

    public class BatchNomCollection {

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StatusId { get; set; }
        public string pipelineDuns { get; set; }
        public bool showMine { get; set; }
       public List<BatchDTO> BatchList { get; set; }
    }


    public class BatchDTO
    {
        public Guid Id { get; set; }
        public string NominationTrackingID { get; set; }
        public string Description { get; set; }
        public int PipelineId { get; set; }
        public DateTime DateBeg { get; set; }
        public DateTime DateEnd { get; set; }
        public int CycleId { get; set; }
        public bool isTest { get; set; }
        public bool ShowZeroes { get; set; }
        public bool RankingChecked { get; set; }
        public bool PackageChecked { get; set; }
        public bool UpDnContract { get; set; }
        public bool ShowZeroesUp { get; set; }
        public bool ShowZeroesDn { get; set; }
        public bool UpDnPakgID { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public int TabToShow { get; set; }
        public int StatusID { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string SQTSReceived { get; set; }
        public string ServiceRequester { get; set; }
        public string PipelineName { get; set; }
        public string Status { get; set; }
        public string Cycle { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime SubmittedDate { get; set; }
        public int NomTypeID { get; set; }
        public string ReferenceNumber { get; set; }
        public string pipeDUNSNo { get; set; }
        public string CycleCode { get; set; }
        public string NomSubCycle { get; set; }
        public string CreaterName { get; set; }
    }
}