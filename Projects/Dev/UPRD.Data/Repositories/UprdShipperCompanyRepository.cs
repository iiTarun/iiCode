using System.Linq;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdShipperCompanyRepository : RepositoryBase<ShipperCompany>, IUprdShipperCompanyRepository
    {
        public UprdShipperCompanyRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public ShipperCompany GetShipperCompanyByDuns(string Duns)
        {
            return this.DbContext.ShipperCompany.Where(a => a.DUNS == Duns).FirstOrDefault();
        }
    }
    public interface IUprdShipperCompanyRepository : IRepository<ShipperCompany>
    {
        ShipperCompany GetShipperCompanyByDuns(string Duns);
    }
}
