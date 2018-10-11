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
    using System.ComponentModel.DataAnnotations.Schema;

    public class UPRDStatu:EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid UPRD_ID { get; set; }
        public string RequestID { get; set; }
        public Nullable<System.Guid> RURD_ID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Guid? TransactionId { get; set; }
        public string DatasetSummary { get; set; }
        public int? DatasetRequested { get; set; }
        public bool IsRURDReceived { get; set; }
        public bool IsDataSetAvailable { get; set; }
        public bool IsDatasetReceived { get; set; }
        public string PipeDuns { get; set; }
    }
}