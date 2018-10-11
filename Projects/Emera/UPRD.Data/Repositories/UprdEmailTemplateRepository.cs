using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdEmailTemplateRepository : RepositoryBase<EmailTemplate>,IUprdEmailTemplateRepository
    {
        public UprdEmailTemplateRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }
    }

    public interface IUprdEmailTemplateRepository : IRepository<EmailTemplate>
    {
    }
}
