using System.Linq;
using System;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdOutboxRepository : RepositoryBase<Outbox>, IUprdOutboxRepository
    {
        public UprdOutboxRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public Outbox GetByTransactionId(Guid MessageId)
        {
            return (from a in this.DbContext.Outbox
                    where a.MessageID == MessageId
                    select a).FirstOrDefault();
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }
    public interface IUprdOutboxRepository : IRepository<Outbox>
    {
        void Save();
        Outbox GetByTransactionId(Guid MessageId);
    }
}
