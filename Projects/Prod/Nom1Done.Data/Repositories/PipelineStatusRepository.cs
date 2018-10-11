using Nom.ViewModel;
using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
   public class PipelineStatusRepository : RepositoryBase<PipelineStatusDTO>, IPipelineStatusRepository
    {
        IDbFactory _dbfactory;
        NomEntities nomEntity;
        public PipelineStatusRepository(IDbFactory dbfactory) : base(dbfactory)
        {
            _dbfactory = dbfactory;
            nomEntity = _dbfactory.Init();
        }

        public IEnumerable<PipelineStatusDTO> GetPipelineStatus(int pipelineId)
        {
            List<PipelineStatusDTO> pipeStatusList = new List<PipelineStatusDTO>();
            try
            {
                DateTime dateTo = DateTime.Now.Date.AddDays(-30);
              //List<TPWBrowserTestResult> browserTestResultList = new List<TPWBrowserTestResult>();               
              //browserTestResultList = nomEntity.TPWBrowserTestResult.Where(a => a.PipelineID == pipelineId && a.CreatedDate >= dateTo).ToList();

                do
                {
                    bool isOacyRec = false;
                    bool isUnscRec = false;
                    bool isSwntRec = false;
                    using (var ctx = new NomEntities())
                    {
                        isOacyRec = ctx.OACYPerTransaction.Any(a => a.PipelineID == pipelineId && DbFunctions.TruncateTime(a.CreatedDate) == dateTo.Date);
                        isUnscRec = ctx.UnscPerTransaction.Any(a => a.PipelineID == pipelineId && DbFunctions.TruncateTime(a.CreatedDate) == dateTo.Date);
                        isSwntRec = ctx.SwntPerTransaction.Any(a => a.PipelineId == pipelineId && DbFunctions.TruncateTime(a.CreatedDate) == dateTo.Date);
                    }
                    PipelineStatusDTO pipeStatus = new PipelineStatusDTO();
                    pipeStatus.PipelineId = pipelineId;
                    pipeStatus.onDate = dateTo;
                    pipeStatus.IsBrowserTestSuccess = false;
                    pipeStatus.IsOacyReceive = isOacyRec;
                    pipeStatus.IsUnscReceive = isUnscRec;
                    pipeStatus.IsNoticeReceive = isSwntRec;
                    pipeStatusList.Add(pipeStatus);
                    dateTo = dateTo.Date.AddDays(1);
                } while (DateTime.Now.Date > dateTo);

              //foreach (var browserTest in browserTestResultList)
              //  {
              //      bool isOacyRec = false;
              //      bool isUnscRec = false;
              //      bool isSwntRec = false;
              //      using (var ctx = nomEntity)
              //      {
              //          isOacyRec = ctx.OACYPerTransaction.Any(a => a.PipelineID == browserTest.PipelineID && DbFunctions.TruncateTime(a.CreatedDate) == browserTest.CreatedDate.Date);
              //          isUnscRec = ctx.UnscPerTransaction.Any(a => a.PipelineID == browserTest.PipelineID && DbFunctions.TruncateTime(a.CreatedDate) == browserTest.CreatedDate.Date);
              //          isSwntRec = ctx.SwntPerTransaction.Any(a => a.PipelineId == browserTest.PipelineID && DbFunctions.TruncateTime(a.CreatedDate) == browserTest.CreatedDate.Date);
              //      }
              //      PipelineStatusDTO pipeStatus = new PipelineStatusDTO();
              //      pipeStatus.PipelineId = browserTest.PipelineID;
              //      pipeStatus.onDate = browserTest.CreatedDate;
              //      pipeStatus.IsBrowserTestSuccess = browserTest.IsWorking.HasValue ? browserTest.IsWorking.Value : false;
              //      pipeStatus.IsOacyReceive = isOacyRec;
              //      pipeStatus.IsUnscReceive = isUnscRec;
              //      pipeStatus.IsNoticeReceive = isSwntRec;
              //      pipeStatusList.Add(pipeStatus);
              //  }
            }
            catch (Exception ex)
            {

            }

            return pipeStatusList.OrderByDescending(a => a.onDate).ToList();
        }
    }

    public interface IPipelineStatusRepository : IRepository<PipelineStatusDTO>
    {
        IEnumerable<PipelineStatusDTO> GetPipelineStatus(int pipelineId);
    }
}
