//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CentralisedUprd.Api.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DashNominationStatu
    {
        public int Id { get; set; }
        public System.Guid TransactionId { get; set; }
        public string Environment { get; set; }
        public string ShipperCompany { get; set; }
        public string UserName { get; set; }
        public string PipelineName { get; set; }
        public string PipeDuns { get; set; }
        public string NomTrackingId { get; set; }
        public System.DateTime SubmittedDate { get; set; }
        public string NomStatus { get; set; }
        public int StatusId { get; set; }
        public bool AlertTrigger { get; set; }
    }
}
