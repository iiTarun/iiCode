using Nom1Done.Enums;
using Nom1Done.Infrastructure;
using Nom1Done.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
    public class MasterColumnRepository : RepositoryBase<MasterColumn>, IMasterColumnRepository
    {
        public MasterColumnRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void SaveChages()
        {
            this.DbContext.SaveChanges();
        }

        public IQueryable<MasterColumn> GetByDataSet(EnercrossDataSets dataSet)
        { 
            if(dataSet==EnercrossDataSets.SWNT)            
                return DbContext.MasterColumns.Where(a=>a.DataSetId==(EnercrossDataSets.SWNT));
            else if(dataSet==EnercrossDataSets.OACY)
                return DbContext.MasterColumns.Where(a => a.DataSetId==(EnercrossDataSets.OACY));
            else
                return DbContext.MasterColumns.Where(a => a.DataSetId==(EnercrossDataSets.UNSC));
       }

    }
    public interface IMasterColumnRepository : IRepository<MasterColumn>
    {
        void SaveChages();
        IQueryable<MasterColumn> GetByDataSet(EnercrossDataSets dataSet);
    }
}
