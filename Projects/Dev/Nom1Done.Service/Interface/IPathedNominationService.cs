using Nom.ViewModel;
using Nom1Done.Model;
using System;
using System.Collections.Generic;

namespace Nom1Done.Service.Interface
{
    public interface IPathedNominationService
    {
        Guid? SaveAndUpdatePathedNomination(PathedDTO pathedNom, bool IsSave);
        List<PathedNomDetailsDTO> GetPathedList(string pipelineDuns, int Status, DateTime StartDate, DateTime EndDate, string shipperDuns, Guid loginUser);
        Pipeline GetPipeline(int PipeLineId);
       // bool CopyNomination(Guid TransactionId);
        bool DeleteNominationData(Guid transactionId);

        int GetPathedListTotalCount(string pipelineDuns, int Status, DateTime StartDate, DateTime EndDate, string shipperDuns, Guid loginUser);

        List<PathedNomDetailsDTO> GetPathedListWithPaging(string pipelineDuns, int Status, DateTime StartDate, DateTime EndDate, string shipperDuns, Guid loginUser, SortingPagingInfo sortingPagingInfo);
        int GetStatusOnTransactionId(Guid tranId);
    }
}
