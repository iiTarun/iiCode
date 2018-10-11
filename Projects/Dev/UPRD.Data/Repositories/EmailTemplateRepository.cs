using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class EmailTemplateRepository : RepositoryBase<EmailTemplate>,IEmailTemplateRepository
    {
        public EmailTemplateRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }
    }

    public interface IEmailTemplateRepository : IRepository<EmailTemplate>
    {
    }
}
