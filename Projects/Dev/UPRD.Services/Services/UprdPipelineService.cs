
using System.Collections.Generic;
using System.Linq;
using UPRD.Data.Repositories;
using UPRD.DTO;
using UPRD.Enums;
using UPRD.Model;
using UPRD.Service.Interface;
using UPRD.Services.Interface;

namespace UPRD.Services.Interface
{
    public class UprdPipelineService : IUprdPipelineService
    {
        IUprdPipelineRepository _IPipelineRepository;
        IModalFactory modalFactory;
        public UprdPipelineService(IUprdPipelineRepository IPipelineRepository, IModalFactory modalFactory)
        {
            this._IPipelineRepository = IPipelineRepository;
            this.modalFactory = modalFactory;
        }

        public IEnumerable<PipelineDTO> GetAllActivePipeline()
        {
            var list = _IPipelineRepository.GetAllActivePipeline().ToList();
            //return PipelineStatusDTO(list);
            return list;
        }

        //public List<PipelineDTO> GetPipelinesByUser()
        //{
        //    var pipelines = _IPipelineRepository.GetPipelinesByUser();
        //                TempItem = b.DUNSNo + "-" + b.ModelTypeID,
        //                     IsNoms = a.IsNoms,
        //                     IsUPRD = a.IsUPRD
        //    //return PipelineStatusDTO(pipelines);
        //}
        public IEnumerable<PipelineDTO> GetPipelinesByUser(PipelineByUserDTO pipelineByUser)
        {
            var list = _IPipelineRepository.GetPipelinesByUserDto(pipelineByUser);
            return list;
        }
        private List<PipelineDTO> PipelineStatusDTO(List<Pipeline> list)
        {
            List<PipelineDTO> CounterStatusList = new List<PipelineDTO>();
            if (list != null && list.Count > 0)
                foreach (var item in list)
                {
                    CounterStatusList.Add(modalFactory.Parse(item));
                }
            return CounterStatusList;
        }

        public IEnumerable<PipelineDTO> GetAllPipelineList(int companyID, string userId)
        {
            return _IPipelineRepository.GetActivePipelineList(companyID, userId).Select(c => modalFactory.Parse(c));
        }



        //public string GetDunsByPipelineID(int ID)
        //{
        //    return _IPipelineRepository.GetById(ID).DUNSNo;
        //}

        //public PipelineDTO GetPipeline(int PipeLineId)
        //{
        //    return modalFactory.Parse(_IPipelineRepository.GetById(PipeLineId));
        //}

        //public UPRD.DTO.NomType GetPathTypeByPipelineDuns(string pipelineDuns)
        //{
        //    return _IPipelineRepository.GetPathTypeByPipelineDuns(pipelineDuns);
        //}

        //public bool UpdatePipeline(PipelineDTO pipeDTO)
        //{
        //    var pipe = _IPipelineRepository.GetById(pipeDTO.ID);
        //    if (pipe != null)
        //    {
        //        pipe.IsUprdActive = pipeDTO.IsUprdActive;
        //        pipe.ModelTypeID = pipeDTO.ModelTypeID;
        //        _IPipelineRepository.Update(pipe);
        //        _IPipelineRepository.SaveChanges();
        //        return true;
        //    }
        //    else
        //        return false;
        //}

        //public PipelineDTO GetPipelineByDunsNo(string dunsNo)
        //{
        //    return modalFactory.Parse(_IPipelineRepository.GetPipelineByDuns(dunsNo));
        //}

        //public int GetTranTypeMapId(string PipeDuns, string TranTypeIden, string TranTypeDesc)
        //{
        //    return _IPipelineRepository.GetTranTypeMapId(PipeDuns, TranTypeIden, TranTypeDesc);
        //}

        //public PipelineDTO GetFirstPipelineByUser(string UserId, int companyId)
        //{
        //    return modalFactory.Parse(_IPipelineRepository.GetSelectedPipelineByUser(UserId, companyId));
        //}
    }
}
