using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdmetadataDatasetRepository : RepositoryBase<metadataDataset>, IUprdmetadataDatasetRepository
    {
        public UprdmetadataDatasetRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
    public interface IUprdmetadataDatasetRepository : IRepository<metadataDataset>
    {

    }
}
