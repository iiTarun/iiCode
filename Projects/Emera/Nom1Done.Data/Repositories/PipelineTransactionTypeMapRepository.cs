using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Nom1Done.Data.Repositories
{
    public class PipelineTransactionTypeMapRepository : RepositoryBase<Pipeline_TransactionType_Map>, IPipelineTransactionTypeMapRepository
    {
        public PipelineTransactionTypeMapRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IQueryable<Pipeline_TransactionType_Map> GetTTbyPipelineDuns(string pipelineDuns) 
        {
            return DbContext.Pipeline_TransactionType_Map.Where(a => a.PipeDuns == pipelineDuns && a.IsActive==true);
        }

        public IQueryable<Pipeline_TransactionType_Map> GetTTbyPathType(string pipelineDuns,string pathType)
        {
            return DbContext.Pipeline_TransactionType_Map.Where(a => a.PipeDuns == pipelineDuns && a.IsActive == true && a.PathType.Trim() == pathType);
        }

    }
    public interface IPipelineTransactionTypeMapRepository : IRepository<Pipeline_TransactionType_Map>
    {
        IQueryable<Pipeline_TransactionType_Map> GetTTbyPipelineDuns(string pipelineDuns);
        IQueryable<Pipeline_TransactionType_Map> GetTTbyPathType(string pipelineDuns, string pathType);
    }
}
