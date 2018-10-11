using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nom1Done.Nom.ViewModel;
using Nom1Done.Data.Repositories;
using Nom1Done.Model;
using Nom1Done.DTO;

namespace Nom1Done.Service
{
    public class SWNTPerTransactionService : ISWNTPerTransactionService
    {

        ISWNTPerTransactionRepository ISWNTPerTransactionRepository;
        IModalFactory modelFactory;

        public SWNTPerTransactionService(ISWNTPerTransactionRepository ISWNTPerTransactionRepository, IModalFactory modelFactory) {
            this.ISWNTPerTransactionRepository = ISWNTPerTransactionRepository;
            this.modelFactory = modelFactory;
        }

        //public IEnumerable<BONotice> GetAll()
        //{
        //    IEnumerable<SwntPerTransaction> swntPerTransList= ISWNTPerTransactionRepository.GetAll();
        //    List<BONotice> BONotices = new List<BONotice>();
        //    foreach (var item in swntPerTransList)
        //    {
        //       BONotices.Add(modelFactory.Parse(item));
        //    }

        //    return BONotices;
        //}

        public BOPipeline GetItem(int id)
        {
            throw new NotImplementedException();
        }

        public BONotice GetNoticeOnId(int id)
        {
            var notice = ISWNTPerTransactionRepository.GetById(id);
            return MapNoticeDetailDTO(notice);
        }

        private BONotice MapNoticeDetailDTO(SwntPerTransaction swnt)
        {
            BONotice notice = new BONotice();
            if (swnt != null)
            {
                notice.ID = (int)swnt.Id;
                notice.MessageID = swnt.TransactionId;
                notice.Message = swnt.Message;
                notice.subject = swnt.Subject;
                notice.IsCritical = swnt.CriticalNoticeIndicator.Trim() == "Y" ? true : false;
                notice.CreatedDate = swnt.CreatedDate.GetValueOrDefault();
                if (swnt.NoticeEffectiveDateTime==null)
                {
                    notice.NoticeEffectiveDate = String.Empty;
                }
                else
                {
                    notice.NoticeEffectiveDate = swnt.NoticeEffectiveDateTime.GetValueOrDefault().ToString("MM/dd/yyyy h:mm:ss tt");
                }
              
                if (swnt.PostingDateTime==null)
                {
                    notice.PostingDate = String.Empty;
                }
                else
                {
                    notice.PostingDate = swnt.PostingDateTime.GetValueOrDefault().ToString("MM/dd/yyyy h:mm:ss tt");
                }
                notice.NoticeEndDate = swnt.NoticeEndDateTime.GetValueOrDefault().ToString("MM/dd/yyyy h:mm:ss tt");
              
                notice.PipelineID = swnt.PipelineId;
                notice.TSP = swnt.TransportationServiceProviderPropCode + " / " + swnt.TransportationserviceProvider;
                notice.subject = swnt.Subject;               
                notice.NoticeId = swnt.NoticeId;
                notice.NoticeStatusDescription = swnt.NoticeStatusDesc;
                notice.NoticeTypeDesc1 = swnt.NoticeTypeDesc1;
                notice.NoticeTypeDesc2 = swnt.NoticeTypeDesc2;
                notice.PriorNotice = swnt.PriorNotice;
                notice.ReqrdResp = swnt.ReqrdResp;
                             
            }
            return notice;
        }

        public List<BONotice> Search(int PipelineID, BONoticeSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }

        public int SearchCount(int PipelineID, BONoticeSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }

        public List<BONotice> SearchNotices(int pipelineId, BONoticeSearchCriteria criteria)
        {
            throw new NotImplementedException();
        }
    }
}
