using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class EmailQueueRepository:RepositoryBase<EmailQueue>,IEmailQueueRepository
    {
        public EmailQueueRepository(IDbFactory dbfactory):base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IEmailQueueRepository : IRepository<EmailQueue>
    {
        void Save();
    }
}
