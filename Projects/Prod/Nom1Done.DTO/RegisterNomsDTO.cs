using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nom.ViewModel
{
    public class RegisterNomsDTO
    {
        public Guid TransactionID { get; set; }
        public string TransactionSetControlNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string VerVal { get; set; }
        public DateTime DateTimeReference { get; set; }
        public string TSPDUNS { get; set; }
        public string TSPPropCode { get; set; }
        public string ServiceRequestorDUNS { get; set; }
        public string ServiceRequestorPropCode { get; set; }
        public string NOMType { get; set; }
    }
}