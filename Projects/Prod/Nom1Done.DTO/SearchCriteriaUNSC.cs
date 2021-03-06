﻿using Nom1Done.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nom1Done.Nom.ViewModel
{
    public class SearchCriteriaUNSC
    {
        public Nullable<int> PipelineID { get; set; }
        public string PipelineDuns { get; set; }

        public string keyword { get; set; } = string.Empty;
        public string sort { get; set; } //SortField
        public string SortDirection { get; set; } //SortDirection
        public int size { get; set; } //PageSize
        public int page { get; set; } //CurrentPageIndex

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? postStartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? postEndDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EffectiveStartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EffectiveEndDate { get; set; }
        public string userId { get; set; }
        public IEnumerable<UnscPerTransactionDTO> UnscPerTransactionViewModel { get; set; }
        public SearchCriteriaUNSC()
        {           
            page = 1;
            size = 10;         
        }
        public bool IsClearFilter { get; set; }
        public int WatchlistId { get; set; }
    
        public bool flagDefault { get; set; } = true;
        public int RecordCount { get; set; }
            
    }

    public class UnscDataFilter
    {
        public Nullable<int> PipelineID { get; set; }

        public string PipelineDuns { get; set; }

        public string keyword { get; set; } = string.Empty;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? postStartDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? StartEffectiveGasDate { get; set; }
        public DateTime? EndEffectiveGasDate { get; set; }

        public int WatchListId { get; set; }      
     
        public bool flagDefault { get; set; } = true;
    }


}