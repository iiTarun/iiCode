using Nom1Done.DTO;
using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
    public class ShipperCompanyRepository : RepositoryBase<ShipperCompany>, IShipperCompanyRepository
    {
        public ShipperCompanyRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }
        public BOShipperCompany GetShipperCompanyByUserId(string userId)
        {
            var shipperCompanyIds = this.DbContext.Shipper.Where(a => a.UserId == userId && a.IsActive == true).Select(a => a.ShipperCompanyID).FirstOrDefault();
            var result = (from a in this.DbContext.ShipperCompany
                          where (a.IsActive == true && a.ID == shipperCompanyIds)
                          select new BOShipperCompany
                          {
                              ID = a.ID,
                              Name = a.Name,
                              DUNS = a.DUNS,
                              IsActive = a.IsActive,
                              CreatedBy = a.CreatedBy,
                              CreatedDate = a.CreatedDate,
                              ModifiedBy = a.ModifiedBy,
                              ModifiedDate = a.ModifiedDate
                          }).FirstOrDefault();
            return result;
        }
        public BOShipperCompany GetShipperByUserId(string userId)
        {
            return this.DbContext.Shipper.Where(a => a.UserId == userId).
                Select(a => new BOShipperCompany
                {
                    ID = a.ShipperCompanyID,
                    FirstName = a.FirstName,
                    LastName = a.LastName
                }).FirstOrDefault();
        }

        public ShipperCompany GetShipperCompanyByDuns(string Duns)
        {
            return this.DbContext.ShipperCompany.Where(a => a.DUNS == Duns).FirstOrDefault();
        }
    }
    public interface IShipperCompanyRepository : IRepository<ShipperCompany>
    {
        BOShipperCompany GetShipperCompanyByUserId(string userId);
        BOShipperCompany GetShipperByUserId(string userId);
        ShipperCompany GetShipperCompanyByDuns(string Duns);
    }
}
