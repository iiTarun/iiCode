using Nom1Done.Model;
using Nom1Done.Infrastructure;

namespace Nom1Done.Data.Repositories
{
    public class metadataQuantityTypeIndicatorRepository : RepositoryBase<metadataQuantityTypeIndicator>,ImetadataQuantityTypeRepository
    {
        public metadataQuantityTypeIndicatorRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
    public interface ImetadataQuantityTypeRepository : IRepository<metadataQuantityTypeIndicator>
    {

    }
}
