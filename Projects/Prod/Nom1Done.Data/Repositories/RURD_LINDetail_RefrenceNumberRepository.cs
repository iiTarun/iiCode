using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
   public class RURD_LINDetail_RefrenceNumberRepository : RepositoryBase<RURD_LINDetail_RefrenceNumber>, IRURD_LINDetail_RefrenceNumberRepository
    { 
        public RURD_LINDetail_RefrenceNumberRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IRURD_LINDetail_RefrenceNumberRepository : IRepository<RURD_LINDetail_RefrenceNumber>
    {
        void Save();
    }

}
