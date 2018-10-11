using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
 public class UPRD_DataRequestCodeRepository : RepositoryBase<UPRD_DataRequestCode>, IUPRD_DataRequestCodeRepository
    {
        public UPRD_DataRequestCodeRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IUPRD_DataRequestCodeRepository : IRepository<UPRD_DataRequestCode>
    {
        void Save();
    }
}
