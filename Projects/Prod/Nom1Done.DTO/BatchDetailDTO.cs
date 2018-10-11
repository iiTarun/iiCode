using Nom.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nom1Done.DTO
{
    public class BatchDetailDTO
    {
        public string CurrentTableId { get; set; }
        public int CurrentContractRow { get; set; }
        public int CurrentSupplyRow { get; set; }
        public int CurrentMarketRow { get; set; }
        public Guid Id { get; set; }
        public int PipelineId { get; set; }
        public int ShipperCompanyId { get; set; }
        public int CycleId { get; set; }
        public string PipeLineName { get; set; }
        public string Duns { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public string ShiperName { get; set; }
        public string ShiperDuns { get; set; }
        public string BatchStatus { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime SubmittedDate { get; set; }
        public DateTime ScheduleDate { get; set; }
        public bool ShowZeroCheck { get; set; }
        public bool RankingCheck { get; set; }
        public bool PakageCheck { get; set; }
        public bool UpDnContractCheck { get; set; }
        public bool ShowZeroUp { get; set; }
        public bool ShowZeroDn { get; set; }
        public bool UpDnPkgCheck { get; set; }
        public bool IsPNT { get; set; }
        public int StatusId { get; set; }



        public List<BatchDetailSupplyDTO> _supply = new List<BatchDetailSupplyDTO>();
        public List<BatchDetailMarketDTO> _market = new List<BatchDetailMarketDTO>();
        public List<BatchDetailContractPathDTO> _transportContract = new List<BatchDetailContractPathDTO>();
        public List<BatchDetailContractDTO> _transport = new List<BatchDetailContractDTO>();
        public List<BatchDetailSupplyDTO> SupplyList { get { return _supply; } set { _supply = value; } }
        public List<BatchDetailMarketDTO> MarketList { get { return _market; } set { _market = value; } }
        public List<BatchDetailContractPathDTO> ContractPath { get { return _transportContract; } set { _transportContract = value; } }
        public List<BatchDetailContractDTO> Contract { get { return _transport; } set { _transport = value; } }

        public string BegginingTime { get; set; }
        public string EndTime { get; set; }
        public int supplyRecTotal { get; set; }
        public int supplyDelTotal { get; set; }
        public int marketRecTotal { get; set; }
        public int marketDelTotal { get; set; }
       // public PathedDTO PathedHybrid { get; set; }
        public List<PathedNomDetailsDTO> PathedNomsList { get; set; } = new List<PathedNomDetailsDTO>();

        public NomType PipelineModelType { get; set; }

        public int BatchNomType { get; set; }

        public string NomSubCycle { get; set; }

        // To get NonPathedHybrid
        public List<NonPathedRecieptNom> ReceiptNoms { get; set; } = new List<NonPathedRecieptNom>();
        public List<NonPathedDeliveryNom> DeliveryNoms { get; set; } = new List<NonPathedDeliveryNom>();

    }
}