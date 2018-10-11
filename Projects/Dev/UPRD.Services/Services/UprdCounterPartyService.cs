using System.Collections.Generic;
using UPRD.Infrastructure;
using UPRD.Model;

using UPRD.Service;
using UPRD.Service.Interface;
using UPRD.Services.Interface;
using UPRD.DTO;
using UPRD.Services;
using UPRD.Data.Repositories;
using System.Linq;

namespace UPRD.Services.Interface
{
    public class CounterPartyService : IUPRDCounterPartyService
    {

        private readonly IUPRDCounterPartyRepository _UPRDCounterParty;

        IModalFactory modalFactory;
        public CounterPartyService(IUPRDCounterPartyRepository IUPRDCounterPartyRepository, IModalFactory modalFactory)
        {
            _UPRDCounterParty = IUPRDCounterPartyRepository;
            this.modalFactory = modalFactory;
        }

        public CounterPartyDTO GetCounterPartyByid(int id)
        {
            var items = _UPRDCounterParty.GetCounterPartyByid(id);

            return modalFactory.Parse(items);

        }


        public bool DeleteCounterPartiesByID(int ID)
        {

            _UPRDCounterParty.DeleteCounterPartiesByID(ID);

            return true;
        }

        public void Save()
        {
            this._UPRDCounterParty.Save();
        }
        

        public bool ActivateCounterParty(int id)
        {
           var Activate = _UPRDCounterParty.ActivateCounterParty(id);
            return Activate;

        }
        public IEnumerable<CounterPartyDTO> GetCounterParties()
        {
            var counter = _UPRDCounterParty.GetCounterParties().ToList();


            return CounterPartyStatusDTO(counter);


        }
        private List<CounterPartyDTO> CounterPartyStatusDTO(List<CounterParty> list)
        {
            List<CounterPartyDTO> CounterStatusList = new List<CounterPartyDTO>();
            if (list != null && list.Count > 0)
                foreach (var item in list)
                {
                    CounterStatusList.Add(modalFactory.Parse(item));
                }
            return CounterStatusList;
        }



        public string UpdateCounterPartyByID(CounterPartyDTO Counter)
        {


            string Notification = _UPRDCounterParty.UpdateCounterPartyByID(modalFactory.Create(Counter));

            return Notification;
        }

        public bool ClientEnvironmentsetting()
        {
            _UPRDCounterParty.ClientEnvironmentsetting();

            return true;
        }
    }

}

