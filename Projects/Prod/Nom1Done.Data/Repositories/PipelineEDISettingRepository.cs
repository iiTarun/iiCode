using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Linq;

namespace Nom1Done.Data.Repositories
{
    public class PipelineEDISettingRepository : RepositoryBase<PipelineEDISetting>,IPipelineEDISettingRepository
    {
        public PipelineEDISettingRepository(IDbFactory dbFactory) : base(dbFactory)
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
    public interface IPipelineEDISettingRepository : IRepository<PipelineEDISetting>
    {
        void SaveChanges();
        PipelineEDISetting GetPipelineSetting(string pipeDuns, int DatasetId, string shipperDuns);
        PipelineEDISetting GetPipelineSettingForManuallySend(int DatasetId,string shipperDuns);
    }
}
