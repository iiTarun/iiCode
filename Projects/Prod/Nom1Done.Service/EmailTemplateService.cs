using Nom1Done.Data.Repositories;
using Nom1Done.Service.Interface;

namespace Nom1Done.Service
{
    public class EmailTemplateService: IEmailTemplateService
    {
        private readonly IEmailTemplateRepository EmailTemplateRepository;
        public EmailTemplateService(IEmailTemplateRepository EmailTemplateRepository)
        {
            this.EmailTemplateRepository = EmailTemplateRepository;
        }
    }
}
