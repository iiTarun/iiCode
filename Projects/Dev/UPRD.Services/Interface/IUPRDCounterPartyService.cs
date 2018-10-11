using System.Collections.Generic;
using UPRD.DTO;


namespace UPRD.Services.Interface
{
    public interface IUPRDCounterPartyService
    {
        CounterPartyDTO GetCounterPartyByid(int id);
        IEnumerable<CounterPartyDTO> GetCounterParties();

        void Save();
        bool DeleteCounterPartiesByID(int id);
        string UpdateCounterPartyByID(CounterPartyDTO Counter);
        bool ActivateCounterParty(int id);
        bool ClientEnvironmentsetting();
    }
}
