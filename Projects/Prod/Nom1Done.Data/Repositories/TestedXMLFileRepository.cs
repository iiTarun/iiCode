using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
  public  class TestedXMLFileRepository : RepositoryBase<TestedXMLFile>, ITestedXMLFileRepository
    {
        public TestedXMLFileRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }
    }

    public interface ITestedXMLFileRepository : IRepository<TestedXMLFile>
    {
    }
}
