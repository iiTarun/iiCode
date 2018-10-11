using System;
using System.ComponentModel.DataAnnotations;

namespace Nom1Done.Model
{
    public class metadataErrorCode
    {
        [Key]
        public int ID { get; set; }
        public string Code { get; set; }
        public string DataElement { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; }
    }
}
