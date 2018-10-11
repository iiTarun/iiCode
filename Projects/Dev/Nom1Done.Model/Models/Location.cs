namespace Nom1Done.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Location
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Identifier { get; set; }
        public string PropCode { get; set; }
        public int RDUsageID { get; set; }
        public int PipelineID { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string PipeDuns { get; set; }
        public int TransactionTypeId { get; set; }
    }
}
