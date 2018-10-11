using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nom.ViewModel
{
    public class NominationSublineItemsDTO
    {
        public Guid NominationPathID { get; set; }
        public Guid TransactionID { get; set; }
        public string BidTransportationRate { get; set; }
        public string ImbalancePeriod { get; set; }
        public string SublineItemExtendedReference { get; set; }
        public string SublineItemIndustryCodeID { get; set; }
        public Guid SublineItemID { get; set; }
    }
}