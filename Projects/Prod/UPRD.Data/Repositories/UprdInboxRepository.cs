using System;
using System.Linq;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdInboxRepository : RepositoryBase<Inbox>,IUprdInboxRepository
    {
        public UprdInboxRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public Inbox GetByTransactionId(Guid MessageId)
        {
            return (from a in this.DbContext.Inboxes
                    where a.MessageID == MessageId
                    select a).FirstOrDefault();
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IUprdInboxRepository:IRepository<Inbox>
    {
        void Save();
        Inbox GetByTransactionId(Guid MessageId);
    }
}
