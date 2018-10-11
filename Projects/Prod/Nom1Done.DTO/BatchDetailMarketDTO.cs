using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nom1Done.DTO
{
    public class BatchDetailMarketDTO
    {
        #region Public Properties
        public long ID { get; set; }

        public String LocationProp { get; set; }

        public String LocationName { get; set; }

        public String Location { get; set; }

        public String TransactionTypeDescription { get; set; }

        public String TransactionType { get; set; }
        public int TransTypeMapId { get; set; }
        public string PathType { get; set; }

        public String ServiceRequestNo { get; set; }

        public String ServiceRequestType { get; set; }

        public String DownstreamIDProp { get; set; }

        public String DownstreamIDName { get; set; }

        public String DownstreamID { get; set; }

        public String DefaultInd { get; set; }

        public int ReceiptQuantityGross { get; set; } = 0;

        public String FuelPercentage { get; set; }

        public String FuelQunatity { get; set; }

        public int DeliveryQuantityNet { get; set; } = 0;

        public String DnstreamRank { get; set; }

        public String PackageID { get; set; }
        public String DnPackageID { get; set; }
        public Boolean IsActive { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public Guid BatchID { get; set; }
        public String DnContractIdentifier { get; set; }

        public bool IsHidden { get; set; }
        //  public String DnPackageID { get; set; }

        public string RecPointQty { get; set; }
        public string DelPointQty { get; set; }
        public string ReductionReason { get; set; }
        public string PipeDuns { get; set; }

        public string TransactionId { get; set; }
        public string NominatorTrackingId { get; set; }

        #endregion
    }
}