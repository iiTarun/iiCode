using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
   public class RURD_DateTime_HeadRepository : RepositoryBase<RURD_DateTime_Head>, IRURD_DateTime_HeadRepository
    {
        public RURD_DateTime_HeadRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IRURD_DateTime_HeadRepository : IRepository<RURD_DateTime_Head>
    {
        void Save();
    }

}
