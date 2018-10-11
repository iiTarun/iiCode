using System;
using System.Collections.Generic;
using Nom1Done.Data.Repositories;
using Nom1Done.DTO;
using Nom1Done.Service.Interface;
using System.Linq;

namespace Nom1Done.Service
{
    public class metadataCapacityTypeIndicatorService: ImetadataCapacityTypeIndicatorService
    {
        private readonly ImetadataCapacityTypeIndicatorRepository metadataCapacityTypeIndicatorRepository;
        public metadataCapacityTypeIndicatorService(ImetadataCapacityTypeIndicatorRepository metadataCapacityTypeIndicatorRepository)
        {
            this.metadataCapacityTypeIndicatorRepository = metadataCapacityTypeIndicatorRepository;
        }

        public List<CapacityIndicatorDTO> GetCapacityTypes()
        {           

            return metadataCapacityTypeIndicatorRepository.GetCapacityTypes();
        }
    }
}
