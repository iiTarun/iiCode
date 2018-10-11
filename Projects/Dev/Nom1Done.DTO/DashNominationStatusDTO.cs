using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.DTO
{
    public class DashNominationStatusDTO
    {
        public int Id { get; set; }
        public Guid TransactionId { get; set; }
        public string Environment { get; set; }
        public string ShipperCompany { get; set; }
        public string UserName { get; set; }
        public string PipelineName { get; set; }
        public string PipeDuns { get; set; }
        public string NomTrackingId { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string NomStatus { get; set; }
        public int StatusId { get; set; }
        public bool AlertTrigger { get; set; }
        public string ShipperDUNS{get;set;}
    }
}
