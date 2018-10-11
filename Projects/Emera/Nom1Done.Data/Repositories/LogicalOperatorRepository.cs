using Nom1Done.Infrastructure;
using Nom1Done.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
    public class LogicalOperatorRepository : RepositoryBase<LogicalOperator>, ILogicalOperatorRepository
    {
        public LogicalOperatorRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void SaveChages()
        {
            this.DbContext.SaveChanges();
        }

        public IQueryable<LogicalOperator> GetByDataTypeId(int dataTypeId)
        {
            return DbContext.LogicalOperators.Where(a => a.DataTypeId == dataTypeId);
        }

    }
    public interface ILogicalOperatorRepository : IRepository<LogicalOperator>
    {
        void SaveChages();
        IQueryable<LogicalOperator> GetByDataTypeId(int dataTypeId);
    }
}
