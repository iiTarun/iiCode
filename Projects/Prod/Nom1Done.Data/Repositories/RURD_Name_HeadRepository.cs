using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
   public class RURD_Name_HeadRepository : RepositoryBase<RURD_Name_Head>, IRURD_Name_HeadRepository
    {
        public RURD_Name_HeadRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IRURD_Name_HeadRepository : IRepository<RURD_Name_Head>
    {
        void Save();
    }
}
