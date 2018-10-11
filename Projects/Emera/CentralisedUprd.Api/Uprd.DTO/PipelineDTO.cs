using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralisedUprd.Api.UPRD.DTO
{
    public class PipelineListDTO
    {

        List<PipelineDTO> pipelines = new List<PipelineDTO>();

        public List<PipelineDTO> PipelineList { get { return pipelines; } set { pipelines = value; } }
    }

    public class PipelineDTO
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public string NameWithoutDuns { get; set; }
        public string DUNSNo { get; set; }
        public int TSPId { get; set; }
        public int ModelTypeID { get; set; }
        public bool ToUseTSPDUNS { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string TempItem { get; set; }
        public bool IsUprdActive { get; set; }
    }
}