using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdEmailQueueRepository:RepositoryBase<EmailQueue>,IUprdEmailQueueRepository
    {
        public UprdEmailQueueRepository(IDbFactory dbfactory):base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IUprdEmailQueueRepository : IRepository<EmailQueue>
    {
        void Save();
    }
}
