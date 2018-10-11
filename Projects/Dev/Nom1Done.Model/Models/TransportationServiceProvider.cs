using System;
using System.ComponentModel.DataAnnotations;

namespace Nom1Done.Model
{
    public class TransportationServiceProvider
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string DUNSNo { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
