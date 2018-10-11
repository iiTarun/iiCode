using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
    public class NMQR_ReferenceIdentification_N9Repository: RepositoryBase<NMQR_ReferenceIdentification_N9>, INMQR_ReferenceIdentification_N9Repository
    {
        public NMQR_ReferenceIdentification_N9Repository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface INMQR_ReferenceIdentification_N9Repository : IRepository<NMQR_ReferenceIdentification_N9>
    {
        void Save();
    }
}
