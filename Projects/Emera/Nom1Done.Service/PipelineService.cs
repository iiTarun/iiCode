using Nom1Done.Service.Interface;
using System.Collections.Generic;
using System.Linq;
using Nom1Done.Data.Repositories;
using Nom.ViewModel;
using Nom1Done.Enums;
using Nom1Done.DTO;
using Nom1Done.Model;

namespace Nom1Done.Service
{
    public class PipelineService : IPipelineService
    {
        IPipelineRepository _IPipelineRepository;
        IModalFactory modalFactory;
        public PipelineService(IPipelineRepository IPipelineRepository, IModalFactory modalFactory)
        {
            this._IPipelineRepository = IPipelineRepository;
            this.modalFactory = modalFactory;
        }

        public IEnumerable<PipelineDTO> GetAllPipelineList(int companyID, string userId)
        {   
            return _IPipelineRepository.GetActivePipelineList(companyID,userId).Select(c => modalFactory.Parse(c));
        }

        public string GetDunsByPipelineID(int ID)
        {
            return _IPipelineRepository.GetById(ID).DUNSNo;
        }

        public PipelineDTO GetPipeline(int PipeLineId)
        {
            return modalFactory.Parse(_IPipelineRepository.GetById(PipeLineId));
        }

        public NomType GetPathTypeByPipelineDuns(string pipelineDuns)
        {
            return _IPipelineRepository.GetPathTypeByPipelineDuns(pipelineDuns);
        }

        public bool UpdatePipeline(PipelineDTO pipeDTO)
        {
            var pipe = _IPipelineRepository.GetById(pipeDTO.ID);
            if (pipe != null)
            {
                pipe.IsUprdActive = pipeDTO.IsUprdActive;
                pipe.ModelTypeID = pipeDTO.ModelTypeID;
                _IPipelineRepository.Update(pipe);
                _IPipelineRepository.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public PipelineDTO GetPipelineByDunsNo(string dunsNo)
        {
            return modalFactory.Parse(_IPipelineRepository.GetPipelineByDuns(dunsNo));
        }

        public int GetTranTypeMapId(string PipeDuns, string TranTypeIden, string TranTypeDesc)
        {
            return _IPipelineRepository.GetTranTypeMapId(PipeDuns, TranTypeIden, TranTypeDesc);
        }

        public PipelineDTO GetFirstPipelineByUser(string UserId,int companyId)
        {
            return modalFactory.Parse(_IPipelineRepository.GetSelectedPipelineByUser(UserId, companyId));
        }

    }
}
