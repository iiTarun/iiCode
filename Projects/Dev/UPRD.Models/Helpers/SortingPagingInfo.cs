using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPRD.Model.Helpers
{
    public class SortingPagingInfo
    {
        public int PageNo { get; set; } = 1;
        public int CurrentPageIndex { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageCount { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }



    }
}