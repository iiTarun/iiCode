using System.Linq;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class JobWorkflowRepository : RepositoryBase<JobWorkflow> , IJobWorkflowRepository
    {
        public JobWorkflowRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public JobWorkflow GetByWorkflowId(long workflowId)
        {
            return this.DbContext.JobWorkflows.Where(a => a.WorkflowId == workflowId).FirstOrDefault();
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IJobWorkflowRepository : IRepository<JobWorkflow>
    {
        void Save();
        JobWorkflow GetByWorkflowId(long workflowId);
    }
}
