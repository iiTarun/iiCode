//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UPRD.Model
{
    using System;
    using System.Collections.Generic;
    
    public class GISBOutbox
    {
        public int Id { get; set; }
        public string MessageID { get; set; }
        public string Gisb { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
