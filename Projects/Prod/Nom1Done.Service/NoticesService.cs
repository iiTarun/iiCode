using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Nom1Done.Model;
using Nom1Done.Data.Repositories;
using Nom1Done.Nom.ViewModel;
using Nom1Done.DTO;

namespace Nom1Done.Service
{
    public class NoticesService : INoticesService
    {
        private INoticesRepository noticesRepository;
        private IModalFactory modelfactory;       

        public NoticesService(IModalFactory modelfactory, INoticesRepository noticesRepository)
        {
            this.noticesRepository = noticesRepository;
            this.modelfactory = modelfactory;
        }

        public void AddNotice(SwntPerTransaction notice) {
            noticesRepository.Add(notice);
            noticesRepository.SaveChages();
        }

        public void UpdateNotice(SwntPerTransaction notice)
        {
            noticesRepository.Update(notice);
            noticesRepository.SaveChages();
        }
        public SwntPerTransaction GetNoticeIdnTSPCode(string NoticeId, string TSPpropCode)
        {
            return noticesRepository.GetNoticeIdnTSPCode(NoticeId,TSPpropCode);
        }

        public BONotice GetNoticeOnId(int id)
        {
            var notice = noticesRepository.GetById(id);
            return MapNoticeDetailDTO(notice);
        }


        public SwntPerTransactionDTO GetNoticeById(int id)
        {
            var notice = noticesRepository.GetById(id);
            return modelfactory.Parse(notice);
        }

        public List<BONotice> FilterNotices(string PipelineDuns, bool isCritical, DateTime startDate, DateTime endDate)
        {
            var result = noticesRepository.GetByCreatedDateRange(PipelineDuns, isCritical, startDate, endDate).OrderByDescending(a => a.PostingDateTime).ToList();
            if (result==null || result.Count == 0) {
                result = noticesRepository.GetLastFiveNotice(PipelineDuns, isCritical);
            }
            List<BONotice> noticeList = new List<BONotice>();
            foreach (var item in result)
            {
                BONotice notice = MapNoticeDetailDTO(item);
                noticeList.Add(notice);
            }
            return noticeList;
        }


       

        public string GetSubjectUsingNoticeDetails(string subject,string details)
        {
            string newSubject = string.Empty;
            if (string.IsNullOrEmpty(subject) || string.IsNullOrWhiteSpace(subject))
            {
                newSubject = new string(details.Take(25).ToArray());
                if (details.Length > 25) { newSubject += "..."; }               
            }
            else {
                return subject;
            }
            return newSubject;
        }


        private BONotice MapNoticeDetailDTO(SwntPerTransaction swnt)
        {
            BONotice notice = new BONotice();
            if (swnt != null)
            {
                notice.ID = (int)swnt.Id;
                notice.Message = swnt.Message;
                notice.subject = GetSubjectUsingNoticeDetails(swnt.Subject,swnt.Message);
                notice.IsCritical = swnt.CriticalNoticeIndicator.Trim() == "Y" ? true : false;
                notice.CreatedDate =swnt.PostingDateTime.GetValueOrDefault();
                if ((swnt.NoticeEffectiveDateTime)==null)
                {
                    notice.NoticeEffectiveDate = String.Empty;
                }
                else
                {
                   notice.NoticeEffectiveDate = swnt.NoticeEffectiveDateTime.GetValueOrDefault().ToString("MM/dd/yyyy");
                    //notice.NoticeEffectiveDate = Convert.ToDateTime(swnt.NoticeEffectiveDate.Substring(4, 2) + "/" + swnt.NoticeEffectiveDate.Substring(6, 2) + "/" + swnt.NoticeEffectiveDate.Substring(0, 4)).Date.ToString();
                }
              
                if (swnt.PostingDateTime==null)
                {
                    notice.PostingDate = String.Empty;
                }
                else
                {
                   notice.PostingDate = swnt.PostingDateTime.GetValueOrDefault().ToString("MM/dd/yyyy");
                   // notice.NoticeEndDate = Convert.ToDateTime(swnt.PostingDate.Substring(4, 2) + "/" + swnt.PostingDate.Substring(6, 2) + "/" + swnt.PostingDate.Substring(0, 4)).Date.ToString();
                }
                notice.NoticeEndDate = swnt.NoticeEndDateTime.GetValueOrDefault().ToString("MM/dd/yyyy");
                //notice.PipelineID = swnt.PipelineId;
                notice.PipelineDuns = swnt.TransportationserviceProvider;
                notice.TSP = swnt.TransportationServiceProviderPropCode + " / " + swnt.TransportationserviceProvider;
                
                notice.NoticeId = swnt.NoticeId;
                notice.NoticeStatusDescription = swnt.NoticeStatusDesc;
                notice.NoticeTypeDesc1 = swnt.NoticeTypeDesc1;
                notice.NoticeTypeDesc2 = swnt.NoticeTypeDesc2;
                notice.PriorNotice = swnt.PriorNotice;
                notice.ReqrdResp = swnt.ReqrdResp;

            }
            return notice;
        }

        

        public List<SwntPerTransactionDTO> GetNoticesBySearch(BONoticeSearchCriteria criteria)
        {
            List<SwntPerTransaction> finalResult = new List<SwntPerTransaction>();         
            // If All Search Date-Fields are Empty or Null
            if (((criteria.EffectiveStartDate == DateTime.MinValue) && (criteria.EffectiveEndDate == DateTime.MinValue)) && ((criteria.postStartDate == DateTime.MinValue) && (criteria.postEndDate == DateTime.MinValue)) &&(string.IsNullOrEmpty(criteria.Keyword)))
            {               
                var startDate = DateTime.Now.AddDays(-30); var endDate = DateTime.Now.AddHours(24);  // Dates are same as on dashboard for Notices.
                finalResult = noticesRepository.GetByCreatedDateRange(criteria.PipelineDuns, criteria.IsCritical, startDate, endDate).OrderByDescending(a => a.PostingDateTime).ToList();
               
            }
            else if (((criteria.EffectiveStartDate == DateTime.MinValue) && (criteria.EffectiveEndDate == DateTime.MinValue)) && ((criteria.postStartDate == DateTime.MinValue) && (criteria.postEndDate == DateTime.MinValue)) && (!string.IsNullOrEmpty(criteria.Keyword)))
            {
                // NOTE:-  Search only based on Description, not any date.
                finalResult = noticesRepository.GetByKeyword(criteria.PipelineDuns,criteria.IsCritical,criteria.Keyword);
            }
            else
                {
                    // No Date-Field is empty
                    if (((criteria.EffectiveStartDate != DateTime.MinValue) && (criteria.EffectiveEndDate != DateTime.MinValue)) && ((criteria.postStartDate != DateTime.MinValue) && (criteria.postEndDate != DateTime.MinValue)))
                    {
                        // result is Notice List of (Post date between Poststart-PostEnd) OR (NoticeEffectiveDate starts from EffectiveStartdate AND NoticeEndDate ends till EffectiveEndDate)
                        finalResult = noticesRepository.GetByPostEffDatesRange(criteria.PipelineDuns,criteria.IsCritical,criteria.postStartDate,criteria.postEndDate,criteria.EffectiveStartDate,criteria.EffectiveEndDate,criteria.Keyword);
                    }
                    else
                    {
                        // Only Post-Date fields in Search are empty
                        if ((criteria.EffectiveStartDate != DateTime.MinValue) && (criteria.EffectiveEndDate != DateTime.MinValue))
                        {
                            // result is list of Notices of (NoticeEffectiveDate > EffectiveStartdate) AND (NoticeEndDate < EffectiveEndDate)
                            finalResult = noticesRepository.GetNoticesByEffDateRange(criteria.PipelineDuns,criteria.IsCritical,criteria.EffectiveStartDate,criteria.EffectiveEndDate,criteria.Keyword);
                        }
                        else
                        {
                            // Only Effective-Date fields in search are empty
                            finalResult = noticesRepository.GetNoticesUsingRangeofPostDate(criteria.PipelineDuns,criteria.IsCritical,criteria.postStartDate,criteria.postEndDate,criteria.Keyword);
                        }

                    }
                }
                        
            return finalResult.Select(a=>modelfactory.Parse(a)).OrderByDescending(a => a.PostingDateTime.GetValueOrDefault()).ToList(); 
        }


        public List<SwntPerTransaction> GetRecentNotice(BONoticeSearchCriteria criteria)
        {
            List<SwntPerTransaction> swntList = new List<SwntPerTransaction>();
            var count = noticesRepository.GetTotalCount(criteria.pipelineId, criteria.IsCritical);
            if (count > 0)
            {
                var RecentDay = noticesRepository.GetRecentNoticePostDate(criteria.pipelineId, criteria.IsCritical);
                 swntList = noticesRepository.GetNoticeUsingPostDate(criteria.pipelineId, criteria.IsCritical,RecentDay);
            }
            return swntList;
        }



    }
}
