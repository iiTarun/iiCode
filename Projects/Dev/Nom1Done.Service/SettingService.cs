using Nom1Done.Data.Repositories;
using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nom1Done.DTO;
using Nom1Done.Model;

namespace Nom1Done.Service
{
    public class SettingService:ISettingService
    {
        private readonly ISettingRepository settingRepository;
        private readonly IModalFactory modelFactory;
        public SettingService(ISettingRepository settingRepository, IModalFactory modelFactory)
        {
            this.settingRepository = settingRepository;
            this.modelFactory = modelFactory;
        }

        public IEnumerable<SettingDTO> GetAll()
        {
            return settingRepository.GetAll().Select(a=>modelFactory.Parse(a));
        }

        public SettingDTO GetById(int Id)
        {
            return modelFactory.Parse(settingRepository.GetById(Id));
        }
    }
}
