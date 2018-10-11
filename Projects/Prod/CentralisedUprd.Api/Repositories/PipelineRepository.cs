using CentralisedUprd.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System;


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
    }


}
