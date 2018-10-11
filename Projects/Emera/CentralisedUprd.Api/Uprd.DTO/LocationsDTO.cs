using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralisedUprd.Api.UPRD.DTO
{
    public class LocationsDTO
    {
        public string Name { get; set; }

        public string RDB { get; set; }
        public int ID { get; set; }
        public string Identifier { get; set; }
        public string PropCode { get; set; }
        public int RDUsageID { get; set; }
        public int PipelineID { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}