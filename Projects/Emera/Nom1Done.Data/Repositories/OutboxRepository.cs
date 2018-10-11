using System.Linq;
using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;

namespace Nom1Done.Data.Repositories
{
    public class OutboxRepository : RepositoryBase<Outbox>, IOutboxRepository
    {
        public OutboxRepository(IDbFactory dbFactory) : base(dbFactory)
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
    public interface IOutboxRepository : IRepository<Outbox>
    {
        void Save();
        Outbox GetByTransactionId(Guid MessageId);
    }
}
