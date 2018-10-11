using System.Linq;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdPipelineEDISettingRepository : RepositoryBase<PipelineEDISetting>,IUprdPipelineEDISettingRepository
    {
        public UprdPipelineEDISettingRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public PipelineEDISetting GetPipelineSetting(string pipeDuns, int DatasetId,string shipperDuns)
        {
            return this.DbContext.PipelineEDISetting.Where(a => a.PipeDuns == pipeDuns && a.DatasetId == DatasetId && a.ShipperCompDuns == shipperDuns).FirstOrDefault();
        }

        public PipelineEDISetting GetPipelineSettingForManuallySend(int DatasetId, string shipperDuns)
        {
            return DbContext.PipelineEDISetting.Where(a => a.DatasetId == DatasetId && a.ShipperCompDuns == shipperDuns && a.SendManually).FirstOrDefault();
        }

        public void SaveChanges()
        {
            this.DbContext.SaveChanges();
        }
    }
    public interface IUprdPipelineEDISettingRepository : IRepository<PipelineEDISetting>
    {
        void SaveChanges();
        PipelineEDISetting GetPipelineSetting(string pipeDuns, int DatasetId, string shipperDuns);
        PipelineEDISetting GetPipelineSettingForManuallySend(int DatasetId,string shipperDuns);
    }
}
