using Nom1Done.DTO;
using Nom1Done.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Service.Interface
{
   public interface IOACYService
    {
        //IEnumerable<OACYPerTransaction> GetAll();
        List<OACYPerTransactionDTO> GetAllOacyOnPipelineId(Search criteria);

        bool AddData(List<OACYPerTransaction> list);

        List<OACYPerTransactionDTO> GetRecentOacy(Search criteria);
        DateTime? GetRecentOacyPostDate(string PipelineDuns);
    }
}
