using Nom1Done.DTO;
using Nom1Done.Model;
using System.Collections.Generic;

namespace Nom1Done.Service
{
    public interface IDashboardService
    {
        IEnumerable<Pipeline> GetAllPipelines();
        IEnumerable<RejectedNomModel> GetRejectedNomination(int pipelineId,string userId);
        BOShipperCompany GetItemByUser(string UserID);
        BOShipperCompany GetShipperByUser(string UserId);
        List<RejectionReasonDTO> GetRejectionReason(string nomId);
        Pipeline GetFirstPipelineByUser(string UserId,int companyId);
    }
}