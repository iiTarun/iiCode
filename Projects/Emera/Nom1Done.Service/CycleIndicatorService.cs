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
    public class CycleIndicatorService : ICycleIndicator
    {
        ImetadataCycleRepository ImetadataCycleRepository;
        public CycleIndicatorService(ImetadataCycleRepository ImetadataCycleRepository) {
           this.ImetadataCycleRepository=ImetadataCycleRepository;
        }

        public List<CycleIndicatorDTO> GetCycles()
        {  
            return ImetadataCycleRepository.GetActiveCycles();
        }
    }
}
