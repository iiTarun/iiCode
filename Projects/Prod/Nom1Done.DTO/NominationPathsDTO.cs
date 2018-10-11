using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nom.ViewModel
{
    public class NominationPathsDTO
    {
        public Guid NominationPathID { get; set; }
        public Guid TransactionID { get; set; }
        public DateTime BeginingDate { get; set; }
        public DateTime BeginingTime { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime EndTime { get; set; }
        public string CycleIndicator { get; set; }
        public string ContractNumber { get; set; }
        public string PathType { get; set; }
        public string ModelType { get; set; }

    }
}