using Nom1Done.DTO;

namespace Nom1Done.Service.Interface
{
    public interface IEmailQueueService
    {
        void AddEmailQueue(EmailQueueDto email);
    }
}
