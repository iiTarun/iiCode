using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
    public class NMQR_HeaderRepository : RepositoryBase<NMQR_Header>, INMQR_HeaderRepository
    {
        public NMQR_HeaderRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface INMQR_HeaderRepository : IRepository<NMQR_Header>
    {
        void Save();
    }
}
