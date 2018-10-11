using Nom1Done.DTO;
using System;
using System.Collections.Generic;


namespace Nom.ViewModel
{

    public class PathedDTO {

        public string PipelineModelType { get; set; } = "Pathed";    // Pathed  or PathedNonPathedHybrid
        public int PipelineID { get; set; }
        public string PipelineDuns { get; set; }
        public string DunsNo { get; set; }     // shipperDuns. 
        public string PipelineDetails { get; set; }
        public int CurrentContractRow { get; set; }

        public Guid? ShipperID { get; set; }

        public int companyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StatusId { get; set; } = -1;
        public bool showMine { get; set; } = true;


        public List<PathedNomDetailsDTO> PathedNomsList { get; set; }     

        public SortingPagingInfo SortingPagingInfo { get; set; }

        public List<NonPathedRecieptNom> ReceiptNoms { get; set; } = new List<NonPathedRecieptNom>();
        public List<NonPathedDeliveryNom> DeliveryNoms { get; set; } = new List<NonPathedDeliveryNom>();

        public NomType PipelineNomType { get; set; }

    }

    public class PathedNomDetailsDTO
    {
        public int ID { get; set; }      
        public Guid TransactionId { get; set; }
        public int StatusID { get; set; }
        public string Status { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public string EndTime { get; set; }
        public int CycleID { get; set; }
        public string CycleName { get; set; }
        public string Contract { get; set; } 
        public string NomSubCycle { get; set; }
        public string ActCode { get; set; }
        public string TransType { get; set; }
        public int TransTypeMapId { get; set; }
        public string TransTypeName { get; set; }
        public int PathedHybridNonpathedType { get; set; }
        public string RecLocation { get; set; } 
        public string RecLocProp { get; set; } 
        public string RecLocID { get; set; } 
        public string UpName { get; set; }
        public string UpIDProp { get; set; } 
        public string UpID { get; set; }
        public string UpKContract { get; set; }
        public string RecRank { get; set; }
        public string PkgIDRec { get; set; }
        public string DelLoc { get; set; }
        public string DelLocProp { get; set; }
        public string DelLocID { get; set; } 
        public string DownName { get; set; } 
        public string DownIDProp { get; set; }
        public string DownID { get; set; }
        public string DownContract { get; set; } 
        public string DelRank { get; set; } 
        public string PkgID { get; set; }
        public string NomTrackingId { get; set; }
        public string UpPkgID { get; set; }
        public string UpRank { get; set; }
        public string DownPkgID { get; set; }
        public string DownRank { get; set; }
        public string BidTransportRate { get; set; }
        public string QuantityType { get; set; }
        public string MaxRate { get; set; }
        public string CapacityType { get; set; }
        public string BidUp { get; set; }
        public string Export { get; set; }
        public string ProcessingRights { get; set; }
        public string AssocContract { get; set; }
        public string DealType { get; set; }
        public string NomUserData1 { get; set; }
        public string NomUserData2 { get; set; }
        public string RecQty { get; set; }
        public decimal FuelPercentage { get; set; }
        public decimal DelQuantity { get; set; }
        public bool IsScheduled { get; set; }
        public System.DateTime ScheduledDateTime { get; set; }       
        public string ReferenceNo { get; set; }
        public int PipelineID { get; set; }
        public int CompanyID { get; set; }
        public string ShipperDuns { get; set; }
        public bool CanWrite { get; set; }

        public string RecPointQty { get; set; }
        public string DelPointQty { get; set; }

        public string ReductionReason { get; set; }
        public DateTime CreatedDate { get; set; }
        public string createrName { get; set; }
        public string CreatedBy { get; set; }
        public bool IsModify { get; set; } = false;
        public string PipelineDuns { get; set; }

    }



}