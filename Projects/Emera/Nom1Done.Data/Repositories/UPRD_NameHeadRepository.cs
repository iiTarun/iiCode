using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
   public class UPRD_NameHeadRepository : RepositoryBase<UPRD_NameHead>, IUPRD_NameHeadRepository
    {
        public UPRD_NameHeadRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IUPRD_NameHeadRepository : IRepository<UPRD_NameHead>
    {
        void Save();
    }
}
