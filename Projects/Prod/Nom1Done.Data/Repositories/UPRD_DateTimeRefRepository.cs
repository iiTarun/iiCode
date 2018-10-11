using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
   public class UPRD_DateTimeRefRepository : RepositoryBase<UPRD_DateTimeRef>, IUPRD_DateTimeRefRepository
    {
        public UPRD_DateTimeRefRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IUPRD_DateTimeRefRepository : IRepository<UPRD_DateTimeRef>
    {
        void Save();
    }
}
