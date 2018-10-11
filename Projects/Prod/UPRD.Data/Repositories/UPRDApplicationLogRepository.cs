using UPRD.Model;
using UPRD.Infrastructure;

namespace UPRD.Data.Repositories
{
    public class UPRDApplicationLogRepository : RepositoryBase<ApplicationLog>,IUPRDApplicationLogRepository
    {
        public UPRDApplicationLogRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }
    public interface IUPRDApplicationLogRepository : IRepository<ApplicationLog>
    {
        void Save();
    }
}
