using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdSettingRepository : RepositoryBase<Setting>, IUprdSettingRepository
    {
        public UprdSettingRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }
    }

    public interface IUprdSettingRepository : IRepository<Setting>
    {
    }
}
