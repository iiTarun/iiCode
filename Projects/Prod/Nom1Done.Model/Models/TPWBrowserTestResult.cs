namespace Nom1Done.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TPWBrowserTestResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BrowserTestID { get; set; }
        public int PipelineID { get; set; }
        public string PipelineName { get; set; }
        public bool? IsProduction { get; set; }
        public bool? IsWorking { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
