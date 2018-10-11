using Nom1Done.Data.Repositories;
using Nom1Done.Service.Interface;
using System;

namespace Nom1Done.Service
{
    public class PasswordHistoryService : IPasswordHistoryService
    {
        IPasswordHistoryRepository _IPasswordHistoryRepository;

        public PasswordHistoryService(IPasswordHistoryRepository IPasswordHistoryRepository)
        {
            _IPasswordHistoryRepository = IPasswordHistoryRepository;
        }
        public DateTime GetLastModifiedDateByUser(string userID)
        {
            var LastModifieddate = _IPasswordHistoryRepository.GetLastModifiedOnByUserID(userID);
            return LastModifieddate;
        }

        public bool UpdateLastModifiedDateByUser(string userID)
        {
            return _IPasswordHistoryRepository.UpdateLastModifiedDate(userID);

        }
    }
}
