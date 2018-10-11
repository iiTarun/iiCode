using UPRD.Model;
using UPRD.Infrastructure;

namespace UPRD.Data.Repositories
{
    public class GISBInboxRepository : RepositoryBase<GISBInbox>, IGISBInboxRepository
    {
        public GISBInboxRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IGISBInboxRepository : IRepository<GISBInbox>
    {
        void Save();
    }
}
