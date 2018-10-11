using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System.Linq;

namespace Nom1Done.Data.Repositories
{
    public class TradingPartnerWorksheetRepository : RepositoryBase<TradingPartnerWorksheet>, ITradingPartnerWorksheetRepository
    {
        public TradingPartnerWorksheetRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public TradingPartnerWorksheet GetByPipelineId(int pipeId)
        {
            return this.DbContext.TradingPartnerWorksheet.Where(a => a.PipelineID == pipeId).FirstOrDefault();
        }
    }
    public interface ITradingPartnerWorksheetRepository : IRepository<TradingPartnerWorksheet>
    {
        TradingPartnerWorksheet GetByPipelineId(int pipeId);
    }
}
