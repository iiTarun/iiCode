using System.Linq;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdmetadataPipelineEncKeyInfoRepository : RepositoryBase<metadataPipelineEncKeyInfo>, IUprdmetadataPipelineEncKeyInfoRepository
    {
        public UprdmetadataPipelineEncKeyInfoRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public metadataPipelineEncKeyInfo GetByPipelineId(int pipelineId)
        {
            return this.DbContext.metadataPipelineEncKeyInfo.Where(a => a.PipelineId == pipelineId).FirstOrDefault();
        }
    }
    public interface IUprdmetadataPipelineEncKeyInfoRepository : IRepository<metadataPipelineEncKeyInfo>
    {
        metadataPipelineEncKeyInfo GetByPipelineId(int pipelineId);
    }
}
