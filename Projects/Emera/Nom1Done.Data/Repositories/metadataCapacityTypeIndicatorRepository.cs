using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nom1Done.DTO;

namespace Nom1Done.Data.Repositories
{
    public class metadataCapacityTypeIndicatorRepository : RepositoryBase<metadataCapacityTypeIndicator>, ImetadataCapacityTypeIndicatorRepository
    {
        public metadataCapacityTypeIndicatorRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public List<CapacityIndicatorDTO> GetCapacityTypes()
        {
            List<CapacityIndicatorDTO> model = new List<CapacityIndicatorDTO>();
            model = DbContext.metadataCapacityTypeIndicator.ToList().Where(a => a.IsActive == true).Select(a => new CapacityIndicatorDTO
            {
                Code = a.Code,
                Name = a.Name
            }).ToList();

            return model;
        }
    }
    public interface ImetadataCapacityTypeIndicatorRepository : IRepository<metadataCapacityTypeIndicator>
    {
        List<CapacityIndicatorDTO> GetCapacityTypes();
    }
}
