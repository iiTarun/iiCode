using Nom1Done.Infrastructure;
using Nom1Done.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
   public class DataTypeGroupingRepository : RepositoryBase<DataTypeGrouping>, IDataTypeGroupingRepository
    {
        public DataTypeGroupingRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void SaveChages()
        {
            this.DbContext.SaveChanges();
        }
    }
    public interface IDataTypeGroupingRepository : IRepository<DataTypeGrouping>
    {
        void SaveChages();       

    }
}
