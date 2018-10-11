using Nom.ViewModel;
using Nom1Done.DTO;
using System;
using System.Collections.Generic;

namespace Nom1Done.Service.Interface
{
    public interface IPNTNominationService
    {
        List<Route> GetRoutes();
        
        bool FindIsSpecialLocsUsingTTPipeMapId(int ttPipeMapId);

        TransactionTypesDTO GetTTUsingttMapId(int TTmapid);
        TransactionTypesDTO GetTTUsingTTCodeTTName(string TTIdentifier, string TTName, string PipelineDuns);

    
        int GetTotalLocationCount(string Keyword, string PipelineDuns, string PopUpFor, bool IsSpecialDelCase);

        IEnumerable<TransactionTypesDTO> GetTransactionsTypes(string pipeLineDuns, string Keyword, string popUpfrom);
        IEnumerable<ContractsDTO> GetContracts( int ShipperID, string PipelineDuns);
        BatchDetailDTO GetNominationDetailByBatchID(Guid BatchID);
      
        bool SendNominationTransaction(Guid transactionId, int ShipperCompanyID,bool sendToTest);
        void SaveBulkUpload(BatchDetailDTO batchDetail, bool IsSave);
        bool CopyNomination(Guid TransactionId);
        bool DirectSent(BatchDetailDTO batchDetail,bool sendToTest);
        IEnumerable<RejectionReasonDTO> GetRejectionReason(Guid transactionId);
        Guid? SaveAndUpdatePNTBatchDetail(BatchDetailDTO batchDetail,bool IsSave);
        IEnumerable<BatchDTO> GetBatches(string Keyword, string PipelineDuns, int statusID, DateTime StartDate, DateTime EndDate, int PageNo, int PageSize, Guid _providerUserKey, string shipperDuns);
        bool ValidateNomination(Guid transactioId, string pipeDuns);
        Guid UpdatePNTBatch(BatchDetailDTO batch);
        BatchDetailDTO GetNomDetail(Guid batchId, string pipeDuns) ;

        NomType GetBatchNomType(Guid TransactionId);
    }
}
