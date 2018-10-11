using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nom.ViewModel;
using Nom1Done.Data.Repositories;

namespace Nom1Done.Service
{
    public class PipelineStatusService : IPipelineStatusService
    {
        IPipelineStatusRepository _IPipelineStatusRepository;
        public PipelineStatusService(IPipelineStatusRepository IPipelineStatusRepository) {

            _IPipelineStatusRepository = IPipelineStatusRepository;
        }
        public IEnumerable<PipelineStatusDTO> GetPipelineStatus(int pipelineId)
        {
            return _IPipelineStatusRepository.GetPipelineStatus(pipelineId);
        }
    }
}
