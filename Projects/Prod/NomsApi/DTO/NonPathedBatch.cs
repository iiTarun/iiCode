using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NomsApi.DTO
{
    public class NonPathedBatch
    {
       
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string CycleCode { get; set; }
        public string ServiceRequesterContractCode { get; set; }
        public string NomSubCycle { get; set; }
        public List<NonPathedRecNom> NonPathedRecNomList { get; set; } = new List<NonPathedRecNom>();
        public List<NonPathedDelNom> NonPathedDelNomList { get; set; } = new List<NonPathedDelNom>();
    }
}