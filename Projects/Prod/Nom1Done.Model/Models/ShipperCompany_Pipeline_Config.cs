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
    
    public class ShipperCompany_Pipeline_Config
    {
        
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public int PipelineID { get; set; }
        public bool IsLocationPropCodeRequired { get; set; }
        public bool IsCounterPartyPropCodeRequired { get; set; }
        public bool IsDeliveredQuantityRequired { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
    }
}
