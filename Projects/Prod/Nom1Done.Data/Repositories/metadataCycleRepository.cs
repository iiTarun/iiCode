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
    public class metadataCycleRepository : RepositoryBase<metadataCycle>,ImetadataCycleRepository
    {
        public metadataCycleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public List<CycleIndicatorDTO> GetActiveCycles() {
            List<CycleIndicatorDTO> model = new List<CycleIndicatorDTO>();
            model = DbContext.metadataCycle.Where(a => a.IsActive == true).Select(a => new CycleIndicatorDTO
            {
                CycleID = a.ID,
                Id = a.ID,
                Code = a.Code,
                Name = a.Name
            }).ToList();
            return model;
        }

    }
    public interface ImetadataCycleRepository : IRepository<metadataCycle>
    {
        List<CycleIndicatorDTO> GetActiveCycles();
    }
}
