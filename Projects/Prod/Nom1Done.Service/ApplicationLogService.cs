using Nom1Done.Data.Repositories;
using Nom1Done.Service.Interface;

namespace Nom1Done.Service
{
    public class ApplicationLogService: IApplicationLogService
    {
        private readonly IApplicationLogRepository ApplicationLogRepository;
        public ApplicationLogService(IApplicationLogRepository ApplicationLogRepository)
        {
            this.ApplicationLogRepository = ApplicationLogRepository;
        }
    }
}
