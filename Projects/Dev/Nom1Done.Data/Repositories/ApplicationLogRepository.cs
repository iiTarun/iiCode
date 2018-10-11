using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
    public class ApplicationLogRepository : RepositoryBase<ApplicationLog>,IApplicationLogRepository
    {
        public ApplicationLogRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }
    public interface IApplicationLogRepository : IRepository<ApplicationLog>
    {
        void Save();
    }
}
