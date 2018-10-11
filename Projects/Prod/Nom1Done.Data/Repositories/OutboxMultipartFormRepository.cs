using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
  public class OutboxMultipartFormRepository : RepositoryBase<OutboxMultipartForm>, IOutboxMultipartFormRepository
    {
        public OutboxMultipartFormRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }
    }

    public interface IOutboxMultipartFormRepository : IRepository<OutboxMultipartForm>
    {
    }
}
