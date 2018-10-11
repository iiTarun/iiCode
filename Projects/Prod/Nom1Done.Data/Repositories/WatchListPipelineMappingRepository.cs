using Nom1Done.Infrastructure;
using Nom1Done.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
   public class WatchListPipelineMappingRepository : RepositoryBase<WatchListPipelineMapping>, IWatchListPipelineMappingRepository
    {
        public WatchListPipelineMappingRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public IQueryable<WatchListPipelineMapping> GetByWatchListId(int watchlistId) {
            return DbContext.WatchListPipelineMappings.Where(a => a.WatchListId == watchlistId);
        }

        public IQueryable<WatchListPipelineMapping> GetByPipelineId(int pipelineId) {
            return DbContext.WatchListPipelineMappings.Where(a => a.PipelineId == pipelineId);
        }

        public void SaveChages()
        {
            this.DbContext.SaveChanges();
        }



    }

    public interface IWatchListPipelineMappingRepository : IRepository<WatchListPipelineMapping>
    {
        void SaveChages();
        IQueryable<WatchListPipelineMapping> GetByWatchListId(int watchlistId);
        IQueryable<WatchListPipelineMapping> GetByPipelineId(int pipelineId);
    }

}
