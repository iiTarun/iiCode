using Nom.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Service.Interface
{
    public interface IPipelineStatusService
    {
        IEnumerable<PipelineStatusDTO> GetPipelineStatus(int pipelineId);
    }
}
