using Nom1Done.Model;
using Nom1Done.Infrastructure;

namespace Nom1Done.Data.Repositories
{
    public class metadataDatasetRepository : RepositoryBase<metadataDataset>, ImetadataDatasetRepository
    {
        public metadataDatasetRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
    public interface ImetadataDatasetRepository : IRepository<metadataDataset>
    {

    }
}
