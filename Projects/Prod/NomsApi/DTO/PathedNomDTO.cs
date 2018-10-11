using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NomsApi.DTO
{
    public class PathedNomDTO
    {
      
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string CycleCode { get; set; }
        public string ServiceRequesterContractCode { get; set; }
        public string NomSubCycle { get; set; }
        public string TransactionType { get; set; }
        public string ReceiptLocId { get; set; }
        public string UpstreamId { get; set; }
        public string UpstreamK { get; set; }   
        public string ReceiptRank { get; set; }
        public string DeliveryLocId { get; set; }
        public string DnstreamId { get; set; }
        public string DnstreamK { get; set; }   
        public string DeliveryRank { get; set; }
        public string PackageId { get; set; }   // Non-Mandatory
        public double RecQty { get; set; } = 0.0;     // By default zero
        public double FuelPercentage { get; set; } = 0.0;   // By default zero
        public double DelQuantity { get; set; } = 0.0;   // By default zero
        public string KochId { get; set; }
        public bool IsValideNom { get; set; }   // For Internal-Use only // Non-Mandatory

    }
}