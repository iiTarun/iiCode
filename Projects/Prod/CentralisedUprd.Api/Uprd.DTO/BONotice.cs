using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralisedUprd.Api.UPRD.DTO
{
    public class BONotice
    {
        #region Public Properties
        public Int32 ID { get; set; }

        public Int32 InboxID { get; set; }

        public Int32 PipelineID { get; set; }

        public string PipelineDuns { get; set; }

        public Int32 RecipientCompanyID { get; set; }

        public Guid MessageID { get; set; }

        public String Message { get; set; }

        public String subject { get; set; }

        public String NoticeEffectiveDate { get; set; }

        public String NoticeEffectiveTime { get; set; }

        public String NoticeEndDate { get; set; }

        public String NoticeEndTime { get; set; }

        public String RequiredResponseIndicator { get; set; }

        public String ReferenceNumberQualifier { get; set; }

        public String GISB { get; set; }

        public Int32 StatusID { get; set; }

        public Boolean IsCritical { get; set; }

        public Boolean IsTest { get; set; }

        public Boolean IsRead { get; set; }

        public DateTime CreatedDate { get; set; }

        #endregion

        public String TSP { get; set; }
        public string PostingDate { get; set; }
        public string NoticeId { get; set; }

        public string NoticeStatusDescription { get; set; }

        public string NoticeTypeDesc1 { get; set; } = string.Empty;
        public string NoticeTypeDesc2 { get; set; } = string.Empty;
        public string ReqrdResp { get; set; } = string.Empty;
        public string PriorNotice { get; set; } = string.Empty;


    }

    public class BONoticeList : List<BONotice>
    {
        public BONoticeList()
        { }
    }

}