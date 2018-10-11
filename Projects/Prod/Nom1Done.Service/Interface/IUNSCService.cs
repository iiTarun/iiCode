using Nom1Done.DTO;
using Nom1Done.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Service.Interface
{
   public interface IUNSCService
    {      
        List<UnscPerTransactionDTO> GetAllUNSCOnPipelineId(Search criteria);
        List<UnscPerTransactionDTO> GetRecentUnsc(Search criteria);
        DateTime? GetRecentUnscPostDate(string PipelineDuns);

    }
}
