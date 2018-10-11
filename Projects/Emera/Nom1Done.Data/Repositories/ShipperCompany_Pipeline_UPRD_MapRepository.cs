using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
  public  class ShipperCompany_Pipeline_UPRD_MapRepository : RepositoryBase<ShipperCompany_Pipeline_UPRD_Map>, IShipperCompany_Pipeline_UPRD_MapRepository
    {
        public ShipperCompany_Pipeline_UPRD_MapRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }
    }

    public interface IShipperCompany_Pipeline_UPRD_MapRepository : IRepository<ShipperCompany_Pipeline_UPRD_Map>
    {
    }
}
