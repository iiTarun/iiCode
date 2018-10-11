using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class Outbox_MultipartFormRepository: RepositoryBase<Outbox_MultipartForm>, IOutbox_MultipartFormRepository
    {
        public Outbox_MultipartFormRepository(IDbFactory dbfactory):base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IOutbox_MultipartFormRepository:IRepository<Outbox_MultipartForm>
    {
        void Save();
    }
}
