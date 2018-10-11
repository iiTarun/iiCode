using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
    public class InboxRepository : RepositoryBase<Inbox>,IInboxRepository
    {
        public InboxRepository(IDbFactory dbfactory) : base(dbfactory)
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

    public interface IInboxRepository:IRepository<Inbox>
    {
        void Save();
        Inbox GetByTransactionId(Guid MessageId);
    }
}
