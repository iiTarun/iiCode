namespace UPRD.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Pipeline
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string DUNSNo { get; set; }
        public int TSPId { get; set; }
        public int ModelTypeID { get; set; }
        public bool ToUseTSPDUNS { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsUprdActive { get; set; }
    }
}
