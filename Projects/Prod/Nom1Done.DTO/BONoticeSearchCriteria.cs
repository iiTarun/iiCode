using Nom1Done.DTO;
using Nom1Done.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nom1Done.Nom.ViewModel
{
    public class BONoticeSearchCriteria
    {
        public string Keyword { get; set; }
        public int RecipientCompanyID { get; set; }
        public bool IsCritical { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
        public int PageSize { get; set; }
        public int PageNo { get; set; } //CurrentPageIndex
        public int pipelineId { get; set; }

        public string PipelineDuns { get; set; }

        public int RowsCount { get; set; }
        public List<SwntPerTransactionDTO> NoticeList { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? postStartDate { get; set; } = DateTime.MinValue;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? postEndDate { get; set; }=DateTime.MinValue;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EffectiveStartDate { get; set; } = DateTime.MinValue;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EffectiveEndDate { get; set; } = DateTime.MinValue;
        public int WatchListId { get; set; }
        public int RecordCount { get; set; }
    }
}