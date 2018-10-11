using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System.Linq;
using System.Collections.Generic;
using Nom1Done.DTO;

namespace Nom1Done.Data.Repositories
{
    public class metadataBidUpIndicatorRepository : RepositoryBase<metadataBidUpIndicator>,ImetadataBidUpIndicatorRepository
    {
        public metadataBidUpIndicatorRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public List<BidUpIndicatorDTO> GetActiveData()
        {
            List<BidUpIndicatorDTO> model = new List<BidUpIndicatorDTO>();
            model = DbContext.BidUpIndicators.Where(a => a.IsActive == true).Select(a => new BidUpIndicatorDTO
            {
                Code = a.Code,
                Name = a.Name
            }).ToList();
            return model;
        }

    }
    public interface ImetadataBidUpIndicatorRepository : IRepository<metadataBidUpIndicator>
    {
        List<BidUpIndicatorDTO> GetActiveData();
    }
}
