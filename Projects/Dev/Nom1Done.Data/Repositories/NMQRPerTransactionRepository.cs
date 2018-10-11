using System;
using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System.Linq;

namespace Nom1Done.Data.Repositories
{
    public class NMQRPerTransactionRepository : RepositoryBase<NMQRPerTransaction>, INMQRPerTransactionRepository
    {
        public NMQRPerTransactionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IQueryable<NMQRPerTransaction> GetByTransactionId(Guid Transactionid) {
            return DbContext.NMQRPerTransactions.Where(a => a.Transactionid == Transactionid).OrderBy(a => a.ValidationCode);
        }

        public IQueryable<NMQRPerTransaction> GetNmqrOnNmqrTranId(Guid NmqrId)
        {
            return this.DbContext.NMQRPerTransactions.Where(a => a.Transactionid == NmqrId);
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }
    public interface INMQRPerTransactionRepository:IRepository<NMQRPerTransaction>
    {
        void Save();

        IQueryable<NMQRPerTransaction> GetByTransactionId(Guid Transactionid);
        IQueryable<NMQRPerTransaction> GetNmqrOnNmqrTranId(Guid NmqrId);
    }
}
