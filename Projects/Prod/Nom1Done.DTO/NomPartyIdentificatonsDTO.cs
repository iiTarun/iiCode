using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nom.ViewModel
{
    public class NomPartyIdentificatonsDTO
    {
        public Guid SublineItemID { get; set; }
        public Guid NomPathId { get; set; }
        public Guid TransactionId { get; set; }
        public string UpstreamIdentifierPropCode { get; set; }
        public string DownstreamIdentifierPropCode { get; set; }
        public string UpstreamIdentifier { get; set; }
        public string DownstreamIdentifier { get; set; }
        public string ReceiptLocation { get; set; }
        public string ReceiptLocationPropCode { get; set; }
        public string DeliveryLocation { get; set; }
        public string DeliveryLocationPropCode { get; set; }
        public string AssignedIdentification { get; set; }
        public string NomPartyExtendedReference { get; set; }
        public string NomPartyIndustryCode { get; set; }
        public string NomPartyQuantityInfo { get; set; }
    }
}