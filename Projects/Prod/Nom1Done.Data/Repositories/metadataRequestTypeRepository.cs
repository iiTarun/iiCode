using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
   public class metadataRequestTypeRepository : RepositoryBase<metadataRequestType>, ImetadataRequestTypeRepository
    {
        public metadataRequestTypeRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public IQueryable<metadataRequestType> GetByRequestorTypeId(int RequestTypeID)
        {
            return DbContext.metadataRequestType.Where(a=>a.ID==RequestTypeID);
        }
    }

    public interface ImetadataRequestTypeRepository : IRepository<metadataRequestType>
    {
        IQueryable<metadataRequestType> GetByRequestorTypeId(int RequestTypeID);
    }

}
