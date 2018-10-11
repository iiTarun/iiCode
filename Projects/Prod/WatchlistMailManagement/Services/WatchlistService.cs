using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using UPRD.Data;
using UPRD.Model;
using UPRD.Model.Enums;
using WatchlistMailManagement.Repositories;
using WatchlistMailManagement.Uprd.DTO;
using WatchlistMailManagement.UPRD.DTO;

namespace WatchlistMailManagement.Services
{
    public class WatchlistService
    {
        public ApplicationLogRepository applogs;
        public UprdWatchListRepository UprdWatchListRepository;
        ModalFactory modalFactory;
        public UPRDEntities DbContext;
        public PipelineRepository PipeRepo;

        public WatchlistService() {
            applogs = new ApplicationLogRepository();
            UprdWatchListRepository = new UprdWatchListRepository();
            modalFactory = new ModalFactory();
            DbContext = new UPRDEntities();
            PipeRepo = new PipelineRepository();
        }

        public void InsertOACYinWatchListMapping(List<OACYPerTransaction> oacyList, WatchListDTO watchlist)
        {            
            // Bulk Insert Using ADO.NET
            DataTable dt = MakeDataTable(oacyList, watchlist);
            AddExecutedWatchListDataToDB(dt);
        }


        public void InsertUNSCinWatchListMapping(List<UnscPerTransaction> unscList, WatchListDTO watchlist)
        {
            // Bulk Insert Using ADO.NET
            DataTable dt = MakeDataTableUNSC(unscList, watchlist);
            AddExecutedWatchListDataToDB(dt);
        }

        public void InsertSWNTinWatchListMapping(List<SwntPerTransaction> swntList, WatchListDTO watchlist)
        {
            // Bulk Insert Using ADO.NET
            DataTable dt = MakeDataTableSWNT(swntList, watchlist);
            AddExecutedWatchListDataToDB(dt);
        }

        public List<OACYPerTransaction> ExecuteWatchListOACYOnly(List<WatchListRule> RuleList, Type type)
        {
            UprdOACYRepository oacy = new UprdOACYRepository();
            ModalFactory factory = new ModalFactory();
            IEnumerable<OACYPerTransaction> dataList = new List<OACYPerTransaction>();
            //List<AbstractSearch> searchList;
            foreach (var rule in RuleList)
            {
                string name = UprdWatchListRepository.GetByIdMasterColumn(rule.PropertyId).Name;
                IQueryable<OACYPerTransaction> query = oacy.GetByPipelineLoc(rule.PipelineDuns, rule.LocationIdentifier);
                decimal minVal = Convert.ToDecimal(rule.value);
                decimal maxVal = Convert.ToDecimal(rule.UpperValue);
                switch (name)
                {
                    case "AvailablePercentage":
                        query = query.Where(a => a.AvailablePercentage >= minVal && a.AvailablePercentage <= maxVal);
                        break;
                    case "OperationallyAvailableQty":
                        query = query.Where(a => a.OperationallyAvailableQty >= minVal && a.OperationallyAvailableQty <= maxVal);
                        break;
                }
                var oacyList = query.ToList();
                dataList = dataList.Union(oacyList);
            }
            return dataList.ToList();//.OrderByDescending(a => a.PostingDateTime).Select(a => factory.Parse(a)).ToList();
        }


        public void AddExecutedWatchListDataToDB(DataTable dt)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["UPRDEntities"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction))
                {
                    bulkCopy.DestinationTableName = dt.TableName;
                    bulkCopy.BulkCopyTimeout = 0;
                    try
                    {
                        conn.Open();
                        bulkCopy.WriteToServer(dt);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("--Error encoutered during bulk copy for "+ dt.TableName +" " + dt.Rows.Count + " rows. Ex:" + ex.Message);
                        //throw;
                    }
                    finally
                    { conn.Close(); }
                }
            }
        }


        public DataTable MakeDataTableSWNT(List<SwntPerTransaction> swntList, WatchListDTO watchlist)
        {
            DataTable SWNTdt = new DataTable("WatchlistMailMappingSWNTs");

            DataColumn swntdtID = new DataColumn("Id", typeof(int));
            swntdtID.AutoIncrement = true;
            swntdtID.Unique = true;
            SWNTdt.Columns.Add(swntdtID);

            DataColumn UserId = new DataColumn("UserId", typeof(string));
            SWNTdt.Columns.Add(UserId);

            DataColumn WatchListId = new DataColumn("WatchListId", typeof(int));
            SWNTdt.Columns.Add(WatchListId);

            DataColumn SWNTID = new DataColumn("SWNTID", typeof(long));
            SWNTdt.Columns.Add(SWNTID);


            DataColumn EmailSentDateTime = new DataColumn("EmailSentDateTime", typeof(DateTime));
            EmailSentDateTime.AllowDBNull = true;
            SWNTdt.Columns.Add(EmailSentDateTime);

            DataColumn IsMailSent = new DataColumn("IsMailSent", typeof(bool));
            SWNTdt.Columns.Add(IsMailSent);

            DataColumn UserEmail = new DataColumn("UserEmail", typeof(string));
            SWNTdt.Columns.Add(UserEmail);


            DataColumn WatchlistName = new DataColumn("WatchlistName", typeof(string));
            SWNTdt.Columns.Add(WatchlistName);

            DataColumn MoreDetailUrl = new DataColumn("MoreDetailUrl", typeof(string));
            SWNTdt.Columns.Add(MoreDetailUrl);

            DataColumn PipelineID = new DataColumn("PipelineID", typeof(int));
            SWNTdt.Columns.Add(PipelineID);


            DataColumn PipelineName = new DataColumn("PipelineName", typeof(string));
            SWNTdt.Columns.Add(PipelineName);

            DataColumn Loc = new DataColumn("Loc", typeof(string));
            SWNTdt.Columns.Add(Loc);

            DataColumn LocName = new DataColumn("LocName", typeof(string));
            SWNTdt.Columns.Add(LocName);

            DataColumn CriticalNoticeIndicator = new DataColumn("CriticalNoticeIndicator", typeof(string));
            SWNTdt.Columns.Add(CriticalNoticeIndicator);

            DataColumn NoticeId = new DataColumn("NoticeId", typeof(string));
            SWNTdt.Columns.Add(NoticeId);

            DataColumn NoticeEffectiveDateTime = new DataColumn("NoticeEffectiveDateTime", typeof(DateTime));
            NoticeEffectiveDateTime.AllowDBNull = true;
            SWNTdt.Columns.Add(NoticeEffectiveDateTime);

            DataColumn PostingDateTime = new DataColumn("PostingDateTime", typeof(DateTime));
            PostingDateTime.AllowDBNull = true;
            SWNTdt.Columns.Add(PostingDateTime);

            DataColumn Category = new DataColumn("Category", typeof(string));
            SWNTdt.Columns.Add(Category);

            DataColumn Subject = new DataColumn("Subject", typeof(string));
            SWNTdt.Columns.Add(Subject);
            

            DataColumn[] keys = new DataColumn[1];
            keys[0] = swntdtID;
            SWNTdt.PrimaryKey = keys;

            string pipelineDuns = "";
            Pipeline pipeline = new Pipeline();
            foreach (var item in swntList)
            {
                if (pipelineDuns == "" || pipelineDuns != item.TransportationserviceProvider)
                {
                    pipelineDuns = item.TransportationserviceProvider;
                    pipeline = PipeRepo.GetPipelineByDuns(item.TransportationserviceProvider);
                }
                DataRow row = SWNTdt.NewRow();
                row["UserId"] = watchlist.UserId;
                row["WatchListId"] = watchlist.id;
                row["SWNTID"] = item.Id;
                row["EmailSentDateTime"] = DateTime.Now;
                row["IsMailSent"] = false;
                row["UserEmail"] = watchlist.UserEmail;
                row["WatchlistName"] = watchlist.ListName;
                row["MoreDetailUrl"] = watchlist.MoreDetailURLinAlert;
                row["PipelineID"] = pipeline != null ? pipeline.ID : 0;
                row["PipelineName"] = pipeline.Name + " / " + pipeline.DUNSNo;
                row["Loc"] = string.Empty;
                row["LocName"] = string.Empty;
                row["CriticalNoticeIndicator"] = item.CriticalNoticeIndicator;
                row["NoticeId"] = item.NoticeId;
                row["NoticeEffectiveDateTime"] = item.NoticeEffectiveDateTime;
                row["PostingDateTime"] = item.PostingDateTime;
                row["Category"] = item.NoticeTypeDesc1;
                row["Subject"] = item.Subject;
                SWNTdt.Rows.Add(row);
            }

            return SWNTdt;
        }


        public DataTable MakeDataTableUNSC(List<UnscPerTransaction> unscList, WatchListDTO watchlist)
        {
            DataTable UNSCdt = new DataTable("WatchlistMailMappingUNSCs");

            DataColumn unscdtID = new DataColumn("Id", typeof(int));
            unscdtID.AutoIncrement = true;
            unscdtID.Unique = true;
            UNSCdt.Columns.Add(unscdtID);

            DataColumn UserId = new DataColumn("UserId", typeof(string));
            UNSCdt.Columns.Add(UserId);

            DataColumn WatchListId = new DataColumn("WatchListId", typeof(int));
            UNSCdt.Columns.Add(WatchListId);

            DataColumn UNSCID = new DataColumn("UNSCID", typeof(long));
            UNSCdt.Columns.Add(UNSCID);


            DataColumn EmailSentDateTime = new DataColumn("EmailSentDateTime", typeof(DateTime));
            EmailSentDateTime.AllowDBNull = true;
            UNSCdt.Columns.Add(EmailSentDateTime);

            DataColumn IsMailSent = new DataColumn("IsMailSent", typeof(bool));
            UNSCdt.Columns.Add(IsMailSent);

            DataColumn UserEmail = new DataColumn("UserEmail", typeof(string));
            UNSCdt.Columns.Add(UserEmail);


            DataColumn WatchlistName = new DataColumn("WatchlistName", typeof(string));
            UNSCdt.Columns.Add(WatchlistName);

            DataColumn MoreDetailUrl = new DataColumn("MoreDetailUrl", typeof(string));
            UNSCdt.Columns.Add(MoreDetailUrl);

            DataColumn PipelineID = new DataColumn("PipelineID", typeof(int));
            UNSCdt.Columns.Add(PipelineID);


            DataColumn PipelineName = new DataColumn("PipelineName", typeof(string));
            UNSCdt.Columns.Add(PipelineName);

            DataColumn Loc = new DataColumn("Loc", typeof(string));
            UNSCdt.Columns.Add(Loc);

            DataColumn LocName = new DataColumn("LocName", typeof(string));
            UNSCdt.Columns.Add(LocName);


            DataColumn UnsubscribeCapacity = new DataColumn("UnsubscribeCapacity", typeof(long));
            UNSCdt.Columns.Add(UnsubscribeCapacity);

           
            DataColumn ChangePercentage = new DataColumn("ChangePercentage", typeof(decimal));
            UNSCdt.Columns.Add(ChangePercentage);

            DataColumn PostingDateTime = new DataColumn("PostingDateTime", typeof(DateTime));
            PostingDateTime.AllowDBNull = true;

            UNSCdt.Columns.Add(PostingDateTime);

            DataColumn[] keys = new DataColumn[1];
            keys[0] = unscdtID;
            UNSCdt.PrimaryKey = keys;

            string pipelineDuns = "";
            Pipeline pipeline = new Pipeline();
            foreach (var item in unscList)
            {
                if (pipelineDuns == "" || pipelineDuns != item.TransactionServiceProvider)
                {
                    pipelineDuns = item.TransactionServiceProvider;
                    pipeline = PipeRepo.GetPipelineByDuns(item.TransactionServiceProvider);
                }
                DataRow row = UNSCdt.NewRow();
                row["UserId"] = watchlist.UserId;
                row["WatchListId"] = watchlist.id;
                row["UNSCID"] = item.UnscID;
                row["EmailSentDateTime"] = DateTime.Now;
                row["IsMailSent"] = false;
                row["UserEmail"] = watchlist.UserEmail;
                row["WatchlistName"] = watchlist.ListName;
                row["MoreDetailUrl"] = watchlist.MoreDetailURLinAlert;               
                row["PipelineID"] = pipeline != null ? pipeline.ID : 0;
                row["PipelineName"] = pipeline.Name + " / " + pipeline.DUNSNo;
                row["Loc"] = item.Loc;
                row["LocName"] = item.LocName;
                row["UnsubscribeCapacity"] = item.UnsubscribeCapacity;
                row["ChangePercentage"] = item.ChangePercentage;
                row["PostingDateTime"] = item.PostingDateTime;
                UNSCdt.Rows.Add(row);
            }

            return UNSCdt;
        }

        public DataTable MakeDataTable(List<OACYPerTransaction> oacyList, WatchListDTO watchlist)
        {
            DataTable OACYdt = new DataTable("WatchlistMailMappingOACies");

            DataColumn oacydtID = new DataColumn("Id",typeof(int));    
            oacydtID.AutoIncrement = true;
            oacydtID.Unique = true;
            OACYdt.Columns.Add(oacydtID);

            DataColumn UserId = new DataColumn("UserId",typeof(string));
            OACYdt.Columns.Add(UserId);

            DataColumn UserEmail = new DataColumn("UserEmail", typeof(string));
            OACYdt.Columns.Add(UserEmail);

            DataColumn WatchListId = new DataColumn("WatchListId",typeof(int));            
            OACYdt.Columns.Add(WatchListId);

            DataColumn OACYID = new DataColumn("OACYID",typeof(long));            
            OACYdt.Columns.Add(OACYID);


            DataColumn EmailSentDateTime = new DataColumn("EmailSentDateTime",typeof(DateTime));
            EmailSentDateTime.AllowDBNull = true;
            OACYdt.Columns.Add(EmailSentDateTime);

            DataColumn IsMailSent = new DataColumn("IsMailSent",typeof(bool));           
            OACYdt.Columns.Add(IsMailSent);

                     


            DataColumn WatchlistName = new DataColumn("WatchlistName",typeof(string));            
            OACYdt.Columns.Add(WatchlistName);

            DataColumn MoreDetailUrl = new DataColumn("MoreDetailUrl",typeof(string));           
            OACYdt.Columns.Add(MoreDetailUrl);

            DataColumn CycleIndicator = new DataColumn("CycleIndicator",typeof(string));           
            OACYdt.Columns.Add(CycleIndicator);


            DataColumn PipelineID = new DataColumn("PipelineID", typeof(int));         
            OACYdt.Columns.Add(PipelineID);


            DataColumn PipelineName = new DataColumn("PipelineName", typeof(string));           
            OACYdt.Columns.Add(PipelineName);

            DataColumn Loc = new DataColumn("Loc",typeof(string));           
            OACYdt.Columns.Add(Loc);

            DataColumn LocName = new DataColumn("LocName", typeof(string));           
            OACYdt.Columns.Add(LocName);


            DataColumn OperatingCapacity = new DataColumn("OperatingCapacity", typeof(long));           
            OACYdt.Columns.Add(OperatingCapacity);

            DataColumn TotalScheduleQty = new DataColumn("TotalScheduleQty",typeof(long));           
            OACYdt.Columns.Add(TotalScheduleQty);

            DataColumn AvailablePercentage = new DataColumn("AvailablePercentage", typeof(decimal));          
            OACYdt.Columns.Add(AvailablePercentage);

            DataColumn PostingDateTime = new DataColumn("PostingDateTime", typeof(DateTime));
            PostingDateTime.AllowDBNull = true;           

            OACYdt.Columns.Add(PostingDateTime);

            DataColumn[] keys = new DataColumn[1];
            keys[0] = oacydtID;
            OACYdt.PrimaryKey = keys;

            string pipelineDuns = "";
            Pipeline pipeline = new Pipeline();
            foreach (var item in oacyList)
            {
                if (pipelineDuns == "" || pipelineDuns != item.TransactionServiceProvider)
                {
                    pipelineDuns = item.TransactionServiceProvider;
                    pipeline = PipeRepo.GetPipelineByDuns(item.TransactionServiceProvider);
                }                
                DataRow row = OACYdt.NewRow();
                row["UserId"] = watchlist.UserId;
                row["UserEmail"] = watchlist.UserEmail;
                row["WatchListId"] = watchlist.id;
                row["OACYID"] = item.OACYID;
                row["EmailSentDateTime"] =DateTime.Now;
                row["IsMailSent"] = false;               
                row["WatchlistName"] = watchlist.ListName;
                row["MoreDetailUrl"] = watchlist.MoreDetailURLinAlert;
                row["CycleIndicator"] = item.CycleIndicator;
                row["PipelineID"] = pipeline != null ? pipeline.ID : 0;
                row["PipelineName"] = pipeline.Name +" / "+ pipeline.DUNSNo;
                row["Loc"] = item.Loc;
                row["LocName"] = item.LocName;
                row["OperatingCapacity"] = item.OperatingCapacity;
                row["TotalScheduleQty"] = item.TotalScheduleQty;
                row["AvailablePercentage"] = item.AvailablePercentage;
                row["PostingDateTime"] = item.PostingDateTime;
                OACYdt.Rows.Add(row);
            }

            return OACYdt;
        }

        public bool GetOacyFromMappingNSendMail()
        {            
            var watchListData = UprdWatchListRepository.GetAllUnSendDataForOACYMapping().ToList();
            string userEmail = "";
            foreach (var watchListDataAccId in watchListData.GroupBy(a=>a.WatchListId))
            {
                userEmail = watchListDataAccId.FirstOrDefault().UserEmail;
                if (ConfigurationManager.AppSettings.Get("EmailIdValidation") == "true")
                {
                    if (userEmail.Contains("enercross") || userEmail.Contains("fifthnote") || userEmail.Contains("invertedi"))
                        sendOacymails(watchListDataAccId.ToList());
                    else
                        continue;
                }else
                    sendOacymails(watchListDataAccId.ToList());
            }
            return true;
        }

        private void sendOacymails(List<WatchlistMailMappingOACY> obj)
        {
            try
            {
                string subject = "Pipeline Alert: Operationally Available Capacity";
                string content = BuildEmailHtmlTemplateForOACY(obj);
                string from = ConfigurationManager.AppSettings.Get("EmailIdForAlert");
                string[] recipients = new[] { obj.FirstOrDefault().UserEmail };
                var isSend = EmailandSMSservice.SendGmail(subject, content, recipients, from);
                if (isSend)
                {
                    UprdWatchListRepository.UpdateForOACYMapping(obj);
                }
            }catch (Exception ex)
            {

            }
        }
        //public bool GetOacyFromMappingNSendMail()
        //{
        //    var UserIds = UprdWatchListRepository.GetAllUnSendDataForOACYMapping().Select(a => a.UserId).Distinct().ToList();
        //    string subject = "Pipeline Alert: Operationally Available Capacity";
        //    string userEmail = "";
        //    foreach (var userId in UserIds)
        //    {
        //        var OacyPerUser = UprdWatchListRepository.GetAllUnSendDataForOACYMappingByUser(userId);//GetAllUnSendDataForOACYMapping().Where(a => a.UserId == userId).ToList();
        //        if (OacyPerUser != null || OacyPerUser.Count > 0) {
        //            userEmail = OacyPerUser.FirstOrDefault().UserEmail;
        //        }
        //        if (ConfigurationManager.AppSettings.Get("EmailIdValidation") == "true")
        //        {
        //            if (userEmail.Contains("enercross") || userEmail.Contains("fifthnote") || userEmail.Contains("invertedi"))
        //            {

        //            }
        //            else {
        //                continue;
        //            }
        //        }
        //        foreach (var watchListdata in OacyPerUser.GroupBy(a => a.WatchListId))
        //        {
        //            string content = BuildEmailHtmlTemplateForOACY(watchListdata.ToList());
        //            string from = ConfigurationManager.AppSettings.Get("EmailIdForAlert");
        //            string[] recipients = new[] { userEmail };
        //            var isSend = EmailandSMSservice.SendGmail(subject, content, recipients, from);
        //            if (isSend)
        //            {
        //                UprdWatchListRepository.UpdateForOACYMapping(OacyPerUser);
        //            }
        //        }

        //    }
        //    return true;
        //}

        public bool GetUnscFromMappingNSendMail()
        {
            var watchListData = UprdWatchListRepository.GetAllUnSendDataForUNSCMapping().ToList();
            string userEmail = "";
            foreach (var watchListDataAccId in watchListData.GroupBy(a => a.WatchListId))
            {
                userEmail = watchListDataAccId.FirstOrDefault().UserEmail;
                if (ConfigurationManager.AppSettings.Get("EmailIdValidation") == "true")
                {
                    if (userEmail.Contains("enercross") || userEmail.Contains("fifthnote") || userEmail.Contains("invertedi"))
                        sendUnscmails(watchListDataAccId.ToList());
                    else
                        continue;
                }
                else
                    sendUnscmails(watchListDataAccId.ToList());
            }
            return true;
        }

        private void sendUnscmails(List<WatchlistMailMappingUNSC> obj)
        {
            try
            {
                string subject = "Pipeline Alert: Unsubscribed Capacity";
                string content = BuildEmailHtmlTemplateForUNSC(obj);
                string from = ConfigurationManager.AppSettings.Get("EmailIdForAlert");
                string[] recipients = new[] { obj.FirstOrDefault().UserEmail };
                var isSend = EmailandSMSservice.SendGmail(subject, content, recipients, from);
                if (isSend)
                {
                    UprdWatchListRepository.UpdateForUNSCMapping(obj);
                }
            }
            catch (Exception ex)
            {

            }
        }


        //public bool GetUnscFromMappingNSendMail()
        //{
        //    var UserIds = UprdWatchListRepository.GetAllUnSendDataForUNSCMapping().Select(a => a.UserId).Distinct().ToList();
        //    string subject = "Pipeline Alert: Unsubscribed Capacity";
        //    string userEmail = "";
        //    foreach (var userId in UserIds)
        //    {                
        //        var UnscPerUser = UprdWatchListRepository.GetAllUnSendDataForUnscMappingByUser(userId);//GetAllUnSendDataForOACYMapping().Where(a => a.UserId == userId).ToList();
        //        if (UnscPerUser != null || UnscPerUser.Count > 0)
        //        {
        //            userEmail = UnscPerUser.FirstOrDefault().UserEmail;
        //        }
        //        if (ConfigurationManager.AppSettings.Get("EmailIdValidation") == "true")
        //        {
        //            if (userEmail.Contains("enercross") || userEmail.Contains("fifthnote") || userEmail.Contains("invertedi"))
        //            {

        //            }
        //            else
        //            {
        //                continue;
        //            }
        //        }

        //        string content = BuildEmailHtmlTemplateForUNSC(UnscPerUser);
        //        string from = ConfigurationManager.AppSettings.Get("EmailIdForAlert");
        //        string[] recipients = new[] { userEmail };
        //        var isSend = EmailandSMSservice.SendGmail(subject, content, recipients, from);
        //        if (isSend)
        //        {
        //           UprdWatchListRepository.UpdateForUNSCMapping(UnscPerUser);
        //        }
        //    }
        //    return true;
        //}


        public bool GetSwntFromMappingNSendMail()
        {
            var watchListData = UprdWatchListRepository.GetAllUnSendDataForSWNTMapping().ToList();
            string userEmail = "";
            foreach (var watchListDataAccId in watchListData.GroupBy(a => a.WatchListId))
            {
                userEmail = watchListDataAccId.FirstOrDefault().UserEmail;
                if (ConfigurationManager.AppSettings.Get("EmailIdValidation") == "true")
                {
                    if (userEmail.Contains("enercross") || userEmail.Contains("fifthnote") || userEmail.Contains("invertedi"))
                        sendSwntmails(watchListDataAccId.ToList());
                    else
                        continue;
                }
                else
                    sendSwntmails(watchListDataAccId.ToList());
            }
            return true;
        }

        private void sendSwntmails(List<WatchlistMailMappingSWNT> obj)
        {
            try
            {
                string subject = "Pipeline Alert: System Wide Notices";
                string content = BuildEmailHtmlTemplateForSWNT(obj);
                string from = ConfigurationManager.AppSettings.Get("EmailIdForAlert");
                string[] recipients = new[] { obj.FirstOrDefault().UserEmail };
                var isSend = EmailandSMSservice.SendGmail(subject, content, recipients, from);
                if (isSend)
                {
                    UprdWatchListRepository.UpdateForSWNTMapping(obj);
                }
            }
            catch (Exception ex)
            {

            }
        }

        //public bool GetSwntFromMappingNSendMail()
        //{
        //    var UserIds = UprdWatchListRepository.GetAllUnSendDataForSWNTMapping().Select(a => a.UserId).Distinct().ToList();
        //    string subject = "Pipeline Alert: System Wide Notices";
        //    string userEmail = "";
        //    foreach (var userId in UserIds)
        //    {
        //        var UnscPerUser = UprdWatchListRepository.GetAllUnSendDataForSWNTMappingByUser(userId);//GetAllUnSendDataForOACYMapping().Where(a => a.UserId == userId).ToList();
        //        if (UnscPerUser != null || UnscPerUser.Count > 0)
        //        {
        //            userEmail = UnscPerUser.FirstOrDefault().UserEmail;
        //        }

        //        if (ConfigurationManager.AppSettings.Get("EmailIdValidation") == "true")
        //        {
        //            if (userEmail.Contains("enercross") || userEmail.Contains("fifthnote") || userEmail.Contains("invertedi"))
        //            {

        //            }
        //            else
        //            {
        //                continue;
        //            }
        //        }

        //        string content = BuildEmailHtmlTemplateForSWNT(UnscPerUser);
        //        string from = ConfigurationManager.AppSettings.Get("EmailIdForAlert");
        //        string[] recipients = new[] { userEmail };
        //        var isSend = EmailandSMSservice.SendGmail(subject, content, recipients, from);
        //        if (isSend)
        //        {
        //            UprdWatchListRepository.UpdateForSWNTMapping(UnscPerUser);
        //        }
        //    }
        //    return true;
        //}

        public string BuildEmailHtmlTemplateForSWNT(List<WatchlistMailMappingSWNT> alertExeDataCollection)
        {
            string viewmoreurl = "";
            string emailbody = "<div style=\" display: inline-block; width:100%;height:600px;\"><div style=\" display: inline-block;width:10%;height:20px;\"></div><div class=\"content\" style=\" display: inline-block;width:75%;height:600px; background:#fff;\"><div style=\"width:100%;height:80px; background:#ffff;\"><img src=\"http://test.natgashub.com/Assets/OrangeNom1DoneLogo.jpg\" alt =\"NatGasHub\" height=\"50px\" width=\"70px\"/><b>";
            emailbody += "  SYSTEM WIDE NOTICES ALERT";
            emailbody += "</b></div><div style=\"width:100%;min-height: 100px;overflow: auto; background:#ffff;\">";           
            viewmoreurl = alertExeDataCollection.FirstOrDefault().MoreDetailUrl;               
                    string tableStr = "";
                    tableStr += "<h3>  WatchList Name: " + alertExeDataCollection.FirstOrDefault().WatchlistName + "</h3>";
                    tableStr += "<table width=\"100%\" bgcolor=\"#f6f8f1\" border=\"0\" cellpadding=\"5\" cellspacing=\"0\">";
                    tableStr += "<thead><tr style=\"background-color:#FF6C3A; color:#fff; height:30px\"><th style=\"text-align:center;\">Type</th><th style=\"text-align:center;\">NoticeID</th><th style=\"text-align:center;\">Pipeline</th><th style=\"text-align:center;\">Eff Date</th><th style=\"text-align:center;\">Post Date</th><th style=\"text-align:center;\"> Category</th><th style=\"text-align:center;\">Subject</th></tr></thead>";
                    tableStr += "<tbody>";
                    var noticeUrl = "&IsCritical=false";
                    alertExeDataCollection = alertExeDataCollection.OrderByDescending(a => a.CriticalNoticeIndicator).ThenBy(a => a.PipelineName).ThenBy(a => a.NoticeEffectiveDateTime).ToList();
                    foreach (var swnt in alertExeDataCollection)
                    {
                        var noticeType = "";
                        if (swnt.CriticalNoticeIndicator == "Y") { noticeType = "Critical"; noticeUrl = "&IsCritical=true"; } else { noticeType = "NonCritical"; noticeUrl = "&IsCritical=false"; }
                        tableStr += "<tr><td style=\"text-align:center;\">" + noticeType + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + swnt.NoticeId + "</td>";
                        tableStr += "<td style=\"text-align:center;\"><a href=\"" + viewmoreurl + "/Notices/Index?pipelineId=" + swnt.PipelineID + noticeUrl + "\" style =\"color:#FF6C3A\">" + swnt.PipelineName + "</a></td>";
                        tableStr += "<td style=\"text-align:center;\">" + swnt.NoticeEffectiveDateTime + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + swnt.PostingDateTime + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + swnt.Category + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + swnt.Subject + "</td></tr>";
                      
                    }
                    tableStr += "</tbody></table>";
                emailbody = emailbody + tableStr;
            
            emailbody = emailbody + "</div><div style=\"width:100%;height:50px; background:#ffff;padding-top: 10px;\"><a href=\"" + viewmoreurl + "/WatchList?watchListId=" + alertExeDataCollection.FirstOrDefault().WatchListId + "\" style=\"color:#FF6C3A\">Manage Alerts</a></div><div style=\"width:100%;height:50px; background:#ffff;\">Copyright © NatGasHub.com. <span style=\"color:#FF6C3A\">Forwarding of this data is a copyright violation under U.S. law.</span></div></div><div style=\" display: inline-block;width:10%;height:20px;\"></div></div>";
            return emailbody;
        }



        public string BuildEmailHtmlTemplateForOACY(List<WatchlistMailMappingOACY> alertExeDataCollection)
        {
            string viewmoreurl = "";
            string emailbody = "<div style=\" display: inline-block; width:100%;height:600px;\"><div style=\" display: inline-block;width:10%;height:20px;\"></div><div class=\"content\" style=\" display: inline-block;width:75%;height:600px; background:#fff;\"><div style=\"width:100%;height:80px; background:#ffff;\"><img src=\"http://test.natgashub.com/Assets/OrangeNom1DoneLogo.jpg\" alt =\"NatGasHub\" height=\"50px\" width=\"70px\"/><b>";
           
            emailbody += "  OPERATIONALLY AVAILABLE CAPACITY ALERT";  

            emailbody += "</b></div><div style=\"width:100%;min-height: 100px;overflow: auto; background:#ffff;\">";
           
             viewmoreurl = alertExeDataCollection.FirstOrDefault().MoreDetailUrl;               
             string tableStr = "";
                    tableStr += "<h3> WatchList Name: " + alertExeDataCollection.FirstOrDefault().WatchlistName + "</h3>";
                    tableStr += "<table width=\"100%\" bgcolor=\"#f6f8f1\" border=\"0\" cellpadding=\"5\" cellspacing=\"0\">";
                    tableStr += "<thead><tr style=\"background-color:#FF6C3A; color:#fff; height:30px\"><th style=\"text-align:center;\">Cycle</th><th style=\"text-align:center;\">Pipeline</th><th style=\"text-align:center;\">Loc Prop</th><th style=\"text-align:center;\">Loc Name</th><th style=\"text-align:center;\">Operating Cap</th><th style=\"text-align:center;\">Scheduled Qty</th><th style=\"text-align:center;\">% Available</th><th style=\"text-align:center;\">Post Date</th></tr></thead>";
                    tableStr += "<tbody>";
                    foreach (var oacy in alertExeDataCollection.OrderBy(a => a.PipelineName).ThenBy(a => a.Loc))
                    {
                        tableStr += "<tr><td style=\"text-align:center;\">" + oacy.CycleIndicator + "</td>";
                        tableStr += "<td style=\"text-align:center;\"><a href=\"" + viewmoreurl + "/MOperationalCapacity/index?pipelineId=" + oacy.PipelineID + "\" style =\"color:#FF6C3A\">" + oacy.PipelineName + "</a></td>";
                        tableStr += "<td style=\"text-align:center;\">" + oacy.Loc + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + oacy.LocName + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + string.Format("{0:n0}", oacy.OperatingCapacity) + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + string.Format("{0:n0}", oacy.TotalScheduleQty) + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + oacy.AvailablePercentage + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + oacy.PostingDateTime + "</td></tr>";
                    }
                    tableStr += "</tbody></table>";
                emailbody = emailbody + tableStr;
           
            emailbody = emailbody + "</div><div style=\"width:100%;height:50px; background:#ffff;padding-top: 10px;\"><a href=\"" + viewmoreurl + "/WatchList?watchListId="+ alertExeDataCollection.FirstOrDefault().WatchListId + "\" style=\"color:#FF6C3A\">Manage Alerts</a></div><div style=\"width:100%;height:50px; background:#ffff;\">Copyright © NatGasHub.com. <span style=\"color:#FF6C3A\">Forwarding of this data is a copyright violation under U.S. law.</span></div></div><div style=\" display: inline-block;width:10%;height:20px;\"></div></div>";
            return emailbody;
        }


        public string BuildEmailHtmlTemplateForUNSC(List<WatchlistMailMappingUNSC> alertExeDataCollection)
        {
            string viewmoreurl = "";
            string emailbody = "<div style=\" display: inline-block; width:100%;height:600px;\"><div style=\" display: inline-block;width:10%;height:20px;\"></div><div class=\"content\" style=\" display: inline-block;width:75%;height:600px; background:#fff;\"><div style=\"width:100%;height:80px; background:#ffff;\"><img src=\"http://test.natgashub.com/Assets/OrangeNom1DoneLogo.jpg\" alt =\"NatGasHub\" height=\"50px\" width=\"70px\"/><b>";
           
            emailbody += "  UNSUBSCRIBED CAPACITY ALERT";           

            emailbody += "</b></div><div style=\"width:100%;min-height: 100px;overflow: auto; background:#ffff;\">";

           
                    viewmoreurl = alertExeDataCollection.FirstOrDefault().MoreDetailUrl;              
                    string tableStr = ""; 
                    tableStr += "<h3> WatchList Name: " + alertExeDataCollection.FirstOrDefault().WatchlistName + "</h3>";
                    tableStr += "<table width=\"100%\" bgcolor=\"#f6f8f1\" border=\"0\" cellpadding=\"5\" cellspacing=\"0\">";
                    tableStr += "<thead><tr style=\"background-color:#FF6C3A; color:#fff; height:30px\"><th style=\"text-align:center;\">Pipeline</th><th style=\"text-align:center;\">Loc Prop</th><th style=\"text-align:center;\">Loc Name</th><th style=\"text-align:center;\">Unsubscribed Cap Today</th><th style=\"text-align:center;\">% Change</th><th style=\"text-align:center;\">Post Date</th></tr></thead>";
                    tableStr += "<tbody>";
                    foreach (var unsc in alertExeDataCollection.OrderBy(a => a.PipelineName).ThenBy(a => a.Loc))
                    {
                        tableStr += "<tr><td style=\"text-align:center;\"><a href=\"" + viewmoreurl + "/MUnsubscribedCapacity/index?pipelineId=" + unsc.PipelineID + "\" style =\"color:#FF6C3A\">" + unsc.PipelineName + "</a></td>";
                        tableStr += "<td style=\"text-align:center;\">" + unsc.Loc + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + unsc.LocName + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + string.Format("{0:n0}", unsc.UnsubscribeCapacity) + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + unsc.ChangePercentage + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + unsc.PostingDateTime + "</td></tr>";
                     
                    }
                    tableStr += "</tbody></table>";                  
                emailbody = emailbody + tableStr;
           
            emailbody = emailbody + "</div><div style=\"width:100%;height:50px; background:#ffff;padding-top: 10px;\"><a href=\"" + viewmoreurl + "/WatchList?watchListId=" + alertExeDataCollection.FirstOrDefault().WatchListId + "\" style=\"color:#FF6C3A\">Manage Alerts</a></div><div style=\"width:100%;height:50px; background:#ffff;\">Copyright © NatGasHub.com. <span style=\"color:#FF6C3A\">Forwarding of this data is a copyright violation under U.S. law.</span></div></div><div style=\" display: inline-block;width:10%;height:20px;\"></div></div>";
            return emailbody;
        }


        public string BuildEmailHtmlTemplate(List<WatchListAlertExecutedDataDTO> alertExeDataCollection)
        {
            string viewmoreurl = "";
            string emailbody = "<div style=\" display: inline-block; width:100%;height:600px;\"><div style=\" display: inline-block;width:10%;height:20px;\"></div><div class=\"content\" style=\" display: inline-block;width:75%;height:600px; background:#fff;\"><div style=\"width:100%;height:80px; background:#ffff;\"><img src=\"http://test.natgashub.com/Assets/OrangeNom1DoneLogo.jpg\" alt =\"NatGasHub\" height=\"50px\" width=\"70px\"/><b>";
            // emailbody += "NatGasHub Alert</b></div><div style=\"width:100%;min-height: 100px;overflow: hidden; background:#ffff;\">";
            if (alertExeDataCollection.FirstOrDefault().watchList.DatasetId == UprdDataSet.OACY)
            {
                emailbody += "  OPERATIONALLY AVAILABLE CAPACITY ALERT";
            }
            else if (alertExeDataCollection.FirstOrDefault().watchList.DatasetId == UprdDataSet.UNSC)
            {
                emailbody += "  UNSUBSCRIBED CAPACITY ALERT";
            }
            else if (alertExeDataCollection.FirstOrDefault().watchList.DatasetId == UprdDataSet.SWNT)
            {
                emailbody += "  SYSTEM WIDE NOTICES ALERT";
                alertExeDataCollection=alertExeDataCollection.OrderByDescending(a=>a.SwntDataList.FirstOrDefault().CriticalNoticeIndicator).ToList();
            }

            emailbody += "</b></div><div style=\"width:100%;min-height: 100px;overflow: auto; background:#ffff;\">";

            foreach (var watchListdata in alertExeDataCollection)
            {
                viewmoreurl = watchListdata.watchList.MoreDetailURLinAlert;
                //int count = 1;
                //int totalRowLimit = 5;
                string tableStr = "";
                if (watchListdata.OacyDataList != null && watchListdata.OacyDataList.Count > 0)
                {
                    tableStr += "<h3> WatchList Name: " + watchListdata.watchList.ListName + "</h3>";
                    tableStr += "<table width=\"100%\" bgcolor=\"#f6f8f1\" border=\"0\" cellpadding=\"5\" cellspacing=\"0\">";
                    tableStr += "<thead><tr style=\"background-color:#FF6C3A; color:#fff; height:30px\"><th style=\"text-align:center;\">Cycle</th><th style=\"text-align:center;\">Pipeline</th><th style=\"text-align:center;\">Loc Prop</th><th style=\"text-align:center;\">Loc Name</th><th style=\"text-align:center;\">Operating Cap</th><th style=\"text-align:center;\">Scheduled Qty</th><th style=\"text-align:center;\">% Available</th><th style=\"text-align:center;\">Post Date</th></tr></thead>";
                    tableStr += "<tbody>";
                    foreach (var oacy in watchListdata.OacyDataList.OrderBy(a => a.PipelineNameDuns).ThenBy(a => a.Loc))
                    {
                        tableStr += "<tr><td style=\"text-align:center;\">" + oacy.CycleIndicator + "</td>";
                        tableStr += "<td style=\"text-align:center;\"><a href=\"" + viewmoreurl + "/MOperationalCapacity/index?pipelineId=" + oacy.PipelineID + "\" style =\"color:#FF6C3A\">" + oacy.PipelineNameDuns + "</a></td>";
                        tableStr += "<td style=\"text-align:center;\">" + oacy.Loc + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + oacy.LocName + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + string.Format("{0:n0}", oacy.OperatingCapacity)   + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + string.Format("{0:n0}", oacy.TotalScheduleQty) + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + oacy.AvailablePercentage + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + oacy.PostingDate + "</td></tr>";
                       
                    }
                    tableStr += "</tbody></table>";                  

                }
                else if (watchListdata.UnscDataList != null && watchListdata.UnscDataList.Count > 0)
                {
                    tableStr += "<h3> WatchList Name: " + watchListdata.watchList.ListName + "</h3>";
                    tableStr += "<table width=\"100%\" bgcolor=\"#f6f8f1\" border=\"0\" cellpadding=\"5\" cellspacing=\"0\">";
                    tableStr += "<thead><tr style=\"background-color:#FF6C3A; color:#fff; height:30px\"><th style=\"text-align:center;\">Pipeline</th><th style=\"text-align:center;\">Loc Prop</th><th style=\"text-align:center;\">Loc Name</th><th style=\"text-align:center;\">Unsubscribed Cap Today</th><th style=\"text-align:center;\">% Change</th><th style=\"text-align:center;\">Post Date</th></tr></thead>";
                    tableStr += "<tbody>";
                    foreach (var unsc in watchListdata.UnscDataList.OrderBy(a => a.PipelineNameDuns).ThenBy(a => a.Loc))
                    {
                        tableStr += "<tr><td style=\"text-align:center;\"><a href=\"" + viewmoreurl + "/MUnsubscribedCapacity/index?pipelineId=" + unsc.PipelineID + "\" style =\"color:#FF6C3A\">" + unsc.PipelineNameDuns + "</a></td>";
                        tableStr += "<td style=\"text-align:center;\">" + unsc.Loc + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + unsc.LocName + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + string.Format("{0:n0}", unsc.UnsubscribeCapacity) + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + unsc.ChangePercentage + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + unsc.PostingDate + "</td></tr>";
                        //count++;
                        //if (count > totalRowLimit)
                        //{
                        //    break;
                        //}
                    }
                    tableStr += "</tbody></table>";
                    //if (count > totalRowLimit)
                    //{
                    //    tableStr += "<div style=\"width:100%;height:50px; background:#ffff;padding-top: 10px;\"><a href=\"" + viewmoreurl + "/WatchList/List?pipelineId=1\" style=\"color:#FF6C3A\"> View More</a></div>";
                    //   // emailbody = emailbody + tableStr;
                    //    //break;
                    //}
                }
                else if (watchListdata.SwntDataList != null && watchListdata.SwntDataList.Count > 0)
                {
                    tableStr += "<h3>  WatchList Name: " + watchListdata.watchList.ListName + "</h3>";
                    tableStr += "<table width=\"100%\" bgcolor=\"#f6f8f1\" border=\"0\" cellpadding=\"5\" cellspacing=\"0\">";
                    tableStr += "<thead><tr style=\"background-color:#FF6C3A; color:#fff; height:30px\"><th style=\"text-align:center;\">Type</th><th style=\"text-align:center;\">NoticeID</th><th style=\"text-align:center;\">Pipeline</th><th style=\"text-align:center;\">Eff Date</th><th style=\"text-align:center;\">Post Date</th><th style=\"text-align:center;\"> Category</th><th style=\"text-align:center;\">Subject</th></tr></thead>";
                    tableStr += "<tbody>";
                    var noticeUrl = "&IsCritical=false";

                    watchListdata.SwntDataList = watchListdata.SwntDataList.OrderByDescending(a => a.CriticalNoticeIndicator).ThenBy(a => a.PipelineNameDuns).ThenBy(a => a.NoticeEffectiveDateTime).ToList();
                    foreach (var swnt in watchListdata.SwntDataList)
                    {
                        var noticeType = "";
                        if (swnt.CriticalNoticeIndicator == "Y") { noticeType = "Critical"; noticeUrl = "&IsCritical=true"; } else { noticeType = "NonCritical"; noticeUrl = "&IsCritical=false"; }
                        tableStr += "<tr><td style=\"text-align:center;\">" + noticeType + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + swnt.NoticeId + "</td>";
                        tableStr += "<td style=\"text-align:center;\"><a href=\"" + viewmoreurl + "/Notices/Index?pipelineId=" + swnt.PipelineId + noticeUrl + "\" style =\"color:#FF6C3A\">" + swnt.PipelineNameDuns + "</a></td>";
                        tableStr += "<td style=\"text-align:center;\">" + swnt.NoticeEffectiveDateTime + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + swnt.PostingDateTime + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + swnt.NoticeTypeDesc1 + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + swnt.Subject + "</td></tr>";
                        //count++;
                        //if (count > totalRowLimit)
                        //{
                        //    break;
                        //}
                    }
                    tableStr += "</tbody></table>";
                    //if (count > totalRowLimit)
                    //{
                    //    tableStr += "<div style=\"width:100%;height:50px; background:#ffff;padding-top: 10px;\"><a href=\"" + viewmoreurl + "/WatchList/List?pipelineId=1\" style=\"color:#FF6C3A\"> View More</a></div>";
                    //   // emailbody = emailbody + tableStr;
                    //   // break;
                    //}
                }
                emailbody = emailbody + tableStr;

            }
            emailbody = emailbody + "</div><div style=\"width:100%;height:50px; background:#ffff;padding-top: 10px;\"><a href=\"" + viewmoreurl + "/WatchList/List?pipelineId=1\" style=\"color:#FF6C3A\">Manage Alerts</a></div><div style=\"width:100%;height:50px; background:#ffff;\">Copyright © NatGasHub.com. <span style=\"color:#FF6C3A\">Forwarding of this data is a copyright violation under U.S. law.</span></div></div><div style=\" display: inline-block;width:10%;height:20px;\"></div></div>";
            return emailbody;
        }


        public  bool SendNotificationByTextOrEmail(List<WatchListAlertExecutedDataDTO> alertExeDataCollection)
        {
            try
            {
               
                var userIds = alertExeDataCollection.Select(a => a.UserId).Distinct();
                foreach (var userId in userIds)
                {
                    var WatchListDataByuser = alertExeDataCollection.Where(a => a.UserId == userId).ToList();

                    #region Get UserInfo - mailaddress and PhoneNumber

                    var userEmail = "";
                    if (WatchListDataByuser.Count != 0) {
                        userEmail=WatchListDataByuser.FirstOrDefault().watchList.UserEmail;
                    }
                    //IdentityUsersRepo.GetUserEmailByUserId(userId);
                    // var UserPhone = IdentityUsersRepo.GetUserPhoneByUserId(userId);

                    #endregion

                    #region SendMail

                    string subject = "NatGasHub WatchList Alerts";
                    if (WatchListDataByuser.FirstOrDefault().watchList.DatasetId == UprdDataSet.OACY)
                    {
                        subject = "Pipeline Alert: Operationally Available Capacity";
                    }
                    else if (WatchListDataByuser.FirstOrDefault().watchList.DatasetId == UprdDataSet.UNSC)
                    {
                        subject = "Pipeline Alert: Unsubscribed Capacity";
                    }
                    else if (WatchListDataByuser.FirstOrDefault().watchList.DatasetId == UprdDataSet.SWNT)
                    {
                        subject = "Pipeline Alert: System Wide Notices";
                    }
                    string content = BuildEmailHtmlTemplate(WatchListDataByuser);


                   // string[] recipients = new[] { "vijay.kumar@invertedi.com" };//,"neha.sharma@invertedi.com","Jay.singh@enercross.com" };
                    // subject+=userEmail;


                    string from = ConfigurationManager.AppSettings.Get("EmailIdForAlert");
                    string[] recipients = new[] { userEmail };

                    var isSend = EmailandSMSservice.SendGmail(subject, content, recipients, from);
                    if (isSend)
                    {
                        EmailQueueDto emailDto = new EmailQueueDto();
                        emailDto.ToUserID = userId;
                        emailDto.Subject = subject;
                        emailDto.Email = content;
                        emailDto.Recipient = recipients.FirstOrDefault();
                        emailDto.IsSent = isSend;
                        emailDto.CreatedDate = DateTime.Now;
                        emailDto.SentDate = DateTime.Now;
                        UprdWatchListRepository.AddEmailQueue(emailDto);

                    }

                    #endregion

                    #region Send SMS 

                    //  var toMobnumber = "+91 ***** *****";    //Verified mobile number on twilio site.
                    ////  var toMobnumber = UserPhone;
                    //  var fromMobnumber = "+15756131164";    // twilio Number 
                    //  var bodyContent = "This is Test SMS service. Please Ignore it.";
                    //  var isSmsSend = EmailandSMSservice.SendSMS(toMobnumber, fromMobnumber, bodyContent);

                    #endregion
                }
                return true;
            }
            catch (Exception ex)
            {
                applogs.AppLogManager("WatchListService", "WatchListAlertJob", "Error SendNotificationByTextOrEmail: "+ ex.ToString());
                return false;
            }
        }
      
        
        public  WatchListDTO GetWatchListById(int watchListId)
        {
            var watchlist = UprdWatchListRepository.GetById(watchListId);
            WatchListDTO watchlistdto = new WatchListDTO();
            watchlistdto.id = watchlist.Id;
            watchlistdto.UserId = watchlist.UserId;
            watchlistdto.DatasetId = watchlist.DataSetId;
            //switch (watchlist.DataSetId) {
            //    case 1:
            //        watchlistdto.DatasetId = UprdDataSet.OACY;
            //        break;
            //    case 2:
            //        watchlistdto.DatasetId = UprdDataSet.UNSC;
            //        break;
            //    case 3:
            //        watchlistdto.DatasetId = UprdDataSet.SWNT;
            //        break;
            //}         
            watchlistdto.ListName = watchlist.Name;
            watchlistdto.UserEmail = watchlist.Email;
            watchlistdto.RuleList = GetWatchListRules(watchlist.Id);
            watchlistdto.MoreDetailURLinAlert = watchlist.MoreDetailURLinAlert;
            return watchlistdto;
        }

        private  List<WatchListRule> GetWatchListRules(int watchListId)
        {
            var modellist = UprdWatchListRepository.GetByWatchListId(watchListId).ToList();
            List<WatchListRule> dtoList = new List<WatchListRule>();
            foreach (var item in modellist)
            {
                WatchListRule dto = new WatchListRule();
                dto.id = item.Id;
                dto.PropertyId = item.ColumnId;
                dto.ComparatorsId = item.OperatorId;
                dto.value = item.RuleValue;
                dto.PipelineDuns = item.PipelineDuns;
                dto.LocationIdentifier = item.LocationIdentifier;
                dto.AlertFrequency = item.AlertFrequency;//==1? Enum.WatchlistAlertFrequency.Daily : Enum.WatchlistAlertFrequency.WhenAvailable;
                dto.AlertSent = item.AlertSent;
                dto.IsCriticalNotice = item.IsCriticalNotice;
                dto.UpperValue = item.UpperRuleValue;
                dtoList.Add(dto);
            }
            return dtoList;
        }


        public  List<WatchListDTO> GetWatchListByUserId(string userId)
        {
            List<WatchListDTO> list = new List<WatchListDTO>();
            var watchlists = UprdWatchListRepository.GetByUserId(userId).OrderByDescending(a => a.CreatedDate).ToList();
            foreach (var watchlist in watchlists)
            {
                WatchListDTO watchlistdto = new WatchListDTO();
                watchlistdto.id = watchlist.Id;
                watchlistdto.UserId = watchlist.UserId;
                watchlistdto.DatasetId = watchlist.DataSetId;
                //switch (watchlist.DataSetId)
                //{
                //    case 1:
                //        watchlistdto.DatasetId = Enum.UprdDataSet.OACY;
                //        break;
                //    case 2:
                //        watchlistdto.DatasetId = Enum.UprdDataSet.UNSC;
                //        break;
                //    case 3:
                //        watchlistdto.DatasetId = Enum.UprdDataSet.SWNT;
                //        break;
                //}              
                watchlistdto.ListName = watchlist.Name;
                watchlistdto.UserEmail = watchlist.Email;
                watchlistdto.RuleList = GetWatchListRules(watchlist.Id);
                watchlistdto.MoreDetailURLinAlert = watchlist.MoreDetailURLinAlert;
                list.Add(watchlistdto);
            }
            return list;
        }

        public  List<WatchListProperty> GetPropertiesByDataSet(UprdDataSet dataset)
        {
            var list = UprdWatchListRepository.GetByDataSet(dataset).ToList();
            List<WatchListProperty> propertylist = new List<WatchListProperty>();
            foreach (var item in list)
            {
                WatchListProperty property = new WatchListProperty();
                property.Id = item.Id;
                property.Name = item.Name;
                property.Title = item.Title;
                property.DataSet = dataset;
                property.Datatype = UprdWatchListRepository.GetByIdDataTypeGrouping(item.DataTypeId).Name;
                property.DatatypeId = item.DataTypeId;
                property.Operators = GetOperatorByDataType(property.DatatypeId);
                propertylist.Add(property);
            }
            return propertylist;
        }


        public  WatchListProperty GetPropertyById(int propertyId)
        {
            var item = UprdWatchListRepository.GetByIdMasterColumn(propertyId);
            WatchListProperty property = new WatchListProperty();
            property.Id = item.Id;
            property.Name = item.Name;
            property.Title = item.Title;
            property.DataSet = item.DataSetId;
            //switch (item.DataSetId)
            //{
            //    case 1:
            //        property.DataSet = Enum.UprdDataSet.OACY;
            //        break;
            //    case 2:
            //        property.DataSet = Enum.UprdDataSet.UNSC;
            //        break;
            //    case 3:
            //        property.DataSet = Enum.UprdDataSet.SWNT;
            //        break;
            //}          
            property.Datatype = UprdWatchListRepository.GetByIdDataTypeGrouping(item.DataTypeId).Name;
            property.DatatypeId = item.DataTypeId;
            property.Operators = GetOperatorByDataType(property.DatatypeId);
            return property;
        }


        public  List<WatchListOperator> GetOperatorByDataType(int dataTypeId)
        {
            var list = UprdWatchListRepository.GetByDataTypeId(dataTypeId);
            List<WatchListOperator> operatorList = new List<WatchListOperator>();
            foreach (var item in list)
            {
                WatchListOperator operatr = new WatchListOperator();
                operatr.Id = item.Id;
                operatr.Name = item.Name;
                operatr.Title = item.Title;
                operatr.DataType = UprdWatchListRepository.GetByIdDataTypeGrouping(item.DataTypeId).Name;
                operatorList.Add(operatr);
            }
            return operatorList;
        }

        public  List<LocationsDTO> GetLocationsFromUPRDs(string PipelineDuns, string Keyword)
        {
            List<LocationsDTO> locs = new List<LocationsDTO>();
            UprdLocationsRepository locRepo = new UprdLocationsRepository();           
            var locsQuery = locRepo.GetLocations(Keyword, PipelineDuns).ToList();
            locs = locsQuery.Select(a => modalFactory.Parse(a)).OrderBy(a => a.Name).ToList();
            return locs;
        }
        public  int GetTotalLocationsFromUPRDs(string PipelineDuns,string Keyword)
        {
            int locs = 0;           
            UprdLocationsRepository locRepo = new UprdLocationsRepository();              
            locs = locRepo.GetLocations(Keyword, PipelineDuns).Count();
            return locs;
        }


        public LocationsResultDTO GetLocationsFromUPRDsUsingPaging(string PipelineDuns, string Keyword, int PageNo, int PageSize, string order, string orderDir)
        {
            LocationsResultDTO result = new LocationsResultDTO();      
            UprdLocationsRepository locRepo = new UprdLocationsRepository();           
            IQueryable<Location> LocDTOQuery;
            SortingPagingInfo sortInfo = new SortingPagingInfo();
            sortInfo.CurrentPageIndex = PageNo;
            sortInfo.PageSize = PageSize;
            sortInfo.SortDirection = orderDir;
            sortInfo.SortField = order;
                       
            var  locsQuery = locRepo.GetLocations(Keyword,PipelineDuns);
            result.RecordCount = locsQuery.Count();
            LocDTOQuery = SortByColumnWithOrder(locsQuery, sortInfo);
            var loc = LocDTOQuery.Skip((sortInfo.CurrentPageIndex - 1) * sortInfo.PageSize).Take(sortInfo.PageSize).ToList();
            result.locationsDTO = loc.Select(a => modalFactory.Parse(a)).OrderBy(a => a.Name).ToList();
            return result;
        }

        public  IQueryable<Location> SortByColumnWithOrder(IQueryable<Location> dataQuery, SortingPagingInfo sortingPagingInfo)
        {
            if (sortingPagingInfo != null)
            {
                // Sorting
                string orderDir = sortingPagingInfo.SortDirection;
                switch (sortingPagingInfo.SortField)
                {
                    case "Loc":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.Identifier)
                                                                                                 : dataQuery.OrderBy(p => p.Identifier);
                        break;
                    case "LocName":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.Name)
                                                                                                 : dataQuery.OrderBy(p => p.Name);
                        break;

                    default:
                        dataQuery = dataQuery.OrderBy(p => p.Name);

                        break;

                }
            }
            else
            {
                return dataQuery;
            }

            return dataQuery;
        }


        public  bool DeleteWatchListById(int watchListid)
        {
            try
            {
                var watchlist = UprdWatchListRepository.GetById(watchListid);
                UprdWatchListRepository.Delete(watchlist);                
                var watchListsRules = UprdWatchListRepository.GetByWatchListId(watchListid).ToList();
                foreach (var item in watchListsRules) { UprdWatchListRepository.Delete(item);}

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public  List<OACYPerTransactionDTO> ExecuteWatchListOACYonScreen(List<WatchListRule> RuleList, Type type)
        {
            UprdOACYRepository oacy = new UprdOACYRepository();
            ModalFactory factory = new ModalFactory();
            IEnumerable<OACYPerTransaction> dataList = new List<OACYPerTransaction>();
            // List<AbstractSearch> searchList;
            foreach (var rule in RuleList)
            {
                //searchList = new List<AbstractSearch>();
                string name = UprdWatchListRepository.GetByIdMasterColumn(rule.PropertyId).Name;
                IQueryable<OACYPerTransaction> query= oacy.GetAllByPipelineLoc(rule.PipelineDuns, rule.LocationIdentifier);
                decimal minVal = Convert.ToDecimal(rule.value);
                decimal maxVal = Convert.ToDecimal(rule.UpperValue);
                switch (name) {
                    case "AvailablePercentage":
                        query = query.Where(a => a.AvailablePercentage >= minVal && a.AvailablePercentage <= maxVal);
                        break;
                    case "OperationallyAvailableQty":
                        query = query.Where(a => a.OperationallyAvailableQty >= minVal && a.OperationallyAvailableQty <= maxVal);
                        break;
                }
                var oacyList = query.ToList();
                //var property = type.GetProperties()
                //.Where(p => p.CanRead && p.CanWrite && (p.Name == name)).FirstOrDefault();
                //var searchProperty = SearchExtensions.CreateSearchCriteria(type, property.PropertyType, property.Name, UprdWatchListRepository.GetByIdLogicalOperator(rule.ComparatorsId).Name, rule.value);
                //searchList.Add(searchProperty);
                //var oacyList = oacy.GetAllByPipelineLoc(rule.PipelineDuns, rule.LocationIdentifier).ApplySearchCriterias(searchList).ToList();
                dataList = dataList.Union(oacyList);
            }

            var OACYResult = dataList.OrderByDescending(a => a.PostingDateTime).Select(a => factory.Parse(a));

            var FinalResult = (from o in OACYResult
                               join l in DbContext.Location on o.TransactionServiceProvider equals l.PipeDuns into oac
                               from oacyResults in oac.Where(x => x.PropCode == o.Loc).DefaultIfEmpty()

                               select new OACYPerTransactionDTO
                               {
                                    OACYID = o.OACYID,
                                    TransactionID = o.TransactionID,
                                    ReceiceFileID = o.ReceiceFileID,
                                    CreatedDate = o.CreatedDate,
                                    TransactionServiceProviderPropCode = o.TransactionServiceProviderPropCode,
                                    TransactionServiceProvider = o.TransactionServiceProvider,
                                    PostingDateTime = o.PostingDateTime,
                                    EffectiveGasDayTime = o.EffectiveGasDayTime,
                                    Loc = o.Loc,
                                    LocName = (o.LocName =="" ? oacyResults.Name : o.LocName),
                                    LocZn = o.LocZn,
                                    FlowIndicator = o.FlowIndicator,
                                    LocPropDesc = o.LocPropDesc,
                                    LocQTIDesc = o.LocQTIDesc,
                                    MeasurementBasis = o.MeasurementBasis,
                                    ITIndicator = o.ITIndicator, 
                                    AllQtyAvailableIndicator = o.AllQtyAvailableIndicator,
                                    DesignCapacity = o.DesignCapacity,
                                    OperatingCapacity = o.OperatingCapacity,
                                    TotalScheduleQty = o.TotalScheduleQty,
                                    OperationallyAvailableQty = o.OperationallyAvailableQty,
                                    PipelineID = o.PipelineID,
                                    CycleIndicator = o.CycleIndicator,
                                    AvailablePercentage = o.AvailablePercentage,
                                    EffectiveGasDay = o.EffectiveGasDay,
                                    PipelineNameDuns =o.PipelineNameDuns,
                                    PostingDate=o.PostingDate
                                
                               });
            return FinalResult.ToList();
          
        }

        public  List<UnscPerTransactionDTO> ExecuteWatchListUNSConScreen(List<WatchListRule> RuleList, Type type)
        {
            UprdUnscRepository unsc = new UprdUnscRepository();
            ModalFactory factory = new ModalFactory();
            IEnumerable<UnscPerTransaction> dataList = new List<UnscPerTransaction>();
            List<AbstractSearch> searchList;
            foreach (var rule in RuleList)
            {
                searchList = new List<AbstractSearch>();
                string name = UprdWatchListRepository.GetByIdMasterColumn(rule.PropertyId).Name;
                var property = type.GetProperties()
                .Where(p => p.CanRead && p.CanWrite && (p.Name == name)).FirstOrDefault();
                var searchProperty = SearchExtensions.CreateSearchCriteria(type, property.PropertyType, property.Name, UprdWatchListRepository.GetByIdLogicalOperator(rule.ComparatorsId).Name, rule.value);
                searchList.Add(searchProperty);
                var unscList = unsc.GetAllByPipelineLoc(rule.PipelineDuns, rule.LocationIdentifier).ApplySearchCriterias(searchList).ToList();
                dataList = dataList.Union(unscList);
            }
            var UNSCResult = dataList.OrderByDescending(a => a.PostingDateTime).Select(a => factory.Parse(a));

            var FinalResult = (from u in UNSCResult join l in DbContext.Location on u.TransactionServiceProvider equals l.PipeDuns into uns
                              from unscResults in uns.Where(x => x.PropCode == u.Loc).DefaultIfEmpty()

                     select new UnscPerTransactionDTO()
                     {
                         UnscID = u.UnscID,
                         TransactionID = u.TransactionID,
                         ReceiveFileID = u.ReceiveFileID,
                         CreatedDate = u.CreatedDate,
                         PipelineID = u.PipelineID,
                         TransactionServiceProvider = u.TransactionServiceProvider,
                         TransactionServiceProviderPropCode = u.TransactionServiceProviderPropCode,
                         Loc = u.Loc,
                         LocName = (u.LocName == "" ? unscResults.Name : u.LocName),
                         LocZn = u.LocZn,
                         LocPurpDesc = u.LocPurpDesc,
                         LocQTIDesc = u.LocQTIDesc,
                         MeasBasisDesc = u.MeasBasisDesc,
                         TotalDesignCapacity = u.TotalDesignCapacity,
                         UnsubscribeCapacity = u.UnsubscribeCapacity,
                         EffectiveGasDay = u.EffectiveGasDay,
                         EndingEffectiveDay = u.EndingEffectiveDay,
                         ChangePercentage = u.ChangePercentage,
                         PipelineNameDuns =u.PipelineNameDuns,
                         PostingDate= u.PostingDate
                     });

            return FinalResult.ToList();
        }



        public  List<SwntPerTransactionDTO> ExecuteWatchListSWNTOnScreen(List<WatchListRule> RuleList, Type type)
        {
            UprdSwntRepository swnt = new UprdSwntRepository();
            ModalFactory factory = new ModalFactory();
            IEnumerable<SwntPerTransaction> dataList = new List<SwntPerTransaction>();
            List<AbstractSearch> searchList;
            foreach (var rule in RuleList)
            {
                // var pipeline = IPipelineRepository.GetPipelineByDuns(rule.PipelineDuns);
                searchList = new List<AbstractSearch>();
                if (rule.PropertyId == 0)
                {
                    var allnoticeList = swnt.GetAllNoticeForPipe(rule.PipelineDuns, rule.IsCriticalNotice).ToList();
                    dataList = dataList.Union(allnoticeList);
                }
                else
                {
                    string name = UprdWatchListRepository.GetByIdMasterColumn(rule.PropertyId).Name;
                    var property = type.GetProperties()
                    .Where(p => p.CanRead && p.CanWrite && (p.Name == name)).FirstOrDefault();
                    var searchProperty = SearchExtensions.CreateSearchCriteria(type, property.PropertyType, property.Name, UprdWatchListRepository.GetByIdLogicalOperator(rule.ComparatorsId).Name, rule.value);
                    searchList.Add(searchProperty);
                    var noticeList = swnt.GetAllNoticeForPipe(rule.PipelineDuns, rule.IsCriticalNotice).ApplySearchCriterias(searchList).ToList();
                    dataList = dataList.Union(noticeList);
                }

            }
            return dataList.OrderByDescending(a => a.PostingDateTime).Select(a => factory.Parse(a)).ToList();
        }

        public  List<SwntPerTransactionDTO> ExecuteWatchListSWNT(List<WatchListRule> RuleList, Type type)
        {
            UprdSwntRepository swnt = new UprdSwntRepository();
            ModalFactory factory = new ModalFactory();
            IEnumerable<SwntPerTransaction> dataList = new List<SwntPerTransaction>();
            List<AbstractSearch> searchList;
            foreach (var rule in RuleList)
            {
                // var pipeline = IPipelineRepository.GetPipelineByDuns(rule.PipelineDuns);
                searchList = new List<AbstractSearch>();
                if (rule.PropertyId == 0)
                {
                    var allnoticeList = swnt.GetNotice(rule.PipelineDuns, rule.IsCriticalNotice).ToList();
                    dataList = dataList.Union(allnoticeList);
                }
                else
                {
                    string name = UprdWatchListRepository.GetByIdMasterColumn(rule.PropertyId).Name;
                    var property = type.GetProperties()
                    .Where(p => p.CanRead && p.CanWrite && (p.Name == name)).FirstOrDefault();
                    var searchProperty = SearchExtensions.CreateSearchCriteria(type, property.PropertyType, property.Name, UprdWatchListRepository.GetByIdLogicalOperator(rule.ComparatorsId).Name, rule.value);
                    searchList.Add(searchProperty);
                    var noticeList = swnt.GetNotice(rule.PipelineDuns, rule.IsCriticalNotice).ApplySearchCriterias(searchList).ToList();
                    dataList = dataList.Union(noticeList);
                }

            }
            return dataList.OrderByDescending(a => a.PostingDateTime).Select(a => factory.Parse(a)).ToList();
        }


        public List<SwntPerTransaction> ExecuteWatchListSWNTModel(List<WatchListRule> RuleList, Type type)
        {
            UprdSwntRepository swnt = new UprdSwntRepository();
            ModalFactory factory = new ModalFactory();
            IEnumerable<SwntPerTransaction> dataList = new List<SwntPerTransaction>();
            List<AbstractSearch> searchList;
            foreach (var rule in RuleList)
            {
                // var pipeline = IPipelineRepository.GetPipelineByDuns(rule.PipelineDuns);
                searchList = new List<AbstractSearch>();
                if (rule.PropertyId == 0)
                {
                    var allnoticeList = swnt.GetNotice(rule.PipelineDuns, rule.IsCriticalNotice).ToList();
                    dataList = dataList.Union(allnoticeList);
                }
                else
                {
                    string name = UprdWatchListRepository.GetByIdMasterColumn(rule.PropertyId).Name;
                    var property = type.GetProperties()
                    .Where(p => p.CanRead && p.CanWrite && (p.Name == name)).FirstOrDefault();
                    var searchProperty = SearchExtensions.CreateSearchCriteria(type, property.PropertyType, property.Name, UprdWatchListRepository.GetByIdLogicalOperator(rule.ComparatorsId).Name, rule.value);
                    searchList.Add(searchProperty);
                    var noticeList = swnt.GetNotice(rule.PipelineDuns, rule.IsCriticalNotice).ApplySearchCriterias(searchList).ToList();
                    dataList = dataList.Union(noticeList);
                }

            }
            return dataList.ToList();//.OrderByDescending(a => a.PostingDateTime).Select(a => factory.Parse(a)).ToList();
        }
        
        public bool ExecuteWatchListMailAlertOACY(WatchlistAlertFrequency alertType)
        {
            try
            {
                bool isSent = false;
                List<WatchListDTO> list = new List<WatchListDTO>();
                var watchlists = UprdWatchListRepository.GetAllBydataSet(UprdDataSet.OACY).ToList();
                foreach (var watchlist in watchlists)
                {
                    WatchListDTO watchlistdto = new WatchListDTO();
                    watchlistdto.id = watchlist.Id;
                    watchlistdto.UserId = watchlist.UserId;
                    watchlistdto.DatasetId = watchlist.DataSetId;                            
                    watchlistdto.ListName = watchlist.Name;
                    watchlistdto.UserEmail = watchlist.Email;
                    watchlistdto.MoreDetailURLinAlert = watchlist.MoreDetailURLinAlert;
                    watchlistdto.RuleList = GetWatchListRules(watchlist.Id);
                    list.Add(watchlistdto);
                }

                List<WatchListRule> ruleList = new List<WatchListRule>();
                foreach (var Item in list)
                {
                    if (alertType == WatchlistAlertFrequency.Daily)
                        ruleList = Item.RuleList.Where(a => a.AlertSent && a.AlertFrequency == WatchlistAlertFrequency.Daily).ToList();
                    else if (alertType == WatchlistAlertFrequency.WhenAvailable)
                        ruleList = Item.RuleList.Where(a => a.AlertSent && a.AlertFrequency == WatchlistAlertFrequency.WhenAvailable).ToList();

                    if (ruleList.Count == 0) { continue; }
                                       
                    var dataOacy = ExecuteWatchListOACYOnly(ruleList, typeof(OACYPerTransaction));
                    dataOacy = GetLatestOACYValidation(dataOacy, Item.id, Item.UserId);

                    if (dataOacy.Count > 0) {
                        InsertOACYinWatchListMapping(dataOacy, Item);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                applogs.AppLogManager("WatchListService", "WatchListAlertJob", "Error ExecuteWatchListMailAlertOACY: " + ex.ToString());
                return false;
            }
        }

        public bool ExecuteWatchListMailAlertUNSC(WatchlistAlertFrequency alertType)
        {
            try
            {
                bool isSent = false;
                List<WatchListDTO> list = new List<WatchListDTO>();
                var watchlists = UprdWatchListRepository.GetAllBydataSet(UprdDataSet.UNSC).ToList();
                foreach (var watchlist in watchlists)
                {
                    WatchListDTO watchlistdto = new WatchListDTO();
                    watchlistdto.id = watchlist.Id;
                    watchlistdto.UserId = watchlist.UserId;
                    watchlistdto.DatasetId = watchlist.DataSetId;                              
                    watchlistdto.ListName = watchlist.Name;
                    watchlistdto.UserEmail = watchlist.Email;
                    watchlistdto.MoreDetailURLinAlert = watchlist.MoreDetailURLinAlert;
                    watchlistdto.RuleList = GetWatchListRules(watchlist.Id);
                    list.Add(watchlistdto);
                }
                List<WatchListRule> ruleList = new List<WatchListRule>();
                foreach (var Item in list)
                {
                    if (alertType == WatchlistAlertFrequency.Daily)
                        ruleList = Item.RuleList.Where(a => a.AlertSent && a.AlertFrequency == WatchlistAlertFrequency.Daily).ToList();
                    else if (alertType == WatchlistAlertFrequency.WhenAvailable)
                        ruleList = Item.RuleList.Where(a => a.AlertSent && a.AlertFrequency == WatchlistAlertFrequency.WhenAvailable).ToList();

                    if (ruleList.Count == 0) { continue; }
                    var UnscDataList = ExecuteWatchListUNSCModel(ruleList, typeof(UnscPerTransaction));
                    UnscDataList = GetLatestUNSCFilter(UnscDataList, Item.id, Item.UserId);                                  
                    if (UnscDataList.Count > 0) {
                        InsertUNSCinWatchListMapping(UnscDataList,Item);
                    }                           
                }               
                return isSent;
            }
            catch (Exception ex)
            {
                applogs.AppLogManager("WatchListService", "WatchListAlertJob", "Error ExecuteWatchListMailAlertUNSC: " + ex.ToString());
                return false;
            }
        }

        public bool ExecuteWatchListMailAlertSWNT(WatchlistAlertFrequency alertType)
        {
            try
            {
                bool isSent = false;
                List<WatchListDTO> list = new List<WatchListDTO>();
                var watchlists = UprdWatchListRepository.GetAllBydataSet(UprdDataSet.SWNT).ToList();
                foreach (var watchlist in watchlists)
                {
                    WatchListDTO watchlistdto = new WatchListDTO();
                    watchlistdto.id = watchlist.Id;
                    watchlistdto.UserId = watchlist.UserId;
                    watchlistdto.DatasetId = watchlist.DataSetId;
                    watchlistdto.ListName = watchlist.Name;
                    watchlistdto.UserEmail = watchlist.Email;
                    watchlistdto.MoreDetailURLinAlert = watchlist.MoreDetailURLinAlert;
                    watchlistdto.RuleList = GetWatchListRules(watchlist.Id);
                    list.Add(watchlistdto);
                }
                List<WatchListRule> ruleList = new List<WatchListRule>();
                foreach (var Item in list)
                {
                    if (alertType == WatchlistAlertFrequency.Daily)
                        ruleList = Item.RuleList.Where(a => a.AlertSent && a.AlertFrequency == WatchlistAlertFrequency.Daily).ToList();
                    else if (alertType == WatchlistAlertFrequency.WhenAvailable)
                        ruleList = Item.RuleList.Where(a => a.AlertSent && a.AlertFrequency == WatchlistAlertFrequency.WhenAvailable).ToList();

                    if (ruleList.Count == 0) { continue; }
                    var SwntDataList = ExecuteWatchListSWNTModel(ruleList, typeof(SwntPerTransaction));
                    SwntDataList = GetLatestSWNTFilter(SwntDataList, Item.id, Item.UserId);                 
                    if (SwntDataList.Count > 0)
                    {
                        InsertSWNTinWatchListMapping(SwntDataList, Item);
                    }
                }
                return isSent;
            }
            catch (Exception ex)
            {
                applogs.AppLogManager("WatchListService", "WatchListAlertJob", "Error ExecuteWatchListMailAlertUNSC: " + ex.ToString());
                return false;
            }
        }




        public bool ExecuteWatchList(WatchlistAlertFrequency alertType, UprdDataSet dataSet)
        {
            try {
            bool isSent = false;
            List<WatchListDTO> list = new List<WatchListDTO>();
            var watchlists = UprdWatchListRepository.GetAllBydataSet(dataSet).ToList();
            foreach (var watchlist in watchlists)
            {
                WatchListDTO watchlistdto = new WatchListDTO();
                watchlistdto.id = watchlist.Id;
                watchlistdto.UserId = watchlist.UserId;
                    watchlistdto.DatasetId = watchlist.DataSetId;
                //switch (watchlist.DataSetId)
                //{
                //    case 1:
                //        watchlistdto.DatasetId = Enum.UprdDataSet.OACY;
                //        break;
                //    case 2:
                //        watchlistdto.DatasetId = Enum.UprdDataSet.UNSC;
                //        break;
                //    case 3:
                //        watchlistdto.DatasetId = Enum.UprdDataSet.SWNT;
                //        break;
                //}               
                watchlistdto.ListName = watchlist.Name;
                watchlistdto.UserEmail = watchlist.Email;
                watchlistdto.MoreDetailURLinAlert = watchlist.MoreDetailURLinAlert;
                watchlistdto.RuleList = GetWatchListRules(watchlist.Id);
                list.Add(watchlistdto);
            }

            List<WatchListRule> ruleList = new List<WatchListRule>();
            List<WatchListAlertExecutedDataDTO> resultantData = new List<WatchListAlertExecutedDataDTO>();
            foreach (var Item in list)
            {
                if (alertType == WatchlistAlertFrequency.Daily)
                    ruleList = Item.RuleList.Where(a => a.AlertSent && a.AlertFrequency == WatchlistAlertFrequency.Daily).ToList();
                else if (alertType == WatchlistAlertFrequency.WhenAvailable)
                    ruleList = Item.RuleList.Where(a => a.AlertSent && a.AlertFrequency == WatchlistAlertFrequency.WhenAvailable).ToList();

                if (ruleList.Count == 0) { continue; }

                WatchListAlertExecutedDataDTO dataList = new WatchListAlertExecutedDataDTO();
                dataList.watchList = Item;
                dataList.UserId = Item.UserId;
                switch (Item.DatasetId)
                {
                    case UprdDataSet.OACY:
                        dataList.OacyDataList = ExecuteWatchListOACY(ruleList, typeof(OACYPerTransaction));
                        dataList.OacyDataList = GetLatestOACYForMailing(dataList.OacyDataList, Item.id, Item.UserId);
                        SaveWatchListUprdForOACY(dataList.OacyDataList,Item);
                        dataList.OacyDataList = ApplyOACYGroupBy(dataList.OacyDataList);
                        if (dataList.OacyDataList.Count > 0) { resultantData.Add(dataList); }
                        break;
                    case UprdDataSet.UNSC:
                        dataList.UnscDataList = ExecuteWatchListUNSC(ruleList, typeof(UnscPerTransaction));
                        dataList.UnscDataList = GetLatestUNSCForMailing(dataList.UnscDataList, Item.id, Item.UserId);
                        SaveWatchListUprdForUNSC(dataList.UnscDataList,Item);
                        dataList.UnscDataList = ApplyUnscGroupby(dataList.UnscDataList);
                        if (dataList.UnscDataList.Count > 0) { resultantData.Add(dataList); }
                        break;
                    case UprdDataSet.SWNT:
                        dataList.SwntDataList = ExecuteWatchListSWNT(ruleList, typeof(SwntPerTransaction));
                        dataList.SwntDataList = GetLatestSWNTForMailing(dataList.SwntDataList, Item.id, Item.UserId);
                        SaveWatchListUprdForSWNT(dataList.SwntDataList,Item);
                        //dataList.SwntDataList = ApplySwntGroupBy(dataList.SwntDataList);
                        if (dataList.SwntDataList.Count > 0) { resultantData.Add(dataList); }
                        break;
                }
            }
            if (resultantData.Count > 0) { isSent = SendNotificationByTextOrEmail(resultantData); }
            return isSent;
            }
            catch (Exception ex)
            {
                applogs.AppLogManager("WatchListService", "WatchListAlertJob", "Error ExecuteWatchList: " + ex.ToString());
                return false;
            }
        }


        public void SaveWatchListUprdForOACY(List<OACYPerTransactionDTO> oacyList, WatchListDTO watchlist)
        {
            foreach (var uprdItem in oacyList)
            {
                WatchListMailAlertUPRDMapping model = new WatchListMailAlertUPRDMapping();
                model.WatchListId = watchlist.id;
                model.DataSetId = UprdDataSet.OACY;
                model.UserId = watchlist.UserId;
                model.EmailSentDateTime = DateTime.Now;
                model.UprdID = uprdItem.OACYID;
                UprdWatchListRepository.AddWatchListUprdMapping(model);

            }
        }


       


        public void SaveWatchListUprdForUNSC(List<UnscPerTransactionDTO> unscItem, WatchListDTO watchlist)
        {
            foreach (var uprdItem in unscItem)
            {
                WatchListMailAlertUPRDMapping model = new WatchListMailAlertUPRDMapping();
                model.WatchListId = watchlist.id;
                model.DataSetId = UprdDataSet.UNSC;
                model.UserId = watchlist.UserId;
                model.EmailSentDateTime = DateTime.Now;
                model.UprdID = uprdItem.UnscID;
                UprdWatchListRepository.AddWatchListUprdMapping(model);
            }
        }

        public void SaveWatchListUprdForSWNT(List<SwntPerTransactionDTO> swntList, WatchListDTO watchlist)
        {
            foreach (var uprdItem in swntList)
            {
                WatchListMailAlertUPRDMapping model = new WatchListMailAlertUPRDMapping();
                model.WatchListId = watchlist.id;
                model.DataSetId = UprdDataSet.SWNT;
                model.UserId = watchlist.UserId;
                model.EmailSentDateTime = DateTime.Now;
                model.UprdID = uprdItem.Id;
                UprdWatchListRepository.AddWatchListUprdMapping(model);
            }
        }


        public List<SwntPerTransactionDTO> ApplySwntGroupBy(List<SwntPerTransactionDTO> oldList)
        {
            List<SwntPerTransactionDTO> newData = new List<SwntPerTransactionDTO>();
            if (oldList.Count > 1)
            {
                newData = (from a in oldList
                           group a by new
                           {
                               a.NoticeId,
                               a.PostingDateTime.Value.Date,
                               a.TransportationserviceProvider
                           } into s
                           select s.OrderByDescending(a => a.PostingDateTime).FirstOrDefault()).ToList();
            }
            else {
                return oldList;
            }
            return newData;
        }

        public  List<SwntPerTransactionDTO> GetLatestSWNTForMailing(List<SwntPerTransactionDTO> swntdata, int watchListId, string userId)
        {
            var mapdata = UprdWatchListRepository.GetDataForUPRDMapping(userId, watchListId, UprdDataSet.SWNT).Select(a => a.UprdID).ToList();
            if (mapdata == null || mapdata.Count == 0 || swntdata == null || swntdata.Count == 0) { return swntdata; }
            var result = swntdata.Where(a => !mapdata.Contains(a.Id)).ToList();
            return result;
        }

        public List<SwntPerTransaction> GetLatestSWNTFilter(List<SwntPerTransaction> swntdata, int watchListId, string userId)
        {
            var mapdata = UprdWatchListRepository.GetDataForSWNTmappingOnly(userId, watchListId).Select(a => a.SWNTID).ToList();
            if (mapdata == null || mapdata.Count == 0 || swntdata == null || swntdata.Count == 0) { return swntdata; }
            var result = swntdata.Where(a => !mapdata.Contains(a.Id)).ToList();
            return result;
        }

        public  List<UnscPerTransactionDTO> ApplyUnscGroupby(List<UnscPerTransactionDTO> oldList)
        {
            List<UnscPerTransactionDTO> newData = new List<UnscPerTransactionDTO>();
            if (oldList.Count > 1)
            {
                newData = (from a in oldList
                           group a by new
                           {
                               a.Loc,
                               post = a.PostingDate.Value.Date,
                               eff = a.EffectiveGasDay.Value.Date,
                               a.LocQTIDesc
                           } into s
                           select s.OrderByDescending(a => a.PostingDate).FirstOrDefault()).ToList();

            }
            else
            {
                return oldList;
            }
            return newData;
        }

        public  List<UnscPerTransactionDTO> GetLatestUNSCForMailing(List<UnscPerTransactionDTO> unscdata, int watchListId, string userId)
        {
            var mapdata = UprdWatchListRepository.GetDataForUPRDMapping(userId, watchListId, UprdDataSet.UNSC).Select(a => a.UprdID).ToList();
            if (mapdata == null || mapdata.Count == 0 || unscdata == null || unscdata.Count == 0) { return unscdata; }
            var result = unscdata.Where(a => !mapdata.Contains(a.UnscID)).ToList();
            return result;
        }

        public List<UnscPerTransaction> GetLatestUNSCFilter(List<UnscPerTransaction> unscdata, int watchListId, string userId)
        {
            var mapdata = UprdWatchListRepository.GetDataForUNSCMappingOnly(userId, watchListId).Select(a => a.UNSCID).ToList();
            if (mapdata == null || mapdata.Count == 0 || unscdata == null || unscdata.Count == 0) { return unscdata; }
            var result = unscdata.Where(a => !mapdata.Contains(a.UnscID)).ToList();
            return result;
        }

        public List<UnscPerTransaction> ExecuteWatchListUNSCModel(List<WatchListRule> RuleList, Type type)
        {
            UprdUnscRepository unsc = new UprdUnscRepository();
            ModalFactory factory = new ModalFactory();
            IEnumerable<UnscPerTransaction> dataList = new List<UnscPerTransaction>();
            List<AbstractSearch> searchList;
            foreach (var rule in RuleList)
            {
                searchList = new List<AbstractSearch>();
                string name = UprdWatchListRepository.GetByIdMasterColumn(rule.PropertyId).Name;
                var property = type.GetProperties()
                .Where(p => p.CanRead && p.CanWrite && (p.Name == name)).FirstOrDefault();
                var searchProperty = SearchExtensions.CreateSearchCriteria(type, property.PropertyType, property.Name, UprdWatchListRepository.GetByIdLogicalOperator(rule.ComparatorsId).Name, rule.value);
                searchList.Add(searchProperty);
                var unscList = unsc.GetByPipelineLoc(rule.PipelineDuns, rule.LocationIdentifier).ApplySearchCriterias(searchList).ToList();
                dataList = dataList.Union(unscList);
            }
            return dataList.ToList();
        }


        public List<UnscPerTransactionDTO> ExecuteWatchListUNSC(List<WatchListRule> RuleList, Type type)
        {
            UprdUnscRepository unsc = new UprdUnscRepository();
            ModalFactory factory = new ModalFactory();
            IEnumerable<UnscPerTransaction> dataList = new List<UnscPerTransaction>();
            List<AbstractSearch> searchList;
            foreach (var rule in RuleList)
            {
                searchList = new List<AbstractSearch>();
                string name = UprdWatchListRepository.GetByIdMasterColumn(rule.PropertyId).Name;
                var property = type.GetProperties()
                .Where(p => p.CanRead && p.CanWrite && (p.Name == name)).FirstOrDefault();
                var searchProperty = SearchExtensions.CreateSearchCriteria(type, property.PropertyType, property.Name, UprdWatchListRepository.GetByIdLogicalOperator(rule.ComparatorsId).Name, rule.value);
                searchList.Add(searchProperty);
                var unscList = unsc.GetByPipelineLoc(rule.PipelineDuns, rule.LocationIdentifier).ApplySearchCriterias(searchList).ToList();
                dataList = dataList.Union(unscList);
            }
            return dataList.OrderByDescending(a => a.PostingDateTime).Select(a => factory.Parse(a)).ToList();
        }

        public  List<OACYPerTransactionDTO> ApplyOACYGroupBy(List<OACYPerTransactionDTO> oldList)
        {
            List<OACYPerTransactionDTO> newData = new List<OACYPerTransactionDTO>();
            if (oldList.Count > 1)
            {
                newData = (from a in oldList
                           group a by new
                           {
                               a.Loc,
                               a.CycleIndicator,
                               a.TotalScheduleQty,
                               eff = a.EffectiveGasDay.Value.Date,
                               post = a.PostingDate.Value.Date,
                               a.FlowIndicator,
                               a.ITIndicator

                           } into s
                           select s.OrderByDescending(a => a.PostingDate).FirstOrDefault()).ToList();

            }
            else
            {
                return oldList;
            }
            return newData;
        }

        public  List<OACYPerTransactionDTO> GetLatestOACYForMailing(List<OACYPerTransactionDTO> oacydata, int watchListId, string userId)
        {
            var mapdata = UprdWatchListRepository.GetDataForUPRDMapping(userId, watchListId, UprdDataSet.OACY).Select(a => a.UprdID).ToList();
            if (mapdata == null || mapdata.Count == 0 || oacydata == null || oacydata.Count == 0) { return oacydata; }
            var result = oacydata.Where(a => !mapdata.Contains(a.OACYID)).ToList();
            return result;
        }

        public List<OACYPerTransaction> GetLatestOACYValidation(List<OACYPerTransaction> oacydata, int watchListId, string userid)
        {
            var mapdata = UprdWatchListRepository.GetDataFromMailMappingOACY(userid, watchListId).Select(a => a.OACYID).ToList();
            if (mapdata == null || mapdata.Count == 0 || oacydata == null || oacydata.Count == 0) { return oacydata; }
            var result = oacydata.Where(a => !mapdata.Contains(a.OACYID)).ToList();
            return result;
        }

        public List<OACYPerTransactionDTO> GetLatestOACYsForMailingMapping(List<OACYPerTransactionDTO> oacydata, int watchListId, string userId)
        {
            var mapdata = UprdWatchListRepository.GetDataForOACYMapping(userId, watchListId).Select(a => a.OACYID).ToList();
            if (mapdata == null || mapdata.Count == 0 || oacydata == null || oacydata.Count == 0) { return oacydata; }
            var result = oacydata.Where(a => !mapdata.Contains(a.OACYID)).ToList();
            return result;
        }



        public  List<OACYPerTransactionDTO> ExecuteWatchListOACY(List<WatchListRule> RuleList, Type type)
        {
            UprdOACYRepository oacy = new UprdOACYRepository();
            ModalFactory factory = new ModalFactory();
            IEnumerable<OACYPerTransaction> dataList = new List<OACYPerTransaction>();
            //List<AbstractSearch> searchList;
            foreach (var rule in RuleList)
            {
                string name = UprdWatchListRepository.GetByIdMasterColumn(rule.PropertyId).Name;
                IQueryable<OACYPerTransaction> query = oacy.GetByPipelineLoc(rule.PipelineDuns, rule.LocationIdentifier);
                decimal minVal = Convert.ToDecimal(rule.value);
                decimal maxVal = Convert.ToDecimal(rule.UpperValue);
                switch (name)
                {
                    case "AvailablePercentage":
                        query = query.Where(a => a.AvailablePercentage >= minVal && a.AvailablePercentage <= maxVal);
                        break;
                    case "OperationallyAvailableQty":
                        query = query.Where(a => a.OperationallyAvailableQty >= minVal && a.OperationallyAvailableQty <= maxVal);
                        break;
                }
                var oacyList = query.ToList();
                //searchList = new List<AbstractSearch>();
                //string name = UprdWatchListRepository.GetByIdMasterColumn(rule.PropertyId).Name;
                //var property = type.GetProperties()
                //.Where(p => p.CanRead && p.CanWrite && (p.Name == name)).FirstOrDefault();
                //var searchProperty = SearchExtensions.CreateSearchCriteria(type, property.PropertyType, property.Name, UprdWatchListRepository.GetByIdLogicalOperator(rule.ComparatorsId).Name, rule.value);
                //searchList.Add(searchProperty);
                //var oacyList = oacy.GetByPipelineLoc(rule.PipelineDuns, rule.LocationIdentifier).ApplySearchCriterias(searchList).ToList();
                dataList = oacyList; // dataList.Union(oacyList);
            }
            return dataList.OrderByDescending(a => a.PostingDateTime).Select(a => factory.Parse(a)).ToList();
        }


    }
}