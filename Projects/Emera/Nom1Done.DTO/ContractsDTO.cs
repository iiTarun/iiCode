using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nom.ViewModel
{

    public class ContractListDTO
    {
        
       public  List<ContractsDTO> _contractLst = new List<ContractsDTO>();

        public int PipelineId { get; set; }

        public Guid ShipperId { get; set; }

        public string PipelineDetails { get; set; }

        public List<ContractsDTO> ContractList { get { return _contractLst; } set { _contractLst = value; } }

    }


    public class ContractsDTO
    {
        public string RequestNo { get; set; }
        public decimal FuelPercent { get; set; }
        public decimal MDQ { get; set; }
        public int ID { get; set; }
        public int? RequestTypeID { get; set; }
        public decimal FuelPercentage { get; set; }
        public int LocationFromID { get; set; }
        public string LocationFrom { get; set; }
        public int LocationToID { get; set; }
        public string LocationTo { get; set; }
        public DateTime ValidUpto { get;  set; }
        public int PipelineID { get;  set; }
        public int ShipperID { get;  set; }
        public bool IsActive { get;  set; }
        public string CreatedBy { get;  set; }
        public DateTime CreatedDate { get;  set; }
        public string ModifiedBy { get;  set; }
        public DateTime ModifiedDate { get;  set; }
        public string RequestType { get; set; }

        public string ReceiptZone { get; set; }
        public string DeliveryZone { get; set; }
        public string PipeDuns { get; set; }
        public string RateSchedule { get; set; }      
    }
}