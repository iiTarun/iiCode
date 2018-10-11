using Nom1Done.DTO;
using Nom1Done.Data.Repositories;
using Nom1Done.Service.Interface;
using System;

namespace Nom1Done.Service
{
    public class PipelineEDISettingService : IPipelineEDISettingService
    {
        IPipelineEDISettingRepository _IPipelineEDISettingRepository;
        IModalFactory _IModalFactory;
        public PipelineEDISettingService(IPipelineEDISettingRepository IPipelineEDISettingRepository, IModalFactory IModalFactory)
        {
            this._IPipelineEDISettingRepository = IPipelineEDISettingRepository;
            this._IModalFactory = IModalFactory;
        }
        public PipelineEDISettingDTO GetPipelineSetting(int DatasetId, string pipeDuns, string shipperDuns)
        {
            return _IModalFactory.Parse(_IPipelineEDISettingRepository.GetPipelineSetting(pipeDuns, DatasetId, shipperDuns));
        }

        public bool UpdatePipelineEDISetting(PipelineEDISettingDTO pipelineEDISettingDTO)
        {
            try
            {
                //var pipeEDISet = _IPipelineEDISettingRepository.GetPipelineSetting(pipelineEDISettingDTO.p.pipeDuns,customUPRDReqDTO.datasetID,customUPRDReqDTO.sh);
                var set=_IPipelineEDISettingRepository.GetById(pipelineEDISettingDTO.id);
                if (set != null)
                {
                    set.StartDate = pipelineEDISettingDTO.StartDate;
                    set.EndDate = pipelineEDISettingDTO.EndDate;
                    set.ForOacy = pipelineEDISettingDTO.ForOacy;
                    set.ForUnsc = pipelineEDISettingDTO.ForUnsc;
                    set.ForSwnt = pipelineEDISettingDTO.ForSwnt;
                    set.SendManually = pipelineEDISettingDTO.SendManually;
                    _IPipelineEDISettingRepository.Update(set);
                    _IPipelineEDISettingRepository.SaveChanges();
                }
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }
    }
}
