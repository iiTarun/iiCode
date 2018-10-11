using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nom1Done.DTO;
using Nom1Done.Data.Repositories;

namespace Nom1Done.Service
{
    public class metadataQuantityTypeIndicatorService : ImetadataQuantityTypeIndicatorService
    {
        ImetadataQuantityTypeRepository ImetadataQuantityTypeRepository;
        public metadataQuantityTypeIndicatorService(ImetadataQuantityTypeRepository ImetadataQuantityTypeRepository) {
            this.ImetadataQuantityTypeRepository=ImetadataQuantityTypeRepository;
        }
        public List<QuantityIndicatorDTO> GetQuantityTypes()
        {
            List<QuantityIndicatorDTO> model = new List<QuantityIndicatorDTO>();
            model = ImetadataQuantityTypeRepository.GetAll().Where(a => a.IsActive == true).Select(a => new QuantityIndicatorDTO
            {
                Code = a.Code,
                Name = a.Name
            }).ToList();
            return model;
        }
    }
}
