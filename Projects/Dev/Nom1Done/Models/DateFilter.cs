using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nom1Done.Models
{
    public class DateFilter
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string status { get; set; }
        public bool showMine { get; set; }
    }


    public class SwntDateFilter
    {
        public DateTime PostStartDate { get; set; }
        public DateTime PostEndDate { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime EffectiveEndDate { get; set; }
        public string Keyword { get; set; }

        public string IsCritical { get; set; }
        public int WatchListId { get; set; }
    }


    public class PathedFilters
    {
        public string PipelineDuns { get; set; }
        public int StatusId { get; set; } = -1;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public bool Showmine { get; set; }

    }

}