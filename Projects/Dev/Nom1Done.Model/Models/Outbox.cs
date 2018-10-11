using System;

namespace Nom1Done.Model
{
    public class Outbox
    {
        public int ID { get; set; }
        public Guid MessageID { get; set; }
        public int DatasetID { get; set; }
        public int StatusID { get; set; }
        public string GISB { get; set; }
        public bool IsTest { get; set; }
        public int PipelineID { get; set; }
        public string PipelineDuns { get; set; }
        public int CompanyID { get; set; }
        public bool IsScheduled { get; set; }
        public DateTime ScheduledDate { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
