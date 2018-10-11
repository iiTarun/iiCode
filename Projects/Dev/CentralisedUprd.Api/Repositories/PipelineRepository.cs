using CentralisedUprd.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using CentralisedUprd.Api.UPRD.DTO;

namespace Nom1Done.Data.Repositories
{
    public class PipelineRepository 
    {

        UprdDbEntities1 DbContext = new UprdDbEntities1();
        public IEnumerable<Pipeline> GetAllActivePipeline()
        {
            return DbContext.Pipelines.Where(a => a.IsActive == true);
        }

        public Pipeline GetPipelineByDuns(string DunsNo)
        {
            var Pipeline = this.DbContext.Pipelines.Where(c => c.DUNSNo == DunsNo).FirstOrDefault();
            return Pipeline;
        }

        public void SaveChanges()
        {
            this.DbContext.SaveChanges();
        }

        public IEnumerable<Pipeline> GetActiveUprdPipelines()
        {
            return this.DbContext.Pipelines.Where(a => a.IsActive && a.IsUprdActive);
        }

        public IEnumerable<PipelineDTO> GetPipelinesByUser(PipelineByUserDTO pipelineByUser)
        {
         

            List<PipelineDTO> Query = (from a in DbContext.UserPipelineMappings
                                 join b in DbContext.Pipelines on a.PipeDuns equals b.DUNSNo
                                where a.shipperId == pipelineByUser.ShipperID && a.userId == pipelineByUser.UserID && (a.IsNoms || a.IsUPRD)
                                 select new PipelineDTO
                                 {
                                     ID = b.ID,
                                     DUNSNo = b.DUNSNo,
                                     Name = b.Name + " (" + b.DUNSNo + ")",
                                     ModelTypeID = b.ModelTypeID,
                                     IsUprdActive = b.IsUprdActive,
                                     IsActive = b.IsActive,
                                     TSPId = b.TSPId,
                                     CreatedBy = b.CreatedBy,
                                     CreatedDate = b.CreatedDate,
                                     ModifiedBy = b.ModifiedBy,
                                     ModifiedDate = b.ModifiedDate,
                                     ToUseTSPDUNS = b.ToUseTSPDUNS,
                                     TempItem = b.DUNSNo + "-" + b.ModelTypeID,
                                     IsNoms = a.IsNoms,
                                     IsUPRD = a.IsUPRD
                                 }).Distinct().OrderBy(c => c.Name).ToList();
               
          return Query;
  
        }

    }


}
