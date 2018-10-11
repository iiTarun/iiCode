using Nom1Done.Data.Repositories;
using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nom1Done.DTO;

namespace Nom1Done.Service
{
    public class metadataBidUpIndicatorService: ImetadataBidUpIndicatorService
    {
        private readonly ImetadataBidUpIndicatorRepository metadataBidUpIndicatorRepository;
        public metadataBidUpIndicatorService(ImetadataBidUpIndicatorRepository metadataBidUpIndicatorRepository)
        {
            this.metadataBidUpIndicatorRepository = metadataBidUpIndicatorRepository;
        }

        public List<BidUpIndicatorDTO> GetBidUpIndicator()
        {            
            return metadataBidUpIndicatorRepository.GetActiveData();
        }
    }
}
