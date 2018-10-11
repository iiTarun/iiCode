using CentralisedUprd.Api.Helpers;
using CentralisedUprd.Api.Models;
using CentralisedUprd.Api.UPRD.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CentralisedUprd.Api.Repositories
{
    public class UprdSwntRepository
    {
       public UprdDbEntities1 dbcontext;
       public ModalFactory modalFactory;

        public UprdSwntRepository() {
            dbcontext = new UprdDbEntities1();
            modalFactory = new ModalFactory();
        }


        public SwntResultDTO GetSwntListWithPaging(string PipelineDuns, bool IsCritical, string Keyword,  DateTime? postStartDate,DateTime? postEndDate, DateTime? EffectiveStartDate, DateTime? EffectiveEndDate, SortingPagingInfo sortingPagingInfo)
        {


            //Get SWNT list using above parameters     
            List<SwntPerTransaction> swntdatalist = new List<SwntPerTransaction>();
            SwntResultDTO swntResultDTO = new SwntResultDTO();
           
            try
            {

                swntdatalist = GetNoticesBySearch(PipelineDuns, IsCritical, Keyword, postStartDate, postEndDate, EffectiveStartDate, EffectiveEndDate);

                var dataQueryWithOrder = SortByColumnWithOrder(swntdatalist.AsQueryable(), sortingPagingInfo);             
                sortingPagingInfo.PageCount = dataQueryWithOrder.Count();
                swntResultDTO.RecordCount= sortingPagingInfo.PageCount;
                  var resultData = dataQueryWithOrder.Skip((sortingPagingInfo.CurrentPageIndex - 1) * sortingPagingInfo.PageSize).Take(sortingPagingInfo.PageSize).ToList();
                        
                 if (resultData.Count() > 0)
                 {
                    var swntResult = resultData.Select(a => modalFactory.Parse(a)).ToList();
                    swntResultDTO.swntPerTransactionDTO = swntResult;
                    return swntResultDTO;
                }

                return new SwntResultDTO();            
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int GetSwntListTotalCount(string PipelineDuns, bool IsCritical, string Keyword, DateTime? postStartDate, DateTime? postEndDate, DateTime? EffectiveStartDate, DateTime? EffectiveEndDate)
        {


            //Get SWNT list using above parameters     
            List<SwntPerTransaction> swntdatalist = new List<SwntPerTransaction>();

            try
            {

                swntdatalist = GetNoticesBySearch(PipelineDuns, IsCritical, Keyword, postStartDate, postEndDate, EffectiveStartDate, EffectiveEndDate);

                return swntdatalist.Count();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }




        #region Sort by column with order method

        /// <summary>
        /// Sort by column with order method.
        /// </summary>
        /// <param name="order">Order parameter</param>
        /// <param name="orderDir">Order direction parameter</param>
        /// <param name="data">Data parameter</param>
        /// <returns>Returns - Data</returns>
        public IQueryable<SwntPerTransaction> SortByColumnWithOrder(IQueryable<SwntPerTransaction> dataQuery, Helpers.SortingPagingInfo sortingPagingInfo)
        {
            if (sortingPagingInfo != null)
            {
                // Sorting
                string orderDir = sortingPagingInfo.SortDirection;
                switch (sortingPagingInfo.SortField)
                {
                    case "NoticeTypeDesc1":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.NoticeTypeDesc1)
                                                                                                 : dataQuery.OrderBy(p => p.NoticeTypeDesc1);
                        break;
                    case "NoticeId":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.NoticeId)
                                                                                                 : dataQuery.OrderBy(p => p.NoticeId);
                        break;
                    case "PostingDateTime":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p=>p.PostingDateTime)
                                                                                                 : dataQuery.OrderBy(p => p.PostingDateTime);
                        break;

                    case "NoticeEffectiveDateTime":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.NoticeEffectiveDateTime)
                                                                                                 : dataQuery.OrderBy(p => p.NoticeEffectiveDateTime);
                        break;

                    case "NoticeEndDateTime":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.NoticeEndDateTime)
                                                                                                 : dataQuery.OrderBy(p => p.NoticeEndDateTime);
                        break;

                    case "Subject":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.Subject)
                                                                                                   : dataQuery.OrderBy(p => p.Subject);
                        break;

                    default:
                        dataQuery = dataQuery.OrderByDescending(p => p.PostingDateTime);

                        break;

                }
            }
            else
            {
                return dataQuery;
            }

            return dataQuery;
        }

        #endregion
        #region Methods On Criteria Basis 

        public IQueryable<SwntPerTransaction> GetAllNoticeForPipe(string pipeDuns, bool isCritical)
        {
            return dbcontext.SwntPerTransactions.Where(a => a.TransportationserviceProvider == pipeDuns
             && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
             && a.IsActive == true);

        }
        public IQueryable<SwntPerTransaction> GetNotice(string pipeDuns, bool isCritical)
        {
            DateTime? recentDate = GetRecentPostingDate();
            if (recentDate != null)
            {
                return dbcontext.SwntPerTransactions.Where(a => a.TransportationserviceProvider == pipeDuns
                                   && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                                  && a.IsActive == true
                                  && DbFunctions.TruncateTime(a.CreatedDate) == DbFunctions.TruncateTime(recentDate));
            }
            else
            {
                return dbcontext.SwntPerTransactions.Where(a => a.TransportationserviceProvider == pipeDuns
                                  && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                                 && a.IsActive == true);
            }
        }

        public DateTime? GetRecentPostingDate()
        {
            return dbcontext.SwntPerTransactions.Select(a => a.CreatedDate).OrderByDescending(a => a).FirstOrDefault();
        }

        public IQueryable<SwntPerTransaction> GetDataBycriteria(string DunsNo, bool isCritical, string keyword, DateTime? pStartDate, DateTime? pEndDate, DateTime? eStartDate, DateTime? eEndDate)
        {
            return dbcontext.SwntPerTransactions.Where(a => a.TransportationserviceProvider == DunsNo
                                  && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                                  && a.IsActive == true
                                  && (((a.Message ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Subject ?? "").ToLower().Contains(keyword.ToLower())))
                                  && DbFunctions.TruncateTime(a.PostingDateTime) >= pStartDate
                                  && DbFunctions.TruncateTime(a.PostingDateTime) <= pEndDate
                                  && DbFunctions.TruncateTime(a.NoticeEffectiveDateTime) == eStartDate
                                  && DbFunctions.TruncateTime(a.NoticeEndDateTime) == eEndDate);

        }

        public List<SwntPerTransaction> GetNoticesByEffDateRange(string PipelineDuns, bool isCritical, DateTime? startEffDate, DateTime? endEffDate, string keyword)
        {

            if (string.IsNullOrEmpty(keyword))
            {
                return dbcontext.SwntPerTransactions.Where(a => a.TransportationserviceProvider == PipelineDuns
                                   && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                                   && a.IsActive == true
                                   && ((DbFunctions.TruncateTime(a.NoticeEffectiveDateTime) >= startEffDate) && (DbFunctions.TruncateTime(a.NoticeEndDateTime) <= endEffDate))
                                   ).ToList();
            }
            else
            {
                return dbcontext.SwntPerTransactions.Where(a => a.TransportationserviceProvider == PipelineDuns
                                    && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                                    && a.IsActive == true
                                    && ((DbFunctions.TruncateTime(a.NoticeEffectiveDateTime) >= startEffDate) && (DbFunctions.TruncateTime(a.NoticeEndDateTime) <= endEffDate))
                                    && ((a.Subject ?? "").Contains(keyword) || ((a.Message ?? "").Contains(keyword)))
                                    ).ToList();
            }
        }

        public List<SwntPerTransaction> GetByCreatedDateRange(string pipelineDuns, bool isCritical, DateTime? startCreateddate, DateTime? endCreatedDate)
        {
            var sDate = startCreateddate.GetValueOrDefault().Date;
            var eDate = endCreatedDate.GetValueOrDefault().Date;

            return dbcontext.SwntPerTransactions.Where(a => a.TransportationserviceProvider == pipelineDuns
                     && (DbFunctions.TruncateTime(a.CreatedDate)) >= sDate
                     && (DbFunctions.TruncateTime(a.CreatedDate)) <= eDate
                     && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                     && a.IsActive == true
                   ).OrderByDescending(a => a.PostingDateTime).ToList();
        }
       

        public List<SwntPerTransaction> GetByKeyword(string PipelineDuns, bool isCritical, string keyword)
        {
            return dbcontext.SwntPerTransactions.Where(a => a.TransportationserviceProvider == PipelineDuns
                                  && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                                  && a.IsActive == true
                                  && ((a.Subject ?? "").Contains(keyword) || ((a.Message ?? "").Contains(keyword)))
                                  ).ToList();
        }
        public List<SwntPerTransaction> GetByPostEffDatesRange(string PipelineDuns, bool isCritical, DateTime? startPostDate, DateTime? endPostDate, DateTime? startEffDate, DateTime? endEffDate, string keyword)
        {
            var sEffDate = startEffDate.GetValueOrDefault().Date;
            var eEffDate = endEffDate.GetValueOrDefault().Date;
            var spostdate = startPostDate.GetValueOrDefault().Date;
            var epostdate = endPostDate.GetValueOrDefault().Date;

            if (string.IsNullOrEmpty(keyword))
            {
                return dbcontext.SwntPerTransactions.Where(a => a.TransportationserviceProvider == PipelineDuns
                                 && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                                 && a.IsActive == true
                                 && (((DbFunctions.TruncateTime(a.PostingDateTime) >= spostdate) && (DbFunctions.TruncateTime(a.PostingDateTime) <= epostdate)))
                                 && (((DbFunctions.TruncateTime(a.NoticeEffectiveDateTime) >= sEffDate) && (DbFunctions.TruncateTime(a.NoticeEndDateTime) <= eEffDate)))
                                 ).ToList();

            }
            else
            {
                return dbcontext.SwntPerTransactions.Where(a => a.TransportationserviceProvider == PipelineDuns
                                 && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                                 && a.IsActive == true
                                 && (((DbFunctions.TruncateTime(a.PostingDateTime) >= spostdate) && (DbFunctions.TruncateTime(a.PostingDateTime) <= epostdate)))
                                 && (((DbFunctions.TruncateTime(a.NoticeEffectiveDateTime) >= sEffDate) && (DbFunctions.TruncateTime(a.NoticeEndDateTime) <= eEffDate)))
                                 && ((a.Subject ?? "").Contains(keyword) || ((a.Message ?? "").Contains(keyword)))
                                 ).ToList();
            }
        }
        public List<SwntPerTransaction> GetNoticesUsingRangeofPostDate(string PipelineDuns, bool isCritical, DateTime? startPostdate, DateTime? endPostDate, String Keyword)
        {
            var spostdate = startPostdate.GetValueOrDefault().Date;
            var epostdate = endPostDate.GetValueOrDefault().Date;
            if (string.IsNullOrEmpty(Keyword))
            {
                return dbcontext.SwntPerTransactions.Where(a => a.TransportationserviceProvider == PipelineDuns
                                    && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                                    && a.IsActive == true
                                    && ((DbFunctions.TruncateTime(a.PostingDateTime) >= spostdate) && (DbFunctions.TruncateTime(a.PostingDateTime) <= epostdate))
                                   ).ToList();
            }
            else
            {
                return dbcontext.SwntPerTransactions.Where(a => a.TransportationserviceProvider == PipelineDuns
                                   && (isCritical ? a.CriticalNoticeIndicator == "Y" : a.CriticalNoticeIndicator != "Y")
                                   && a.IsActive == true
                                   && ((DbFunctions.TruncateTime(a.PostingDateTime) >= spostdate) && (DbFunctions.TruncateTime(a.PostingDateTime) <= epostdate))
                                   && ((a.Subject ?? "").Contains(Keyword) || ((a.Message ?? "").Contains(Keyword)))
                                   ).ToList();
            }
        }

        public string GetSubjectUsingNoticeDetails(string subject, string details)
        {
            string newSubject = string.Empty;
            if (string.IsNullOrEmpty(subject) || string.IsNullOrWhiteSpace(subject))
            {
                newSubject = new string(details.Take(25).ToArray());
                if (details.Length > 25) { newSubject += "..."; }
            }
            else
            {
                return subject;
            }
            return newSubject;
        }

        public BONotice MappingNotice(SwntPerTransaction swnt)
        {
            BONotice notice = new BONotice();
            if (swnt != null)
            {
                notice.ID = (int)swnt.Id;
                notice.Message = swnt.Message;
                notice.subject = GetSubjectUsingNoticeDetails(swnt.Subject, swnt.Message);
                notice.IsCritical = swnt.CriticalNoticeIndicator.Trim() == "Y" ? true : false;
                notice.CreatedDate = swnt.PostingDateTime.GetValueOrDefault();
                if ((swnt.NoticeEffectiveDateTime) == null)
                {
                    notice.NoticeEffectiveDate = String.Empty;
                }
                else
                {
                    notice.NoticeEffectiveDate = swnt.NoticeEffectiveDateTime.GetValueOrDefault().ToString("MM/dd/yyyy");
                    //notice.NoticeEffectiveDate = Convert.ToDateTime(swnt.NoticeEffectiveDate.Substring(4, 2) + "/" + swnt.NoticeEffectiveDate.Substring(6, 2) + "/" + swnt.NoticeEffectiveDate.Substring(0, 4)).Date.ToString();
                }

                if (swnt.PostingDateTime == null)
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

        public BONotice GetNoticeById(int id)
        {

            SwntPerTransaction swnt = dbcontext.SwntPerTransactions.Where(a=>a.Id==id).FirstOrDefault();
            var notice=  MappingNotice(swnt);
            return notice;
        }

        

        public List<SwntPerTransaction> GetNoticesBySearch(string PipelineDuns, bool IsCritical, string Keyword, DateTime? postStartDate, DateTime? postEndDate, DateTime? EffectiveStartDate, DateTime? EffectiveEndDate)
        {
            List<SwntPerTransaction> finalResult = new List<SwntPerTransaction>();


            var pStartDate = postStartDate.GetValueOrDefault().Date;
            var pEndDate = postEndDate.GetValueOrDefault().Date;
            var eStartDate = EffectiveStartDate.GetValueOrDefault().Date;
            var eEndDate = EffectiveEndDate.GetValueOrDefault().Date;

            // If All Search Date-Fields are Empty or Null
            if (((eStartDate == DateTime.MinValue) && (eEndDate == DateTime.MinValue)) && ((pStartDate == DateTime.MinValue) && (pEndDate == DateTime.MinValue)) && (string.IsNullOrEmpty(Keyword)))
            {
                var startDate = DateTime.Now.AddDays(-30); var endDate = DateTime.Now.AddHours(24);  // Dates are same as on dashboard for Notices.
                finalResult = GetByCreatedDateRange(PipelineDuns, IsCritical, startDate, endDate).OrderByDescending(a => a.PostingDateTime).ToList();

            }
            else if (((eStartDate == DateTime.MinValue) && (eEndDate == DateTime.MinValue)) && ((pStartDate == DateTime.MinValue) && (pEndDate == DateTime.MinValue)) && (!string.IsNullOrEmpty(Keyword)))
            {
                // NOTE:-  Search only based on Description, not any date.
                finalResult = GetByKeyword(PipelineDuns, IsCritical, Keyword);
            }
            else
            {
                // No Date-Field is empty
                if (((eStartDate != DateTime.MinValue) && (eEndDate != DateTime.MinValue)) && ((pStartDate != DateTime.MinValue) && (pEndDate != DateTime.MinValue)))
                {
                    // result is Notice List of (Post date between Poststart-PostEnd) OR (NoticeEffectiveDate starts from EffectiveStartdate AND NoticeEndDate ends till EffectiveEndDate)
                    finalResult = GetByPostEffDatesRange(PipelineDuns, IsCritical, pStartDate, pEndDate, eStartDate, eEndDate, Keyword);
                }
                else
                {
                    // Only Post-Date fields in Search are empty
                    if ((eStartDate != DateTime.MinValue) && (eEndDate != DateTime.MinValue))
                    {
                        // result is list of Notices of (NoticeEffectiveDate > EffectiveStartdate) AND (NoticeEndDate < EffectiveEndDate)
                        finalResult = GetNoticesByEffDateRange(PipelineDuns, IsCritical, eStartDate, eEndDate, Keyword);
                    }
                    else
                    {
                        // Only Effective-Date fields in search are empty
                        finalResult = GetNoticesUsingRangeofPostDate(PipelineDuns, IsCritical, pStartDate, pEndDate, Keyword);
                    }

                }
            }

            return finalResult.OrderByDescending(a => a.PostingDateTime.GetValueOrDefault()).ToList();

        }


        #endregion


      


    }
}