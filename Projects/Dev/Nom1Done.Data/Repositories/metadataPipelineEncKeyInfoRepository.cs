using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System.Linq;

namespace Nom1Done.Data.Repositories
{
    public class metadataPipelineEncKeyInfoRepository : RepositoryBase<metadataPipelineEncKeyInfo>, ImetadataPipelineEncKeyInfoRepository
    {
        public metadataPipelineEncKeyInfoRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public metadataPipelineEncKeyInfo GetByPipelineDuns(string pipeDuns)
        {
            return this.DbContext.metadataPipelineEncKeyInfo.Where(a => a.PipeDuns == pipeDuns).FirstOrDefault();
        }

        public metadataPipelineEncKeyInfo GetByPipelineId(int pipelineId)
        {
            return this.DbContext.metadataPipelineEncKeyInfo.Where(a => a.PipelineId == pipelineId).FirstOrDefault();
        }
    }
    public interface ImetadataPipelineEncKeyInfoRepository : IRepository<metadataPipelineEncKeyInfo>
    {
        metadataPipelineEncKeyInfo GetByPipelineId(int pipelineId);
        metadataPipelineEncKeyInfo GetByPipelineDuns(string pipeDuns);
    }
}
