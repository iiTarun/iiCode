
using System.Collections.Generic;
using UPRD.DTO;
using UPRD.Enums;

namespace UPRD.Services.Interface
{
    public interface IUprdPipelineService
    {
        //PipelineDTO GetPipeline(int PipeLineId);
        IEnumerable<PipelineDTO> GetAllPipelineList(int companyID, string userId);
        //string GetDunsByPipelineID(int ID);
        //NomType GetPathTypeByPipelineDuns(string pipelineDuns);
        //bool UpdatePipeline(PipelineDTO pipe);
        //PipelineDTO GetPipelineByDunsNo(string dunsNo);
        //int GetTranTypeMapId(string PipeDuns, string TranTypeIden, string TranTypeDesc);

        //PipelineDTO GetFirstPipelineByUser(string UserId, int companyId);
        IEnumerable<PipelineDTO> GetAllActivePipeline();


        //IEnumerable<PipelineDTO> GetPipelinesByUser(PipelineByUserDTO pipelineByUser);
        //List<PipelineDTO> GetPipelinesByUser();
        IEnumerable<PipelineDTO> GetPipelinesByUser(PipelineByUserDTO pipelineByUser);
    }
}
