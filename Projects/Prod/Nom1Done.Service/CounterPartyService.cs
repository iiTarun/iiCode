using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nom.ViewModel;
using Nom1Done.Data.Repositories;

namespace Nom1Done.Service
{
    public class CounterPartyService : ICounterPartiesService
    {
        ICounterPartyRepository counterPartyRepository;
        IModalFactory modalFactory;
        public CounterPartyService(ICounterPartyRepository counterPartyRepository, IModalFactory modalFactory)
        {
            this.counterPartyRepository = counterPartyRepository;
            this.modalFactory = modalFactory;
        }        

        public CounterPartiesDTO GetCounterPartyByPropCode(string propCode)
        {
            var data= counterPartyRepository.GetCounterPartyByPropCode(propCode);
            return modalFactory.Parse(data);
        }

        public CounterPartiesDTO GetCounterPartyByIdentifier(string identifier)
        {
            var data = counterPartyRepository.GetCounterPartyByIdentifier(identifier);
            if (data == null)
                return null;
            return modalFactory.Parse(data);
        }

    }
}
