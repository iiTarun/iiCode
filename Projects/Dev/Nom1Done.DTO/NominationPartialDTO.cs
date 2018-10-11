using Nom1Done.DTO;
using Nom1Done.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nom.ViewModel
{
    public class NominationPartialDTO
    {
        public string OnFlyContractNumber { get; set; }
        public string OnFlyFuelPercentage { get; set; } = "0";
        public string ForRow { get; set; }
        public string PopUpFor { get; set; }
        public int PipelineId { get; set; }

        public string PipelineDuns { get; set; }

        public List<Route> Routes { get; set; }

        public List<LocationsDTO> Locations { get; set; }
        public List<ContractsDTO> Contracts { get; set; }
        public List<CounterPartiesDTO> CounterParties { get; set; }
        public List<TransactionTypesDTO> TransactionTypes { get; set; }
        public List<RejectionReasonDTO> StatusReason { get; set; } = new List<RejectionReasonDTO>();
    }


    public class WatchListPopUpPartialDTO
    { 
        public string RowId { get; set; }
        public EnercrossDataSets DataSet { get; set; } 
        public string PipelineDuns { get; set; }
        public List<LocationsDTO> Locations { get; set; }       
      
    }

}