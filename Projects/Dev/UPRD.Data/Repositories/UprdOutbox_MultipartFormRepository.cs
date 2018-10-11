using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdOutbox_MultipartFormRepository: RepositoryBase<Outbox_MultipartForm>, IUprdOutbox_MultipartFormRepository
    {
        public UprdOutbox_MultipartFormRepository(IDbFactory dbfactory):base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IUprdOutbox_MultipartFormRepository:IRepository<Outbox_MultipartForm>
    {
        void Save();
    }
}
