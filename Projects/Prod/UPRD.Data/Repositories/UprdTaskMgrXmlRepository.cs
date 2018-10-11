using System.Linq;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdTaskMgrXmlRepository : RepositoryBase<TaskMgrXml>, IUprdTaskMgrXmlRepository
    {
        public UprdTaskMgrXmlRepository(IDbFactory dbfactory) : base(dbfactory)
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

    public interface IUprdTaskMgrXmlRepository : IRepository<TaskMgrXml>
    {
        void Save();
        TaskMgrXml GetbyTransactionId(string TransactionId);
    }
}
