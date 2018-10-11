namespace Nom1Done.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TaskMgrReceiveMultipuleFile
    {
        [Key]
        public Guid ReceiveFileID { get; set; }
        public string TransactionId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string FileName { get; set; }
        public bool? IsAdd { get; set; }
        public int? PipelineID { get; set; }
        public int? DatasetId { get; set; }
        public string ReceiveData { get; set; }
    }
}
