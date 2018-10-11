using System;
using System.ComponentModel.DataAnnotations;

namespace Nom1Done.Model
{
    public class NominationStatu
    {
        [Key]
        public int Id { get; set; }
       
        public long NomStatusID { get; set; }
        public Guid NOM_ID { get; set; }
        public string NMQR_ID { get; set; }
        public string ReferenceNumber { get; set; }
        public int StatusID { get; set; }
        public string StatusDetail { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsAutomated { get; set; }
    }
}
