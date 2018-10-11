using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdTransportationServiceProviderRepository : RepositoryBase<TransportationServiceProvider>,IUprdTransportationServiceProviderRepository
    {
        public UprdTransportationServiceProviderRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }
    public interface IUprdTransportationServiceProviderRepository:IRepository<TransportationServiceProvider>
    {
        void Save();
    }
}
