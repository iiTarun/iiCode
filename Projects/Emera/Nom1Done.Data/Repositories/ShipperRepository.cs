using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System.Linq;

namespace Nom1Done.Data.Repositories
{
    public class ShipperRepository : RepositoryBase<Shipper>,IShipperRepository
    {
        public ShipperRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public Shipper GetbyUserId(string userId)
        {
            return this.DbContext.Shipper.Where(a => a.UserId == userId).FirstOrDefault();
        }
    }

    public interface IShipperRepository : IRepository<Shipper>
    {
        Shipper GetbyUserId(string userId);
    }
}
