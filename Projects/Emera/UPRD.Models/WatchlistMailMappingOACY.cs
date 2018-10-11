using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPRD.Model
{
    public class WatchlistMailMappingOACY
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public int WatchListId { get; set; }      
        public long OACYID { get; set; }
        public Nullable<System.DateTime> EmailSentDateTime { get; set; }
        public bool IsMailSent { get; set; } = false;
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
        public DateTime? PostingDateTime { get; set; }




    }
}
