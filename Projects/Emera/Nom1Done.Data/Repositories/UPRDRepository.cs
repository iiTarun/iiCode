using Nom1Done.Model;
using Nom1Done.Infrastructure;

namespace Nom1Done.Data.Repositories
{
    public class UPRDRepository : RepositoryBase<UPRD>, IUPRDRepository
    {
        public UPRDRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IUPRDRepository : IRepository<UPRD>
    {
        void Save();

    }
}
