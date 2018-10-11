using Nom1Done.Model;
using Nom1Done.Infrastructure;

namespace Nom1Done.Data.Repositories
{
    public class UserRepository : RepositoryBase<Shipper>, IUserRepository
    {
        public UserRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }
    }

    public interface IUserRepository : IRepository<Shipper>
    {

    }
}
