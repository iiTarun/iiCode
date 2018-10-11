using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System.Linq;

namespace Nom1Done.Data.Repositories
{
    public class TaskMgrXmlRepository : RepositoryBase<TaskMgrXml>, ITaskMgrXmlRepository
    {
        public TaskMgrXmlRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public TaskMgrXml GetbyTransactionId(string TransactionId)
        {
            return (from a in this.DbContext.TaskMgrXmls
                    where a.TransactionId == TransactionId
                    select a).FirstOrDefault();
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface ITaskMgrXmlRepository : IRepository<TaskMgrXml>
    {
        void Save();
        TaskMgrXml GetbyTransactionId(string TransactionId);
    }
}
