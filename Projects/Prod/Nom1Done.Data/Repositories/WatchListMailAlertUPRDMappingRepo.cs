using Nom1Done.Enums;
using Nom1Done.Infrastructure;
using Nom1Done.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
   public class WatchListMailAlertUPRDMappingRepo : RepositoryBase<WatchListMailAlertUPRDMapping>, IWatchListMailAlertUPRDMappingRepo
    {
        public WatchListMailAlertUPRDMappingRepo(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void SaveChages()
        {
            this.DbContext.SaveChanges();
        }


        public IQueryable<WatchListMailAlertUPRDMapping> GetData( string UserId ,int watchListId, EnercrossDataSets dataSet)
        {
            return DbContext.WatchListMailAlertUPRDMappings.Where(a=>a.UserId==UserId
            && a.WatchListId==watchListId
            && a.DataSetId==dataSet            
            );
        }

    }

    public interface IWatchListMailAlertUPRDMappingRepo : IRepository<WatchListMailAlertUPRDMapping>
    {      
        void SaveChages();
        IQueryable<WatchListMailAlertUPRDMapping> GetData(string UserId, int watchListId, EnercrossDataSets dataSet);

    }
}
