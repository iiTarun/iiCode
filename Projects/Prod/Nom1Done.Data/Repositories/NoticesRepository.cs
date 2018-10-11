using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Nom1Done.Data.Repositories
{
    public class NoticesRepository : RepositoryBase<SwntPerTransaction>,INoticesRepository
    {
        public NoticesRepository(IDbFactory dbfactory):base(dbfactory)
        {

        }
        public void SaveChages()
        {           
            this.DbContext.SaveChanges();
        }

        public SwntPerTransaction GetNoticeIdnTSPCode(string NoticeId,string TSPpropCode)
        {
            return DbContext.SwntPerTransaction.Where(a=>a.NoticeId==NoticeId && a.TransportationServiceProviderPropCode==TSPpropCode).FirstOrDefault();
         }

        public int GetTotalCount(int pipelineId,bool isCritical) {
           return  DbContext.SwntPerTransaction.Where(a => (a.PipelineId == pipelineId)
                             && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                             && a.IsActive == true
                             ).Count();
        }

        public DateTime? GetRecentNoticePostDate(int pipelineId, bool isCritical)
        {
            return DbContext.SwntPerTransaction.Where(a => (a.PipelineId == pipelineId)
                              && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                              && a.IsActive == true
                              ).Select(a => a.PostingDateTime).OrderByDescending(a => a).FirstOrDefault();
        }

        public List<SwntPerTransaction> GetNoticeUsingPostDate(int pipelineId, bool isCritical, DateTime? postdate)
        {
            var date = postdate.GetValueOrDefault().Date;
            return DbContext.SwntPerTransaction.Where(a => (a.PipelineId == pipelineId)
                               && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                               && a.IsActive == true
                               && (DbFunctions.TruncateTime(a.PostingDateTime)) == date
                               ).ToList();
        }


        public List<SwntPerTransaction> GetNoticesUsingRangeofPostDate(string PipelineDuns, bool isCritical, DateTime? startPostdate,DateTime? endPostDate , String Keyword)
        {
            var spostdate = startPostdate.GetValueOrDefault().Date;
            var epostdate = endPostDate.GetValueOrDefault().Date;
            if (string.IsNullOrEmpty(Keyword))
            {
                return DbContext.SwntPerTransaction.Where(a => a.TransportationserviceProvider == PipelineDuns
                                    && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                                    && a.IsActive == true
                                    && ((DbFunctions.TruncateTime(a.PostingDateTime) >= spostdate) && (DbFunctions.TruncateTime(a.PostingDateTime) <= epostdate))
                                   ).ToList();
            }
            else {
                return DbContext.SwntPerTransaction.Where(a => a.TransportationserviceProvider == PipelineDuns
                                   && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                                   && a.IsActive == true
                                   && ((DbFunctions.TruncateTime(a.PostingDateTime) >= spostdate) && (DbFunctions.TruncateTime(a.PostingDateTime) <= epostdate))
                                   && ((a.Subject ?? "").Contains(Keyword) || ((a.Message ?? "").Contains(Keyword)))
                                   ).ToList();
            }
        }


        public List<SwntPerTransaction> GetNoticesByEffDateRange(string PipelineDuns, bool isCritical, DateTime? startEffDate,DateTime? endEffDate, string keyword )
        {
            var sEffDate = startEffDate.GetValueOrDefault().Date;
            var eEffDate = endEffDate.GetValueOrDefault().Date;
            if (string.IsNullOrEmpty(keyword))
            {
                return DbContext.SwntPerTransaction.Where(a => a.TransportationserviceProvider == PipelineDuns
                                   && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                                   && a.IsActive == true
                                   && ((DbFunctions.TruncateTime(a.NoticeEffectiveDateTime) >= sEffDate) && (DbFunctions.TruncateTime(a.NoticeEndDateTime) <= eEffDate))                                  
                                   ).ToList();
            }
            else {
                return DbContext.SwntPerTransaction.Where(a => a.TransportationserviceProvider == PipelineDuns
                                    && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                                    && a.IsActive == true
                                    && ((DbFunctions.TruncateTime(a.NoticeEffectiveDateTime) >= sEffDate) && (DbFunctions.TruncateTime(a.NoticeEndDateTime) <= eEffDate))
                                    && ((a.Subject ?? "").Contains(keyword) || ((a.Message ?? "").Contains(keyword)))
                                    ).ToList();
            }
        }

        public List<SwntPerTransaction> GetByPostEffDatesRange(string PipelineDuns, bool isCritical,DateTime? startPostDate,DateTime? endPostDate, DateTime? startEffDate, DateTime? endEffDate, string keyword)
        {
            var sEffDate = startEffDate.GetValueOrDefault().Date;
            var eEffDate = endEffDate.GetValueOrDefault().Date;
            var spostdate = startPostDate.GetValueOrDefault().Date;
            var epostdate = endPostDate.GetValueOrDefault().Date;

            if (string.IsNullOrEmpty(keyword))
            {
                return DbContext.SwntPerTransaction.Where(a => a.TransportationserviceProvider==PipelineDuns
                                 && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                                 && a.IsActive == true
                                 && (((DbFunctions.TruncateTime(a.PostingDateTime) >= spostdate) && (DbFunctions.TruncateTime(a.PostingDateTime) <= epostdate)))
                                 && (((DbFunctions.TruncateTime(a.NoticeEffectiveDateTime) >= sEffDate) && (DbFunctions.TruncateTime(a.NoticeEndDateTime) <= eEffDate)))
                                 ).ToList();
                                   
            }
            else {
                return DbContext.SwntPerTransaction.Where(a => a.TransportationserviceProvider == PipelineDuns
                                 && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                                 && a.IsActive == true
                                 && (((DbFunctions.TruncateTime(a.PostingDateTime) >= spostdate) && (DbFunctions.TruncateTime(a.PostingDateTime) <= epostdate)))
                                 && (((DbFunctions.TruncateTime(a.NoticeEffectiveDateTime) >= sEffDate) && (DbFunctions.TruncateTime(a.NoticeEndDateTime) <= eEffDate)))
                                 && ((a.Subject ?? "").Contains(keyword) || ((a.Message ?? "").Contains(keyword)))
                                 ).ToList();
            }
        }


        public List<SwntPerTransaction> GetByKeyword(string PipelineDuns,bool isCritical, string keyword)
        {
            return DbContext.SwntPerTransaction.Where(a => a.TransportationserviceProvider == PipelineDuns
                                  && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                                  && a.IsActive == true
                                  && ((a.Subject ?? "").Contains(keyword) || ((a.Message ?? "").Contains(keyword)))
                                  ).ToList();
        }

        public List<SwntPerTransaction> GetByCreatedDateRange(string pipelineDuns,bool isCritical, DateTime? startCreateddate, DateTime? endCreatedDate)
        {
            var sDate = startCreateddate.GetValueOrDefault().Date;
            var eDate = endCreatedDate.GetValueOrDefault().Date;

            return DbContext.SwntPerTransaction.Where(a => a.TransportationserviceProvider == pipelineDuns
                     && (DbFunctions.TruncateTime(a.CreatedDate)) >= sDate
                     && (DbFunctions.TruncateTime(a.CreatedDate)) <= eDate
                     && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                     && a.IsActive == true
                   ).ToList();
        }

        public List<SwntPerTransaction> GetLastFiveNotice(string pipelineDuns, bool isCritical)
        {          

            return DbContext.SwntPerTransaction.Where(a => a.TransportationserviceProvider == pipelineDuns                 
                     && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                     && a.IsActive == true
                   ).OrderByDescending(a=>a.PostingDateTime).Take(5).ToList();
        }




        public IQueryable<SwntPerTransaction> GetNotice(string pipeDuns, bool isCritical)
        {
            DateTime? recentDate = GetRecentPostingDate();
            if (recentDate != null) {
                return DbContext.SwntPerTransaction.Where(a=>a.TransportationserviceProvider== pipeDuns
                                   && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                                  && a.IsActive == true
                                  && DbFunctions.TruncateTime(a.PostingDateTime)==DbFunctions.TruncateTime(recentDate));
            }
            else {
                return DbContext.SwntPerTransaction.Where(a => a.TransportationserviceProvider == pipeDuns
                                  && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                                 && a.IsActive == true);
            }
        }


        public IQueryable<SwntPerTransaction> GetAllNoticeForPipe(string pipeDuns, bool isCritical)
        {  
            return DbContext.SwntPerTransaction.Where(a => a.TransportationserviceProvider == pipeDuns
             && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
             && a.IsActive == true);

        }


        public DateTime? GetRecentPostingDate() { 
            return DbContext.SwntPerTransaction.Select(a => a.PostingDateTime).OrderByDescending(a => a).FirstOrDefault();
        }

    }

    public interface INoticesRepository:IRepository<SwntPerTransaction>
    {
        IQueryable<SwntPerTransaction> GetAllNoticeForPipe(string pipeDuns, bool isCritical);
        SwntPerTransaction GetNoticeIdnTSPCode(string NoticeId, string TSPpropCode);
       List<SwntPerTransaction> GetNoticesUsingRangeofPostDate(string PipelineDuns, bool isCritical, DateTime? startPostdate, DateTime? endPostDate, String Keyword);
       DateTime? GetRecentNoticePostDate(int pipelineId, bool isCritical);
       int GetTotalCount(int pipelineId, bool isCritical);
       void SaveChages();
        List<SwntPerTransaction> GetByKeyword(string PipelineDuns, bool isCritical, string keyword);
        List<SwntPerTransaction> GetNoticeUsingPostDate(int pipelineId, bool isCritical, DateTime? postdate);

        List<SwntPerTransaction> GetByPostEffDatesRange(string PipelineDuns, bool isCritical, DateTime? startPostDate, DateTime? endPostDate, DateTime? startEffDate, DateTime? endEffDate, string keyword);
        List<SwntPerTransaction> GetNoticesByEffDateRange(string PipelineDuns, bool isCritical, DateTime? startEffDate, DateTime? endEffDate, string keyword);
        IQueryable<SwntPerTransaction> GetNotice(string pipeDuns, bool isCritical);

        List<SwntPerTransaction> GetLastFiveNotice(string pipelineDuns, bool isCritical);
        List<SwntPerTransaction> GetByCreatedDateRange(string pipelineDuns, bool isCritical, DateTime? startCreateddate, DateTime? endCreatedDate);
    }
}
