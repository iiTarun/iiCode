using System.Linq;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdTransactionLogRepository : RepositoryBase<TransactionLog>, IUprdTransactionLogRepository
    {
        public UprdTransactionLogRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public TransactionLog GetByTransactionId(string transactionId)
        {
            return this.DbContext.TransactionLog.Where(a => a.TransactionId == transactionId).FirstOrDefault();
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }
    public interface IUprdTransactionLogRepository : IRepository<TransactionLog>
    {
        void Save();
        TransactionLog GetByTransactionId(string transactionId);
    }
}
