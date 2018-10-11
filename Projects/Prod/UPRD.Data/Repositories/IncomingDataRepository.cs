using System.Linq;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class IncomingDataRepository : RepositoryBase<IncomingData>, IIncomingDataRepository
    {
        public IncomingDataRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public IncomingData GetByTransactionId(string transactionId)
        {
            return this.DbContext.IncomingDatas.Where(a => a.MessageId == transactionId).FirstOrDefault();
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IIncomingDataRepository : IRepository<IncomingData>
    {
        void Save();
        IncomingData GetByTransactionId(string transactionId);
    }
}
