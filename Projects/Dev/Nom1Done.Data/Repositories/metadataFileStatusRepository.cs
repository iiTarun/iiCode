using Nom1Done.Model;
using Nom1Done.Infrastructure;

namespace Nom1Done.Data.Repositories
{
    public class metadataFileStatusRepository : RepositoryBase<metadataFileStatu>,ImetadataFileStatusRepositpry
    {
        public metadataFileStatusRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
    public interface ImetadataFileStatusRepositpry : IRepository<metadataFileStatu>
    {

    }
}
