using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
    public class NMQR_SublineItemDetailRepository: RepositoryBase<NMQR_SublineItemDetail>, INMQR_SublineItemDetailRepository
    {
        public NMQR_SublineItemDetailRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface INMQR_SublineItemDetailRepository : IRepository<NMQR_SublineItemDetail>
    {
        void Save();
    }
}
