namespace Nom1Done.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TaskMgrJob
    {
        [Key]
        public long Id { get; set; }
        public string TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int StatusId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsSending { get; set; }
        public int? StageId { get; set; }
        public int? DatasetId { get; set; }
        public int? EDICheckCount { get; set; }
        public int? NoOfXmlInEDI { get; set; }
    }
}
