using CentralisedUprd.Api.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralisedUprd.Api.UPRD.DTO
{

    public class WatchListCollection
    {
        public string UserId { get; set; }
        public List<WatchListDTO> CollectionWatchList { get; set; }
    }

    public class WatchListDTO
    {
        public WatchListDTO()
        {

        }
        public int id { get; set; } = 0;
        public List<WatchListRule> RuleList { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public UprdDataSet DatasetId { get; set; }
        public string ListName { get; set; }
        public string MoreDetailURLinAlert { get; set; }
    }


    public class WatchListRule
    {
        public int id { get; set; } = 0;
        public int PropertyId { get; set; }
        public int ComparatorsId { get; set; }
        public string value { get; set; }
        public string PipelineDuns { get; set; }
        public string LocationIdentifier { get; set; }
        public bool AlertSent { get; set; } = false;
        public WatchlistAlertFrequency AlertFrequency { get; set; } = WatchlistAlertFrequency.Daily;
        public bool IsCriticalNotice { get; set; } = true;    //Use only for SWNT Dataset
        public string UpperValue { get; set; }
    }

    public class WatchListProperty
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public UprdDataSet DataSet { get; set; }
        public string Datatype { get; set; }
        public int DatatypeId { get; set; }
        public List<WatchListOperator> Operators { get; set; }
    }

    public class WatchListOperator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Title { get; set; }
    }


    public class WatchListAlertExeCutedDataCollectionDTO
    {
        public string UserId { get; set; }
        public List<WatchListAlertExecutedDataDTO> WatchListExecutedDataList { get; set; }
    }

    public class WatchListAlertExecutedDataDTO
    {
        public string UserId { get; set; }
        public WatchListDTO watchList { get; set; }
        public List<SwntPerTransactionDTO> SwntDataList { get; set; }
        public List<UnscPerTransactionDTO> UnscDataList { get; set; }
        public List<OACYPerTransactionDTO> OacyDataList { get; set; }

    }


}