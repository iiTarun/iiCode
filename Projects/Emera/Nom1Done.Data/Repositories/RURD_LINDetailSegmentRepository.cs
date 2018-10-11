using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
  public  class RURD_LINDetailSegmentRepository : RepositoryBase<RURD_LINDetailSegment>, IRURD_LINDetailSegmentRepository
    {
        public RURD_LINDetailSegmentRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IRURD_LINDetailSegmentRepository : IRepository<RURD_LINDetailSegment>
    {
        void Save();
    }
}
