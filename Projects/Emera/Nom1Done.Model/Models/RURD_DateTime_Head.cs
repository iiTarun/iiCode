//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nom1Done.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class RURD_DateTime_Head
    {
        [Key]
        public long DateTimeHeadID { get; set; }
        public System.Guid TransactionID { get; set; }
        public string DateTimeQuailifier { get; set; }
        public string DateTimePeriodFormat { get; set; }
        public string DataProcessingDate { get; set; }
        public string DataProcessingTime { get; set; }
    }
}
