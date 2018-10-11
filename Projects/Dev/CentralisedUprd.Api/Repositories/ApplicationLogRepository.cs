using CentralisedUprd.Api.Models;
using System;

namespace CentralisedUprd.Api.Repositories
{
    public class ApplicationLogRepository
    {
        UprdDbEntities1 DbContext;

        public ApplicationLogRepository() {
            DbContext = new UprdDbEntities1();
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

    }
}