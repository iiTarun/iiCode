using Nom.ViewModel;
using Nom1Done.DTO;
using Nom1Done.Enums;
using Nom1Done.Model;
using System.Collections.Generic;

namespace Nom1Done.Service.Interface
{
    public interface IPipelineService
    {
        PipelineDTO GetPipeline(int PipeLineId);
        IEnumerable<PipelineDTO> GetAllPipelineList(int companyID, string userId);
        string GetDunsByPipelineID(int ID);
        NomType GetPathTypeByPipelineDuns(string pipelineDuns);
        bool UpdatePipeline(PipelineDTO pipe);
        PipelineDTO GetPipelineByDunsNo(string dunsNo);
        int GetTranTypeMapId(string PipeDuns, string TranTypeIden, string TranTypeDesc);

        PipelineDTO GetFirstPipelineByUser(string UserId,int companyId);
    }
}
