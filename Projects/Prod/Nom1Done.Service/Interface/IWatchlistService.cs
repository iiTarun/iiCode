using Nom.ViewModel;
using Nom1Done.DTO;
using Nom1Done.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Service.Interface
{
   public interface IWatchlistService
    {
        List<SwntPerTransactionDTO> ExecuteWatchListSWNTOnScreen(List<WatchListRule> RuleList, Type type);
        List<UnscPerTransactionDTO> ExecuteWatchListUNSConScreen(List<WatchListRule> RuleList, Type type);

        List<OACYPerTransactionDTO> ExecuteWatchListOACYonScreen(List<WatchListRule> RuleList, Type type);
        bool ExecuteWatchList(WatchlistAlertFrequency alertType, EnercrossDataSets dataSet);

        bool DeleteWatchListById(int watchListid);
        List<WatchListDTO> GetWatchListByUserId(string userId);
        WatchListDTO GetWatchListById(int watchListId);
        WatchListProperty GetPropertyById(int propertyId);
        List<WatchListDTO> GetWatchList(int pipelineId, string userId, EnercrossDataSets dataset);
        int SaveWatchList(WatchListDTO watchList);
        bool UpdateWatchList(WatchListDTO watchList);
        List<SwntPerTransactionDTO> ExecuteWatchListWithNotices(int pipelineId,WatchListDTO watchListDto, Type type);
        List<WatchListOperator> GetOperatorByDataType(int dataTypeId);
        List<WatchListProperty> GetPropertiesByDataSet(EnercrossDataSets dataset);
        List<OACYPerTransactionDTO> ExecuteWatchListWithOACY(int pipelineId,WatchListDTO watchListDto, Type type);
        List<UnscPerTransactionDTO> ExecuteWatchListWithUNSC(int pipelineId,WatchListDTO watchListDto, Type type);      

        List<OACYPerTransactionDTO> ExecuteWatchListOACY(List<WatchListRule> RuleList, Type type);
        List<SwntPerTransactionDTO> ExecuteWatchListSWNT(List<WatchListRule> RuleList, Type type);
        List<UnscPerTransactionDTO> ExecuteWatchListUNSC(List<WatchListRule> RuleList, Type type);

        List<LocationsDTO> GetLocationsFromUPRDs(string PipelineDuns, EnercrossDataSets datasetType);

    }
}
