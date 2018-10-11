using Nom1Done.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        NomEntities dbContext;

        public NomEntities Init()
        {
            return dbContext ?? (dbContext = new NomEntities());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
