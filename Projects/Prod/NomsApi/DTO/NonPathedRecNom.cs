using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NomsApi.DTO
{
    public class NonPathedRecNom
    {
        public string ReceiptLocId { get; set; }     
        public string UpstreamId { get; set; }
        public string UpstreamK { get; set; }  
        public double ReceiptQty { get; set; } = 0.0;  // By default Zero
        public string ReceiptRank { get; set; }  
        public string TransactionType { get; set; }      
        public string PackageId { get; set; }  // Non-Mandatory                                              
        public double FuelPercentage { get; set; } = 0.0;  // By default Zero
        public string KochId { get; set; }    
        public bool IsValideNom { get; set; }   // For Internal-Use Only // Non-Mandatory    

    }
}