using System;
using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System.Linq;

namespace Nom1Done.Data.Repositories
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
