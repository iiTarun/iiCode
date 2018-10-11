using UPRD.Model;
using UPRD.Infrastructure;
using System;

namespace UPRD.Data.Repositories
{
    public class UPRDApplicationLogRepository : RepositoryBase<ApplicationLog>,IUPRDApplicationLogRepository
    {
        public UPRDApplicationLogRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }
        public void AppLogManager(string source, string type, string errMsg)
        {
            ApplicationLog log = new ApplicationLog();
            log.Source = source.ToString();
            log.Type = type;
            log.Description = errMsg.ToString();
            log.CreatedDate = DateTime.Now;
            DbContext.ApplicationLogs.Add(log);
            DbContext.SaveChanges();
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }
    public interface IUPRDApplicationLogRepository : IRepository<ApplicationLog>
    {
        void AppLogManager(string source, string type, string errMsg);
        void Save();

    }
}
