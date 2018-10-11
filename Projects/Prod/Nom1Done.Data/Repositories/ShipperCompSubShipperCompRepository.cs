using Nom1Done.Infrastructure;
using Nom1Done.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
    public class ShipperCompSubShipperCompRepository : RepositoryBase<ShipperCompSubShipperComp>, IShipperCompSubShipperCompRepository
    {
        public ShipperCompSubShipperCompRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public ShipperCompSubShipperComp GetShipperAndSubShipperCompOnDuns(string dunsNo)
        {
            return this.DbContext.ShipperCompSubShipperComp.Where(a => a.ShipperCompDuns == dunsNo).FirstOrDefault();
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }
    public interface IShipperCompSubShipperCompRepository : IRepository<ShipperCompSubShipperComp>
    {
        ShipperCompSubShipperComp GetShipperAndSubShipperCompOnDuns(string dunsNo);
        void Save();
    }
}
