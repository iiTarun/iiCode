using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nom1Done.DTO
{
    public class BatchDetailContractPathDTO
    {
        #region Public Properties
        public long ID { get; set; }

        public String ServiceRequestNo { get; set; }

        public String ServiceRequestType { get; set; }

        public String FuelPercentage { get; set; }

        public String DefaultInd { get; set; }

        public String MaxDeliveredQuantity { get; set; }

        public String NominatedQuantity { get; set; }

        public String OverrunQuantity { get; set; }

        public Guid BatchID { get; set; }

        public Boolean IsActive { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }
        public bool IsHidden { get; set; }
        public int transportRecTotal { get; set; }
        public int transportDelTotal { get; set; }

        #endregion
    }
}