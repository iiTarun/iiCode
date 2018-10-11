using Nom1Done.DTO;
using Nom1Done.Nom.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Service.Interface
{
   public interface ISWNTPerTransactionService
    {
      //  IEnumerable<BONotice> GetAll();
        List<BONotice> SearchNotices(int pipelineId, BONoticeSearchCriteria criteria);
        BONotice GetNoticeOnId(int id);

        List<BONotice> Search(int PipelineID, BONoticeSearchCriteria searchCriteria);

        Int32 SearchCount(int PipelineID, BONoticeSearchCriteria searchCriteria);

        BOPipeline GetItem(Int32 id);

    }
}
