using System.Linq;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
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
