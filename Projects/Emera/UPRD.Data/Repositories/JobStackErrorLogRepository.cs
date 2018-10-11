using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class JobStackErrorLogRepository : RepositoryBase<JobStackErrorLog>, IJobStackErrorLogRepository
    {
        public JobStackErrorLogRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IJobStackErrorLogRepository : IRepository<JobStackErrorLog>
    {
        void Save();
    }
}
