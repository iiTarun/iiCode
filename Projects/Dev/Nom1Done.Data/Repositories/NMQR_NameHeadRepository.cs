using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
    public class NMQR_NameHeadRepository : RepositoryBase<NMQR_NameHead>, INMQR_NameHeadRepository  
    {
        public NMQR_NameHeadRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface INMQR_NameHeadRepository : IRepository<NMQR_NameHead>
    {
        void Save();
    }
}
