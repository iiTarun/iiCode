namespace Nom1Done.Model
{
    using System;
    public class UploadedFile
    {
        public long ID { get; set; }
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }
        public string AddedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
         // public int PipelineId { get; set; }
        public string PipelineDuns { get; set; }
    }
}
