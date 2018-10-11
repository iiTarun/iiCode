using System;
using System.ComponentModel.DataAnnotations;

namespace Nom1Done.Model
{
    public class Pipeline_TransactionType_Map
    {
        [Key]
        public int ID { get; set; }
        public int PipelineID { get; set; }
        public int TransactionTypeID { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string PathType { get; set; }
        public string PipeDuns { get; set; }

        public bool IsSpecialLocs { get; set; }
    }
}
