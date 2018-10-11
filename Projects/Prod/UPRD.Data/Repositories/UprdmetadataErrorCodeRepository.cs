using UPRD.Model;
using UPRD.Infrastructure;

namespace Nom1Done.Data.Repositories
{
    public class UprdmetadataErrorCodeRepository : RepositoryBase<metadataErrorCode>, IUprdmetadataErrorCodeRepository
    {
        public UprdmetadataErrorCodeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }
    public interface IUprdmetadataErrorCodeRepository : IRepository<metadataErrorCode>
    {
        void Save();
    }
}
