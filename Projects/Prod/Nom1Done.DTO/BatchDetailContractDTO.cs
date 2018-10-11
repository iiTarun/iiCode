using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nom1Done.DTO
{
    public class BatchDetailContractDTO
    {
        #region Public Properties
        public long ID { get; set; }

        public String TransactionTypeDescription { get; set; }

        public String TransactionType { get; set; }
        public String ServiceRequestNo { get; set; }
        public String RecLocationProp { get; set; }

        public String RecLocationName { get; set; }

        public String RecLocation { get; set; }

        public String DelRank { get; set; }

        public String RecZone { get; set; }

        public String DelLocationProp { get; set; }

        public String DelLocationName { get; set; }

        public String DelLocation { get; set; }

        public String RecRank { get; set; }

        public String DelZone { get; set; }

        public String ReceiptDth { get; set; } = "0";

        public String FuelPercentage { get; set; }

        public String FuelDth { get; set; }

        public String DeliveryDth { get; set; } = "0";

        public String Route { get; set; } = string.Empty;

        public String PackageID { get; set; }

        public String PathRank { get; set; }

        public Int32 TransportContractID { get; set; }

        public Guid BatchID { get; set; }

        public Boolean IsActive { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsHidden { get; set; }

        public string RecPointQty { get; set; }
        public string DelPointQty { get; set; }
        public string ReductionReason { get; set; }
        public string PipeDuns { get; set; }

        public string TransactionId { get; set; }
        public string NominatorTrackingId { get; set; }


        #endregion
    }
}