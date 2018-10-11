using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.DTO
{
    public class SQTSOPPerTransactionDTO
    {
        public long Id { get; set; }
        public Guid TransactionId { get; set; }
        public DateTime StatementDate { get; set; }
        public string PreparerID { get; set; }
        public string Statement_ReceipentID { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime EffectiveEndDate { get; set; }
        public string CycleIndicator { get; set; }

        public string LocationCapacityFlowIndicator { get; set; }

        public string ConfirmationRole { get; set; }

        public string Location { get; set; }
        public long LocationNetCapacity { get; set; }
        public string ServiceContract { get; set; }
        public string ServiceIdentifierCode { get; set; }
        public string ConfirmationTrackingID { get; set; }
        public long Quantity { get; set; }
        public string ContractualFLowIndicator { get; set; }
        public string ConfirmationSusequenceCycleIndicator { get; set; }
        public string ReductionReason { get; set; }
        public string SchedulingStatus { get; set; }
        public long ReductionQuantity { get; set; }
        public string ServiceRequester { get; set; }
        public string DownstreamParty { get; set; }
        public string UpstreamParty { get; set; }
        public string ServiceRequesterContract { get; set; }
        public string DwnStreamShipperContract { get; set; }
        public string UpstrmShipperContract { get; set; }
        public string DownPkgId { get; set; }
        public string UpstrmPkgId { get; set; }

        // Define according to SQOP Table Mockup
        public string PkgId { get; set; }
        public long DelQuantity { get; set; }

        public string PackageId { get; set; }
        public string ConfirmationUserData1 { get; set; }
        public string ConfirmationUserData2 { get; set; }
    }
}
