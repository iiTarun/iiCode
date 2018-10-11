using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nom1Done.Model;
using Nom1Done.Data.Repositories;

namespace Nom1Done.Service
{
    public class metadataRequestTypeService : ImetadataRequestTypeService
    {
        ImetadataRequestTypeRepository _ImetadataRequestTypeRepository;
        public metadataRequestTypeService(ImetadataRequestTypeRepository ImetadataRequestTypeRepository) {
            _ImetadataRequestTypeRepository=ImetadataRequestTypeRepository;
        }
        public IEnumerable<metadataRequestType> GetRequestType()
        {
            return _ImetadataRequestTypeRepository.GetAll();
        }
    }
}
