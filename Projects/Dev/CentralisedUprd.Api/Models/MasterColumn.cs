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
    
    public partial class MasterColumn
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public int DataSetId { get; set; }
        public int DataTypeId { get; set; }
    
        public virtual DataTypeGrouping DataTypeGrouping { get; set; }
    }
}
