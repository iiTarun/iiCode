using System.Linq;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdTradingPartnerWorksheetRepository : RepositoryBase<TradingPartnerWorksheet>, IUprdTradingPartnerWorksheetRepository
    {
        public UprdTradingPartnerWorksheetRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public TradingPartnerWorksheet GetByPipelineId(int pipeId)
        {
            return this.DbContext.TradingPartnerWorksheet.Where(a => a.PipelineID == pipeId).FirstOrDefault();
        }
    }
    public interface IUprdTradingPartnerWorksheetRepository : IRepository<TradingPartnerWorksheet>
    {
        TradingPartnerWorksheet GetByPipelineId(int pipeId);
    }
}
