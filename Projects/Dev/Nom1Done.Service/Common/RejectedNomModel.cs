using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Common
{
    public class RejectedNomModel
    {
        public Guid TransactionID { get; set; }
        public int PipelineID { get; set; }
        public string PipelineName { get; set; }
        public DateTime FlowDate { get; set; }
        public string RejectionReason { get; set; }
    }
}
