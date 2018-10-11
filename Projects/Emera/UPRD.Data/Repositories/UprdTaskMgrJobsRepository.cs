using System.Linq;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdTaskMgrJobsRepository : RepositoryBase<TaskMgrJob>,IUprdTaskMgrJobsRepository
    {
        public UprdTaskMgrJobsRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public TaskMgrJob GetByTransactionId(string transactionId)
        {
            return (from a in this.DbContext.TaskMgrJob
                    where a.TransactionId == transactionId
                    select a).FirstOrDefault();
        }

        public TaskMgrJob GetPendingJobReceive(int status, int stage)
        {
            return this.DbContext.TaskMgrJob.Where(a => a.StageId == stage && a.StatusId == status && !a.IsSending).FirstOrDefault();
        }

        public TaskMgrJob GetPendingJobSend(int status, int stage)
        {
            return this.DbContext.TaskMgrJob.Where(a => a.StageId == stage && a.StatusId == status && a.IsSending).FirstOrDefault();
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }
    public interface IUprdTaskMgrJobsRepository:IRepository<TaskMgrJob>
    {
        void Save();
        TaskMgrJob GetByTransactionId(string transactionId);
        TaskMgrJob GetPendingJobReceive(int status,int stage);
        TaskMgrJob GetPendingJobSend(int status, int stage);
    }
}
