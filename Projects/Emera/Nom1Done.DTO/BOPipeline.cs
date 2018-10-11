using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.DTO
{
   public class BOPipeline
    {

        #region Public Properties
        public Int32 ID { get; set; }

        public String Name { get; set; }

        public String DUNSNo { get; set; }

        public Int32 TSPId { get; set; }

        public Int32 ModelTypeID { get; set; }

        public Boolean ToUseTSPDUNS { get; set; }

        public Boolean IsActive { get; set; }

        public String CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public String ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }
        public string TempItem { get; set; }

        #endregion
    }
}
