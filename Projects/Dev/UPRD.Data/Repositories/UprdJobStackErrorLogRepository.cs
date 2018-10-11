using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdJobStackErrorLogRepository : RepositoryBase<JobStackErrorLog>, IUprdJobStackErrorLogRepository
    {
        public UprdJobStackErrorLogRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IUprdJobStackErrorLogRepository : IRepository<JobStackErrorLog>
    {
        void Save();
    }
}
