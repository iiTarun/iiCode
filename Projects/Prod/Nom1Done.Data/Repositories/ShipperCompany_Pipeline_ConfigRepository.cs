using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
   public class ShipperCompany_Pipeline_ConfigRepository : RepositoryBase<ShipperCompany_Pipeline_Config>, IShipperCompany_Pipeline_ConfigRepository
    {
        public ShipperCompany_Pipeline_ConfigRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }
    }

    public interface IShipperCompany_Pipeline_ConfigRepository : IRepository<ShipperCompany_Pipeline_Config>
    {
    }
}
