using Nom.ViewModel;
using Nom1Done.DTO;
using Nom1Done.Infrastructure;

namespace Nom1Done.Data.Repositories
{
    public class PNTNominationRepository : RepositoryBase<BatchDetailDTO>, IPNTNominationRepository
    {
        public PNTNominationRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
    public interface IPNTNominationRepository : IRepository<BatchDetailDTO>
    {

    }
}
