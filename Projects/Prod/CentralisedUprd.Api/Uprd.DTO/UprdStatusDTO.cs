using System;

namespace CentralisedUprd.Api.UPRD.DTO
{
    public class UPRDStatusDTO
    {
        public System.Guid UPRD_ID { get; set; }
        public string RequestID { get; set; }
        public Nullable<System.Guid> RURD_ID { get; set; }
        public string Pipeline { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Guid? TransactionId { get; set; }
        public string DatasetSummary { get; set; }
        public int? DatasetRequested { get; set; }
        public bool IsRURDReceived { get; set; }
        public bool IsDataSetAvailable { get; set; }
        public bool IsDatasetReceived { get; set; }
        public string PipeDuns { get; set; }
    }

}