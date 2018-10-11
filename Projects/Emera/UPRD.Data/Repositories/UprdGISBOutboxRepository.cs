using UPRD.Model;
using UPRD.Infrastructure;

namespace UPRD.Data.Repositories
{
    public class UprdGISBOutboxRepository : RepositoryBase<GISBOutbox>, IUprdGISBOutboxRepository
    {
        public UprdGISBOutboxRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IUprdGISBOutboxRepository : IRepository<GISBOutbox>
    {
        void Save();
    }
}
