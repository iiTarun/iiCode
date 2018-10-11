using System.Collections.Generic;
using System.Linq;
using System;
using UPRD.Data;
using UPRD.Model;

namespace WatchlistMailManagement.Repositories
{
    public class PipelineRepository 
    {
        UPRDEntities DbContext = new UPRDEntities();
        public IEnumerable<Pipeline> GetAllActivePipeline()
        {
            return DbContext.Pipeline.Where(a => a.IsActive == true);
        }

        public Pipeline GetPipelineByDuns(string DunsNo)
        {
            var Pipeline = this.DbContext.Pipeline.Where(c => c.DUNSNo == DunsNo).FirstOrDefault();
            return Pipeline;
        }

        public void SaveChanges()
        {
            this.DbContext.SaveChanges();
        }

        public IEnumerable<Pipeline> GetActiveUprdPipelines()
        {
            return this.DbContext.Pipeline.Where(a => a.IsActive && a.IsUprdActive);
        }
    }


}
