using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nom1Done.DTO;
using Nom1Done.Model;
using Nom1Done.Data.Repositories;

namespace Nom1Done.Service
{
    public class UNSCService : IUNSCService
    {
        IUNSCRepository _IUNSCRepository;
        IModalFactory modelFactory;

        public UNSCService(IModalFactory modelFactory,IUNSCRepository IUNSCRepository) {
            _IUNSCRepository = IUNSCRepository;
            this.modelFactory = modelFactory;
        }        


        public DateTime? GetRecentUnscPostDate(string PipelineDuns)
        {
            DateTime? recentdate = new DateTime?();
            var count = _IUNSCRepository.GetTotalCountByPipeDuns(PipelineDuns);
            if (count > 0)
            {
                recentdate = _IUNSCRepository.GetRecentPostDateUsingDuns(PipelineDuns);
            }
            else
            {
                recentdate = DateTime.Now.AddDays(-1);    // if no data found, then return Yesterday date.
            }
            return recentdate;
        }



        public List<UnscPerTransactionDTO> GetRecentUnsc(Search criteria)
        {
            List<UnscPerTransactionDTO> oacyList = new List<UnscPerTransactionDTO>();
            var count = _IUNSCRepository.GetTotalCountByPipeDuns(criteria.PipelineDuns);
            if (count > 0)
            {
                DateTime? recentdate = _IUNSCRepository.GetRecentPostDateUsingDuns(criteria.PipelineDuns);
                oacyList = _IUNSCRepository.GetRecentUnscByPostDate(criteria.PipelineDuns,recentdate).Select(a=>modelFactory.Parse(a)).ToList();
            }
            return oacyList;
        }


        public List<UnscPerTransactionDTO> GetAllUNSCOnPipelineId(Search criteria)
        {
           
            var result = new List<UnscPerTransactionDTO>();          
            DateTime minDate = DateTime.MinValue;              
                if ((criteria.EffectiveStartDate == minDate) && (criteria.EffectiveEndDate == minDate) && (criteria.postStartDate == minDate) && (string.IsNullOrEmpty(criteria.keyword)))
                {
                    result = GetRecentUnsc(criteria);
                }else if ((criteria.EffectiveStartDate == minDate) && (criteria.EffectiveEndDate == minDate) && (criteria.postStartDate == minDate) && (!string.IsNullOrEmpty(criteria.keyword)))
                  {
                   result = _IUNSCRepository.GetByKeyword(criteria.PipelineDuns,criteria.keyword).Select(a => modelFactory.Parse(a)).ToList();
            }
            else if ((criteria.EffectiveStartDate != minDate) && (criteria.EffectiveEndDate != minDate) && (criteria.postStartDate != minDate) && (string.IsNullOrEmpty(criteria.keyword))) {
                result = _IUNSCRepository.GetByPostDateStartEffEndEffDate(criteria.PipelineDuns,criteria.postStartDate,criteria.EffectiveStartDate,criteria.EffectiveEndDate).Select(a => modelFactory.Parse(a)).ToList();
            }
            else if ((criteria.EffectiveStartDate != minDate) && (criteria.EffectiveEndDate != minDate) && (criteria.postStartDate != minDate) && (!string.IsNullOrEmpty(criteria.keyword))) {
                result = _IUNSCRepository.GetByPostDateStartEffEndEffDateKeyword(criteria.PipelineDuns, criteria.postStartDate, criteria.EffectiveStartDate, criteria.EffectiveEndDate,criteria.keyword).Select(a => modelFactory.Parse(a)).ToList();
            }
            else if ((criteria.EffectiveStartDate != minDate) && (criteria.EffectiveEndDate != minDate) && (criteria.postStartDate == minDate) && ( string.IsNullOrEmpty(criteria.keyword))) {
                result = _IUNSCRepository.GetByStartEffDateEndEffDate(criteria.PipelineDuns, criteria.EffectiveStartDate, criteria.EffectiveEndDate).Select(a => modelFactory.Parse(a)).ToList();
            }
            else if ((criteria.EffectiveStartDate != minDate) && (criteria.EffectiveEndDate != minDate) && (criteria.postStartDate == minDate) && (!string.IsNullOrEmpty(criteria.keyword))) {
                result = _IUNSCRepository.GetByStartEffDateEndEffDateKeyword(criteria.PipelineDuns, criteria.EffectiveStartDate, criteria.EffectiveEndDate, criteria.keyword).Select(a => modelFactory.Parse(a)).ToList();
            }
            else if ((criteria.EffectiveStartDate != minDate) && (criteria.EffectiveEndDate == minDate) && (criteria.postStartDate != minDate) && (string.IsNullOrEmpty(criteria.keyword))) {
                result = _IUNSCRepository.GetByPostDateStartEffDate(criteria.PipelineDuns, criteria.postStartDate, criteria.EffectiveStartDate).Select(a => modelFactory.Parse(a)).ToList();
            }
            else if ((criteria.EffectiveStartDate != minDate) && (criteria.EffectiveEndDate == minDate) && (criteria.postStartDate != minDate) && (!string.IsNullOrEmpty(criteria.keyword))) {
                result = _IUNSCRepository.GetByPostDateStartEffDateKeyword(criteria.PipelineDuns, criteria.postStartDate, criteria.EffectiveStartDate, criteria.keyword).Select(a => modelFactory.Parse(a)).ToList();
            }
            else if ((criteria.EffectiveStartDate == minDate) && (criteria.EffectiveEndDate != minDate) && (criteria.postStartDate != minDate) && (string.IsNullOrEmpty(criteria.keyword))) {
                result = _IUNSCRepository.GetByPostDateEndEffDate(criteria.PipelineDuns, criteria.postStartDate, criteria.EffectiveEndDate).Select(a => modelFactory.Parse(a)).ToList();
            }
            else if ((criteria.EffectiveStartDate == minDate) && (criteria.EffectiveEndDate != minDate) && (criteria.postStartDate != minDate) && (!string.IsNullOrEmpty(criteria.keyword))) {
                result = _IUNSCRepository.GetByPostDateEndEffDateKeyword(criteria.PipelineDuns, criteria.postStartDate, criteria.EffectiveEndDate, criteria.keyword).Select(a => modelFactory.Parse(a)).ToList();
            }
            else if ((criteria.EffectiveStartDate == minDate) && (criteria.EffectiveEndDate == minDate) && (criteria.postStartDate != minDate) && (string.IsNullOrEmpty(criteria.keyword))) {
                result = _IUNSCRepository.GetByPostDate(criteria.PipelineDuns, criteria.postStartDate).Select(a => modelFactory.Parse(a)).ToList();
            }
            else if ((criteria.EffectiveStartDate == minDate) && (criteria.EffectiveEndDate == minDate) && (criteria.postStartDate != minDate) && (!string.IsNullOrEmpty(criteria.keyword))) {
                result = _IUNSCRepository.GetByPostDateKeyword(criteria.PipelineDuns, criteria.postStartDate, criteria.keyword).Select(a => modelFactory.Parse(a)).ToList();
            }
            else if ((criteria.EffectiveStartDate == minDate) && (criteria.EffectiveEndDate != minDate) && (criteria.postStartDate == minDate) && (string.IsNullOrEmpty(criteria.keyword))) {
                result = _IUNSCRepository.GetByEndEffDate(criteria.PipelineDuns, criteria.EffectiveEndDate).Select(a => modelFactory.Parse(a)).ToList();
            }
            else if ((criteria.EffectiveStartDate == minDate) && (criteria.EffectiveEndDate != minDate) && (criteria.postStartDate == minDate) && (!string.IsNullOrEmpty(criteria.keyword))) {
                result = _IUNSCRepository.GetByEndEffDateKeyword(criteria.PipelineDuns, criteria.EffectiveEndDate, criteria.keyword).Select(a => modelFactory.Parse(a)).ToList();

            }
            else if ((criteria.EffectiveStartDate != minDate) && (criteria.EffectiveEndDate == minDate) && (criteria.postStartDate == minDate) && (string.IsNullOrEmpty(criteria.keyword))) {
                result = _IUNSCRepository.GetByStartEffDate(criteria.PipelineDuns,  criteria.EffectiveStartDate).Select(a => modelFactory.Parse(a)).ToList();
            }
            else if ((criteria.EffectiveStartDate != minDate) && (criteria.EffectiveEndDate == minDate) && (criteria.postStartDate == minDate) && (!string.IsNullOrEmpty(criteria.keyword))) {
                result = _IUNSCRepository.GetByStartEffDateKeyword(criteria.PipelineDuns, criteria.EffectiveStartDate, criteria.keyword).Select(a => modelFactory.Parse(a)).ToList();
            }
            return result;
        }
    }
}
