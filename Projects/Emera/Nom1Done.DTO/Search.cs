using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.DTO
{
    public class NominationDelete
    {
        public string MessageID { get; set; }

        public string UserID { get; set; }
    }
    public class Search
    {
        public string PipelineDuns { get; set; }
        public int PipelineID { get; set; }
        public string keyword { get; set; }
        public int page { get; set; }
        public int size { get; set; }
        public string sort { get; set; }
        public DateTime postStartDate { get; set; }
        public DateTime postEndDate { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime EffectiveEndDate { get; set; }
        public string userId { get; set; }
        public string Cycle { get; set; }
    }
    public class NominationSearchCriteria
    {
        public int RecipientCompanyID { get; set; }
        public int PipelineID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StatusID { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
