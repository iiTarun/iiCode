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
    
    public partial class WatchlistMailMappingOACy
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public int WatchListId { get; set; }
        public long OACYID { get; set; }
        public Nullable<System.DateTime> EmailSentDateTime { get; set; }
        public bool IsMailSent { get; set; }
        public string WatchlistName { get; set; }
        public string MoreDetailUrl { get; set; }
        public string CycleIndicator { get; set; }
        public int PipelineID { get; set; }
        public string PipelineName { get; set; }
        public string Loc { get; set; }
        public string LocName { get; set; }
        public long OperatingCapacity { get; set; }
        public long TotalScheduleQty { get; set; }
        public decimal AvailablePercentage { get; set; }
        public Nullable<System.DateTime> PostingDateTime { get; set; }
    }
}
