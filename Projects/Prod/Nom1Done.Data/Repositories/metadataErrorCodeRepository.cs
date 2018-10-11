using Nom1Done.Model;
using Nom1Done.Infrastructure;

namespace Nom1Done.Data.Repositories
{
    public class metadataErrorCodeRepository : RepositoryBase<metadataErrorCode>, ImetadataErrorCodeRepository
    {
        public metadataErrorCodeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }
    public interface ImetadataErrorCodeRepository : IRepository<metadataErrorCode>
    {
        void Save();
    }
}
