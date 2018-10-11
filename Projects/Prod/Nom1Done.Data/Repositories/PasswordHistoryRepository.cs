using Nom1Done.Infrastructure;
using Nom1Done.Model;
using System;
using System.Linq;

namespace Nom1Done.Data.Repositories
{
    public class PasswordHistoryRepository : RepositoryBase<PasswordHistory>, IPasswordHistoryRepository
    {

        public PasswordHistoryRepository(IDbFactory dbfactory) : base(dbfactory)
        {
           
        }

        public DateTime GetLastModifiedOnByUserID(string userID)
        {
            return DbContext.passwordHistories.Where(a => a.UserID == userID).Select(a => a.LastModifiedOn).FirstOrDefault();
        }

        public bool UpdateLastModifiedDate(string userID)
        {
            var b = DbContext.passwordHistories.Where(a => a.UserID == userID).FirstOrDefault();
            if(b!=null)
            {
                b.LastModifiedOn = DateTime.Now;
                DbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }

    public interface IPasswordHistoryRepository:IRepository<PasswordHistory>
    {
        DateTime GetLastModifiedOnByUserID(string userID);
        bool UpdateLastModifiedDate(string userID);
    }
}
