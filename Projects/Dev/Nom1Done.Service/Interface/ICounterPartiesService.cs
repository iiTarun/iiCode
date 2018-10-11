using Nom.ViewModel;
using System.Collections.Generic;

namespace Nom1Done.Service.Interface
{
    public interface ICounterPartiesService
    {        
        CounterPartiesDTO GetCounterPartyByPropCode(string propCode);
        CounterPartiesDTO GetCounterPartyByIdentifier(string identifier);
    }
}
