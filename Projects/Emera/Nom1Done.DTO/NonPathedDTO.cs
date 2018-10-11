using Nom.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.DTO
{
    public class NonPathedBatchListDTO {

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StatusId { get; set; }

        public int PipelineId { get; set; }

        public List<BatchDTO> BatchDtoList { get; set; }
    }


    public class NonPathedDTO
    {
        public int PipelineId { get; set; }

        public string PipelineDuns { get; set; } 

        public Guid? UserId { get; set; }

        public string ShipperDuns { get; set; }

        public int CompanyId { get; set; }

        public Guid? TransactionId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StatusId { get; set; } = -1;        
        public List<NonPathedRecieptNom> ReceiptNoms { get; set; } = new List<NonPathedRecieptNom>();
        public List<NonPathedDeliveryNom> DeliveryNoms { get; set; } = new List<NonPathedDeliveryNom>();
        //public decimal DeliveryQtyTotal { get; set; }
        //public decimal ReceiptQtyTotal { get; set; }
    }


    public class NonPathedRecieptNom
    {
        public Guid? TransactionId { get; set; }
        public int PipelineId { get; set; }
        public string ShipperDuns { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string Cycle { get; set; }
        public int CycleId { get; set; }
        public string CreatedBy { get; set; }
        public string NomSubCycle { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime CreateDateTime { get; set; }

      
        public string ReceiptLocId { get; set; }
        public string ReceiptLocProp { get; set; }
        public string ReceiptLocName { get; set; }
        public string UpstreamName { get; set; }
        public string UpstreamId { get; set; }
        public string UpstreamProp { get; set; }
        public string UpstreamK { get; set; }
        public decimal ReceiptQty { get; set; }
        public string ReceiptRank { get; set; }
   
        public string TransactionType { get; set; }
        public string TransactionTypeDesc { get; set; }

        public int TransTypeMapId { get; set; }

        public string PackageId { get; set; }
       
         public string NomTrackingId { get; set; }      
        public string ServiceRequesterContractName { get; set; }
        public string ServiceRequesterContractCode { get; set; }      
        
        public decimal FuelPercentage { get; set; } 
      
    }

    public class NonPathedDeliveryNom {

        public Guid? TransactionId { get; set; }
        public int PipelineId { get; set; }
        public string ShipperDuns { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public string Cycle { get; set; }
        public int CycleId { get; set; }
        public string CreatedBy { get; set; }
        public string NomSubCycle { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime CreateDateTime { get; set; }
       
        public string DeliveryLocId { get; set; }
        public string DeliveryLocProp { get; set; }
        public string DeliveryLocName { get; set; }
        public string DnstreamName { get; set; }
        public string DnstreamId { get; set; }
        public string DnstreamProp { get; set; }
        public string DnstreamK { get; set; }
        public decimal DeliveryQty { get; set; }
        public string DeliveryRank { get; set; }
      
        public string TransactionType { get; set; }
        public string TransactionTypeDesc { get; set; }

        public int TransTypeMapId { get; set; }

        public string PackageId { get; set; }
        public string NomTrackingId { get; set; }
      
        public string ServiceRequesterContractName { get; set; }
        public string ServiceRequesterContractCode { get; set; }
        public decimal FuelPercentage { get; set; }


    }
}
