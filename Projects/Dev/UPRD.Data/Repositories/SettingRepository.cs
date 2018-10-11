using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class SettingRepository : RepositoryBase<Setting>, ISettingRepository
    {
        public SettingRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }
    }

    public interface ISettingRepository : IRepository<Setting>
    {
    }
}
