using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NomsApi.DTO
{
    public class NonPathedDelNom
    {     
          
        public string DeliveryLocId { get; set; }      
        public string DnstreamId { get; set; }      
        public string DnstreamK { get; set; }   // Non-Mandatory
        public double DeliveryQty { get; set; } = 0.0;   // By default zero
        public string DeliveryRank { get; set; }    
        public string TransactionType { get; set; }     
        public string PackageId { get; set; }     // Non-Mandatory       
        public double FuelPercentage { get; set; } = 0.0;  // By default zero
        public string KochId { get; set; }
        public bool IsValideNom { get; set; }  // For Internal-Use Only // Non-Mandatory

    }
}