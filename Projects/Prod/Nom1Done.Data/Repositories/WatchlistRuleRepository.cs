using Nom1Done.DTO;
using Nom1Done.Infrastructure;
using Nom1Done.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
    public class WatchlistRuleRepository : RepositoryBase<WatchlistRule>, IWatchlistRuleRepository
    {
        public WatchlistRuleRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void SaveChages()
        {
            this.DbContext.SaveChanges();
        }


        public int AddWatchListRule(int watchListId , WatchListRule watchListRule)
        {
            WatchlistRule newRule = new WatchlistRule() {
                WatchlistId= watchListId,
                ColumnId= watchListRule.PropertyId,
                OperatorId= watchListRule.ComparatorsId,
                RuleValue = watchListRule.value,
                PipelineDuns = watchListRule.PipelineDuns,
                LocationIdentifier = watchListRule.LocationIdentifier,
                IsCriticalNotice= watchListRule.IsCriticalNotice,
                AlertSent=watchListRule.AlertSent,
                AlertFrequency=watchListRule.AlertFrequency
            };
            DbContext.WatchlistRules.Add(newRule);
            DbContext.SaveChanges();
            return newRule.Id;
        }

        public IQueryable<WatchlistRule> GetByWatchListId(int watchListId)
        {
            return DbContext.WatchlistRules.Where(a => a.WatchlistId == watchListId);
        }
    }
    public interface IWatchlistRuleRepository : IRepository<WatchlistRule>
    {
        void SaveChages();

        int AddWatchListRule(int watchListId, WatchListRule watchListRule);

        IQueryable<WatchlistRule> GetByWatchListId(int watchListId);
    }
}


