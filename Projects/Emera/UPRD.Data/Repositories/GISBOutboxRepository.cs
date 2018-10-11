using UPRD.Model;
using UPRD.Infrastructure;

namespace UPRD.Data.Repositories
{
    public class GISBOutboxRepository : RepositoryBase<GISBOutbox>, IGISBOutboxRepository
    {
        public GISBOutboxRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IGISBOutboxRepository : IRepository<GISBOutbox>
    {
        void Save();
    }
}
