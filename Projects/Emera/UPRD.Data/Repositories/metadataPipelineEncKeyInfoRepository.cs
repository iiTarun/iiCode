using System.Linq;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class metadataPipelineEncKeyInfoRepository : RepositoryBase<metadataPipelineEncKeyInfo>, ImetadataPipelineEncKeyInfoRepository
    {
        public metadataPipelineEncKeyInfoRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public metadataPipelineEncKeyInfo GetByPipelineId(int pipelineId)
        {
            return this.DbContext.metadataPipelineEncKeyInfo.Where(a => a.PipelineId == pipelineId).FirstOrDefault();
        }
    }
    public interface ImetadataPipelineEncKeyInfoRepository : IRepository<metadataPipelineEncKeyInfo>
    {
        metadataPipelineEncKeyInfo GetByPipelineId(int pipelineId);
    }
}
