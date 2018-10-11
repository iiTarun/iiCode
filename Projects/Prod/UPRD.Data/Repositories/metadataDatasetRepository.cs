using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
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
