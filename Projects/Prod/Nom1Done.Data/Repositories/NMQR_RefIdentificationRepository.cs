using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
    public class NMQR_RefIdentificationRepository :  RepositoryBase<NMQR_RefIdentification>, INMQR_RefIdentificationRepository
    {
        public NMQR_RefIdentificationRepository(IDbFactory dbfactory): base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface INMQR_RefIdentificationRepository : IRepository<NMQR_RefIdentification>
    {
        void Save();
    }
}
