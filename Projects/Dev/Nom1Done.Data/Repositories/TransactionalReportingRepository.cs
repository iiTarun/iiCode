using Nom1Done.Infrastructure;
using Nom1Done.Model;
using Nom1Done.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
   public class TransactionalReportingRepository : RepositoryBase<TransactionalReport>, ITransactionalReportingRepository
    {
        public TransactionalReportingRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void SaveChages()
        {
            this.DbContext.SaveChanges();
        }


    }

    public interface ITransactionalReportingRepository : IRepository<TransactionalReport>
    {
        void SaveChages();

    }

}
