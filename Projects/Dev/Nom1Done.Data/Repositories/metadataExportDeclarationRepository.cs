using Nom1Done.Model;
using Nom1Done.Infrastructure;

namespace Nom1Done.Data.Repositories
{
    public class metadataExportDeclarationRepository : RepositoryBase<metadataExportDeclaration>, ImetadataExportDeclarationRepository
    {
        public metadataExportDeclarationRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
    public interface ImetadataExportDeclarationRepository : IRepository<metadataExportDeclaration>
    {

    }
}
