using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
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
