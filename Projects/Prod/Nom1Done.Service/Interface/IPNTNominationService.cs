using Nom.ViewModel;
using Nom1Done.DTO;
using System;
using System.Collections.Generic;

namespace Nom1Done.Service.Interface
{
    public interface IPNTNominationService
    {
        List<Route> GetRoutes();
        int GetTotalCounterPartiesCount(string Keyword, int PipelineID);
        List<LocationsDTO> GetLocationsForSpecialCases(string TransTypeMapId, int PipelineID);
        bool FindIsSpecialLocsUsingTTPipeMapId(int ttPipeMapId);

        TransactionTypesDTO GetTTUsingttMapId(int TTmapid);
        TransactionTypesDTO GetTTUsingTTCodeTTName(string TTIdentifier, string TTName, string PipelineDuns);

        IEnumerable<LocationsDTO> GetLocations(string Keyword,string PipelineDuns, int PageNo, int PageSize, string PopUpFor, bool IsSpecialDelCase,string order, string orderDir);

        int GetTotalLocationCount(string Keyword, string PipelineDuns, string PopUpFor, bool IsSpecialDelCase);

        IEnumerable<TransactionTypesDTO> GetTransactionsTypes(int PipelineId, string Keyword, string popUpfrom);
        IEnumerable<ContractsDTO> GetContracts(string Keyword, int ShipperID, int PipelineID, int PageNo, int PageSize);
        IEnumerable<CounterPartiesDTO> GetCounterParties(string Keyword, int PipelineID, int PageNo, int PageSize,string order,string oderDir);
        BatchDetailDTO GetNominationDetailByBatchID(Guid BatchID);
        //Guid? SaveAndUpdateBatchWithDetail(BatchDetailDTO batchDetail, bool IsSave);
        bool SendNominationTransaction(Guid transactionId, int ShipperCompanyID,bool sendToTest);
        void SaveBulkUpload(BatchDetailDTO batchDetail, bool IsSave);
        bool CopyNomination(Guid TransactionId);
        bool DirectSent(BatchDetailDTO batchDetail,bool sendToTest);
        IEnumerable<RejectionReasonDTO> GetRejectionReason(Guid transactionId);
        Guid? SaveAndUpdatePNTBatchDetail(BatchDetailDTO batchDetail,bool IsSave);
        IEnumerable<BatchDTO> GetBatches(string Keyword, Int32 PipelineID, int statusID, DateTime StartDate, DateTime EndDate, int PageNo, int PageSize, Guid _providerUserKey, string shipperDuns);
        bool ValidateNomination(Guid transactioId, int pipeId);
        Guid UpdatePNTBatch(BatchDetailDTO batch);
        BatchDetailDTO GetNomDetail(Guid batchId, int pipeId) ;

        NomType GetBatchNomType(Guid TransactionId);
        bool CopyNonPathedNomination(Guid TransactionId);
    }
}
