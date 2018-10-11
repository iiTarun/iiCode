using System;
using UPRD.Data;
using UPRD.Model;

namespace WatchlistMailManagement.Repositories
{
    public class ApplicationLogRepository
    {
        UPRDEntities DbContext;

        public ApplicationLogRepository() {
            DbContext = new UPRDEntities();
        }

        public void AppLogManager(string source, string type, string errMsg)
        {
            WatchListLog log = new WatchListLog();
            log.Source = source.ToString();
            log.Type = type;
            log.Description = errMsg.ToString();
            log.CreatedDate = DateTime.Now;
            DbContext.WatchListLogs.Add(log);
            DbContext.SaveChanges();
        }

    }
}