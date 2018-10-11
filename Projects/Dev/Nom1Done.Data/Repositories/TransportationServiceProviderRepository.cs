using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
    public class TransportationServiceProviderRepository : RepositoryBase<TransportationServiceProvider>,ITransportationServiceProviderRepository
    {
        public TransportationServiceProviderRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }
    public interface ITransportationServiceProviderRepository:IRepository<TransportationServiceProvider>
    {
        void Save();
    }
}
