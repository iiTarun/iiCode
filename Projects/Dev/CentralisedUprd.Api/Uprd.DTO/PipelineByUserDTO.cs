using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralisedUprd.Api.UPRD.DTO
{
    public class PipelineByUserDTO
    {
        public string UserID { get; set; }
        public int ShipperID { get; set; }
        public string Keyword { get; set; }
    }
}