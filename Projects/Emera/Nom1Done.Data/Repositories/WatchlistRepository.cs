using Nom1Done.Enums;
using Nom1Done.Infrastructure;
using Nom1Done.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
    public  class WatchlistRepository : RepositoryBase<Watchlist>, IWatchlistRepository
    {
        public WatchlistRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void SaveChages()
        {
            this.DbContext.SaveChanges();
        }

        public IQueryable<Watchlist> GetByDataSet(int pipelineId, EnercrossDataSets dataSet,string userId)
        {
            var result = (from mapping in DbContext.WatchListPipelineMappings
                          join res in DbContext.Watchlists on mapping.WatchListId equals res.Id
                          where mapping.PipelineId == pipelineId && res.UserId == userId && res.DataSetId == dataSet
                          select res);
                             
                return result;           
        }

        //public IQueryable<Watchlist> GetByAlertFrequency(WatchlistAlertFrequency alertFrequency) {
        //    return DbContext.Watchlists.Where(a => a.AlertFrequency == alertFrequency);
        //}

        public int AddWatchList(string Name,EnercrossDataSets dataset,string UserId)
        {
            Watchlist newWatchList = new Watchlist();           
            newWatchList.Name = Name;
            if (dataset == EnercrossDataSets.SWNT)
                newWatchList.DataSetId = EnercrossDataSets.SWNT;
            else if (dataset == EnercrossDataSets.OACY)
                newWatchList.DataSetId = EnercrossDataSets.OACY;
            else
                newWatchList.DataSetId = EnercrossDataSets.UNSC;
            newWatchList.UserId = UserId;         
            newWatchList.CreatedDate = DateTime.UtcNow;
            newWatchList.ModifiedDate = DateTime.UtcNow;
            DbContext.Watchlists.Add(newWatchList);
            DbContext.SaveChanges();
           return newWatchList.Id;
           
        }


        public IQueryable<Watchlist> GetByUserId(string userId)
        {
            return DbContext.Watchlists.Where(a => userId == a.UserId);
        }

        public IQueryable<Watchlist> GetAllBydataSet(EnercrossDataSets dataset)
        {
            return DbContext.Watchlists.Where(a => dataset == a.DataSetId);
        }

    }
    public interface IWatchlistRepository : IRepository<Watchlist>
    {
        int AddWatchList(string Name, EnercrossDataSets dataset, string UserId);

        IQueryable<Watchlist> GetByDataSet(int pipelineId,EnercrossDataSets dataSet, string userId);
        void SaveChages();
        IQueryable<Watchlist> GetByUserId(string userId);

        IQueryable<Watchlist> GetAllBydataSet(EnercrossDataSets dataset);
     

    }
}

