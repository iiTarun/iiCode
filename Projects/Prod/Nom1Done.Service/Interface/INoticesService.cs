using Nom1Done.DTO;
using Nom1Done.Model;
using Nom1Done.Nom.ViewModel;
using System;
using System.Collections.Generic;

namespace Nom1Done.Service.Interface
{
    public interface INoticesService
    {
        BONotice GetNoticeOnId(int id);
        SwntPerTransaction GetNoticeIdnTSPCode(string NoticeId, string TSPpropCode);
        List<BONotice> FilterNotices(string PipelineDuns, bool isCritical, DateTime startDate, DateTime endDate);

        void AddNotice(SwntPerTransaction notice);

        void UpdateNotice(SwntPerTransaction notice);
       
        List<SwntPerTransactionDTO> GetNoticesBySearch( BONoticeSearchCriteria criteria);

        SwntPerTransactionDTO GetNoticeById(int id);

        string GetSubjectUsingNoticeDetails(string subject, string details);
    }
}
