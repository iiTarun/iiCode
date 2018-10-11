using System;

namespace Nom1Done.DTO
{
    public class RejectedNomModel
    {
        public string NMQR_ID { get; set; }
        public Guid TransactionID { get; set; }
      
        public string PipelineDuns { get; set; }
        public string PipelineName { get; set; }
        public DateTime FlowDate { get; set; }
        public string RejectionReason { get; set; }
    }
}
