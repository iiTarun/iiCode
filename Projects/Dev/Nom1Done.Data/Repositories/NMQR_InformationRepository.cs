using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
    public class NMQR_InformationRepository : RepositoryBase<NMQR_Information>, INMQR_InformationRepository
    {
        public NMQR_InformationRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface INMQR_InformationRepository : IRepository<NMQR_Information>
    {
        void Save();
    }
}
