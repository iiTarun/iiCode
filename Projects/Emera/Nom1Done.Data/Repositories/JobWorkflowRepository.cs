using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
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
