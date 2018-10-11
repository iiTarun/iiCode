using Nom1Done.Data.Repositories;
using Nom1Done.DTO;
using Nom1Done.Model;
using Nom1Done.Service.Interface;

namespace Nom1Done.Service
{
    public class EmailQueuService: IEmailQueueService
    {
        private readonly IEmailQueueRepository EmailQueueRepoitory;
        public EmailQueuService(IEmailQueueRepository EmailQueueRepoitory)
        {
            this.EmailQueueRepoitory = EmailQueueRepoitory;
        }


        public void AddEmailQueue(EmailQueueDto email)
        {
            if (email != null)
            {
                EmailQueue model = new EmailQueue()
                {
                    ToUserID = email.ToUserID,
                    Subject = email.Subject,
                    Email = email.Email,
                    Recipient = email.Recipient,
                    CC = email.CC,
                    Bcc = email.Bcc,
                    IsSent = email.IsSent,
                    CreatedDate = email.CreatedDate,
                    SentDate = email.SentDate
                };

                EmailQueueRepoitory.Add(model);
                EmailQueueRepoitory.Save();
            }
        }
    }
}
