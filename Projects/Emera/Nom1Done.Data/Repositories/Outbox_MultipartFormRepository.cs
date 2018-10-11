using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
    public class Outbox_MultipartFormRepository: RepositoryBase<Outbox_MultipartForm>, IOutbox_MultipartFormRepository
    {
        public Outbox_MultipartFormRepository(IDbFactory dbfactory):base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IOutbox_MultipartFormRepository:IRepository<Outbox_MultipartForm>
    {
        void Save();
    }
}
