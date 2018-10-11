using System.Linq;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class TransactionLogRepository : RepositoryBase<TransactionLog>, ITransactionLogRepository
    {
        public TransactionLogRepository(IDbFactory dbFactory) : base(dbFactory)
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
    public interface ITransactionLogRepository : IRepository<TransactionLog>
    {
        void Save();
        TransactionLog GetByTransactionId(string transactionId);
    }
}
