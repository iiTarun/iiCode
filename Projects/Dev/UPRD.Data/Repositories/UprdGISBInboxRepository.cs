using UPRD.Model;
using UPRD.Infrastructure;

namespace UPRD.Data.Repositories
{
    public class UprdGISBInboxRepository : RepositoryBase<GISBInbox>, IUprdGISBInboxRepository
    {
        public UprdGISBInboxRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IUprdGISBInboxRepository : IRepository<GISBInbox>
    {
        void Save();
    }
}
