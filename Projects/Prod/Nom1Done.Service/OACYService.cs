using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nom1Done.Model;
using Nom1Done.Data.Repositories;
using Nom1Done.DTO;

namespace Nom1Done.Service
{
   public class OACYService : IOACYService
    {
        IOACYRepository _IOACYRepository;
        IModalFactory modelFactory;
        public OACYService(IModalFactory modelFactory,IOACYRepository IOACYRepository)
        {
            this.modelFactory = modelFactory;
            _IOACYRepository = IOACYRepository;
        }

        public bool AddData(List<OACYPerTransaction> list)
        {
            foreach (var item in list) {
                _IOACYRepository.Add(item);
                _IOACYRepository.Save();
            }
          
            return true;
           
        }
        

        public DateTime? GetRecentOacyPostDate(string PipelineDuns)
        {
            DateTime? recentdate = new DateTime?();
            var count = _IOACYRepository.GetOACYListTotalCount(PipelineDuns);
            if (count > 0)
            {
                recentdate = _IOACYRepository.GetRecentPostDateUsngDuns(PipelineDuns);
            }
            else {
                recentdate = DateTime.Now.AddDays(-1);    // if no data found, then return Yesterday date.
            }
            return recentdate;
        }
        

        public List<OACYPerTransactionDTO> GetRecentOacy(Search criteria)
        {
            List<OACYPerTransactionDTO> oacyList = new List<OACYPerTransactionDTO>();           
            var count = _IOACYRepository.GetOACYListTotalCount(criteria.PipelineDuns);
            if (count > 0)
            {
                DateTime? recentdate = _IOACYRepository.GetRecentPostDateUsngDuns(criteria.PipelineDuns);
                oacyList = _IOACYRepository.GetByPostDateEffDate(criteria.PipelineDuns, recentdate, DateTime.MinValue, string.Empty, string.Empty).Select(a => modelFactory.Parse(a)).ToList();
            }
            return oacyList;
        }


        public List<OACYPerTransactionDTO> GetAllOacyOnPipelineId(Search criteria)
        {  
            DateTime minDate = DateTime.MinValue;
            List<OACYPerTransactionDTO> result = new List<OACYPerTransactionDTO>();
            result = _IOACYRepository.GetByPostDateEffDate(criteria.PipelineDuns, criteria.postStartDate, criteria.EffectiveStartDate, criteria.keyword, criteria.Cycle)
                .Select(a => modelFactory.Parse(a)).ToList();
            return result;
        }


    }
}
