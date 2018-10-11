using System;
using System.ComponentModel.DataAnnotations;

namespace Nom1Done.Model
{
    public class CounterParty
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Identifier { get; set; }
        public string PropCode { get; set; }
        public int PipelineID { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string PipeDuns { get; set; }
    }
}
