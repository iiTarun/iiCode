using Nom.ViewModel;
using Nom1Done.Data.Repositories;
using Nom1Done.DTO;
using Nom1Done.Enums;
using Nom1Done.Model;
using Nom1Done.Model.Models;
using Nom1Done.Service.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Nom1Done.Service
{
   public class WatchlistService: IWatchlistService
    {
        IWatchlistRepository IWatchListRepo;
        IWatchlistRuleRepository IWatchListRuleRepo;
        IMasterColumnRepository IMasterColumnRepo;
        ILogicalOperatorRepository ILogicalOperatorRepo;
        IDataTypeGroupingRepository IdataTypeGroupRepo;
        INoticesRepository INoticesRepository;
        IOACYRepository IOacyRepository;
        IUNSCRepository IUNSCRepository;
        IModalFactory ImodelFactory;
        IWatchListPipelineMappingRepository IWatchListPipelineMappingRepository;
        IPipelineRepository IPipelineRepository;
        IIdentityUsersRepo  IdentityUsersRepo;
        IWatchListMailAlertUPRDMappingRepo IWatchListMailAlertUPRDMappingRepo;
        IEmailQueueService IemailQueueService;


        public WatchlistService(
            IEmailQueueService IemailQueueService,
             IWatchListMailAlertUPRDMappingRepo IWatchListMailAlertUPRDMappingRepo,
            IIdentityUsersRepo IdentityUsersRepo,
            IPipelineRepository IPipelineRepository,
            IWatchListPipelineMappingRepository IWatchListPipelineMappingRepository,
            IUNSCRepository IUNSCRepository,
            IOACYRepository IOacyRepository,
            IModalFactory ImodelFactory,
            INoticesRepository INoticesRepository,
            IWatchlistRepository IWatchListRepo,
            IWatchlistRuleRepository IWatchListRuleRepo,
            IMasterColumnRepository IMasterColumnRepo,
            ILogicalOperatorRepository ILogicalOperatorRepo,
            IDataTypeGroupingRepository IdataTypeGroupRepo
            )
        {
            this.IemailQueueService = IemailQueueService;
            this.IWatchListMailAlertUPRDMappingRepo=IWatchListMailAlertUPRDMappingRepo;
            this.IdentityUsersRepo  = IdentityUsersRepo;
            this.IWatchListPipelineMappingRepository=IWatchListPipelineMappingRepository;
            this.IUNSCRepository=IUNSCRepository;
            this.IOacyRepository = IOacyRepository;
            this.ImodelFactory = ImodelFactory;
            this.INoticesRepository = INoticesRepository;
            this.IWatchListRepo = IWatchListRepo;
            this.IWatchListRuleRepo = IWatchListRuleRepo;
            this.IMasterColumnRepo = IMasterColumnRepo;
            this.ILogicalOperatorRepo = ILogicalOperatorRepo;
            this.IdataTypeGroupRepo = IdataTypeGroupRepo;
            this.IPipelineRepository = IPipelineRepository;
        }

        public int SaveWatchList(WatchListDTO watchList)
        {
            int watchListId = SaveWatchListTableData(watchList);
            foreach (var watchlistRule in watchList.RuleList)
            {
               IWatchListRuleRepo.AddWatchListRule(watchListId, watchlistRule);
            }
            return watchListId;
        }

        private int SaveWatchListTableData(WatchListDTO watchList)
        {
            return IWatchListRepo.AddWatchList(watchList.ListName,watchList.DatasetId,watchList.UserId);
        }


        public bool DeleteWatchListById(int watchListid)
        {
            try
            {
                var watchlist = IWatchListRepo.GetById(watchListid);
                IWatchListRepo.Delete(watchlist);
                IWatchListRepo.SaveChages();
                var watchListsRules = IWatchListRuleRepo.GetByWatchListId(watchListid).ToList();
                foreach (var item in watchListsRules) { IWatchListRuleRepo.Delete(item); IWatchListRuleRepo.SaveChages(); }             
               
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      

        public WatchListDTO GetWatchListById(int watchListId)
        {
            var watchlist = IWatchListRepo.GetById(watchListId);
            WatchListDTO watchlistdto = new WatchListDTO();
            watchlistdto.id = watchlist.Id;          
            watchlistdto.UserId = watchlist.UserId;
            watchlistdto.DatasetId = watchlist.DataSetId;
            watchlistdto.ListName = watchlist.Name;           
            watchlistdto.RuleList = GetWatchListRules(watchlist.Id);
            return watchlistdto;
        }

        public List<WatchListDTO> GetWatchListByUserId(string userId)
        {
            List<WatchListDTO> list = new List<WatchListDTO>();
            var watchlists = IWatchListRepo.GetByUserId(userId).OrderByDescending(a=>a.CreatedDate).ToList();
            foreach (var watchlist in watchlists)
            {
                WatchListDTO watchlistdto = new WatchListDTO();
                watchlistdto.id = watchlist.Id;                
                watchlistdto.UserId = watchlist.UserId;
                watchlistdto.DatasetId = watchlist.DataSetId;
                watchlistdto.ListName = watchlist.Name;               
                watchlistdto.RuleList = GetWatchListRules(watchlist.Id);              
                list.Add(watchlistdto);
            }
            return list;
        }

        public List<WatchListDTO> GetWatchList(int pipelineId,string userId, EnercrossDataSets dataset)
        {
            List<WatchListDTO> list = new List<WatchListDTO>();            
            var watchlists = IWatchListRepo.GetByDataSet(pipelineId,dataset,userId).ToList();
            foreach (var watchlist in watchlists)
            {
                WatchListDTO watchlistdto = new WatchListDTO();
                watchlistdto.id = watchlist.Id;
               // watchlistdto.PipelineId = pipelineId;
                watchlistdto.UserId = userId;
                watchlistdto.DatasetId = dataset;
                watchlistdto.ListName = watchlist.Name;
               // watchlistdto.AlertSent = watchlist.AlertSent;
              //  watchlistdto.AlertFrequency = watchlist.AlertFrequency;
                watchlistdto.RuleList = GetWatchListRules(watchlist.Id);
               list.Add(watchlistdto);
            }           
            return list;
        }

        private List<WatchListRule> GetWatchListRules(int watchListId)
        {
            var modellist = IWatchListRuleRepo.GetByWatchListId(watchListId).ToList();
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
                dto.AlertFrequency = item.AlertFrequency;
                dto.AlertSent = item.AlertSent;
                dto.IsCriticalNotice = item.IsCriticalNotice;
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public bool UpdateWatchList(WatchListDTO watchListDto)
        {
            var watchlistModel = IWatchListRepo.GetById(watchListDto.id);

            watchlistModel.Name = watchListDto.ListName;
            watchlistModel.DataSetId = watchListDto.DatasetId;        
            watchlistModel.ModifiedDate = DateTime.UtcNow;
            IWatchListRepo.Update(watchlistModel);
            IWatchListRepo.SaveChages();
            
             
                var list = IWatchListRuleRepo.GetByWatchListId(watchlistModel.Id).ToList();
                foreach (var item in list)
                {
                    IWatchListRuleRepo.Delete(item);
                    IWatchListRuleRepo.SaveChages();
                }
                foreach (var newItem in watchListDto.RuleList)
                {
                    IWatchListRuleRepo.AddWatchListRule(watchListDto.id,newItem);
                }                
          
            return true;
        }

        public bool ExecuteWatchList(WatchlistAlertFrequency alertType, EnercrossDataSets dataSet)
        {
            bool isSent = false;
            List<WatchListDTO> list = new List<WatchListDTO>();
            var watchlists = IWatchListRepo.GetAllBydataSet(dataSet).ToList();
            foreach (var watchlist in watchlists)
            {
                WatchListDTO watchlistdto = new WatchListDTO();
                watchlistdto.id = watchlist.Id;
                watchlistdto.UserId = watchlist.UserId;
                watchlistdto.DatasetId = watchlist.DataSetId;
                watchlistdto.ListName = watchlist.Name;
                watchlistdto.RuleList = GetWatchListRules(watchlist.Id);
                list.Add(watchlistdto);
            }

            List<WatchListRule> ruleList = new List<WatchListRule>();           
            List<WatchListAlertExecutedDataDTO> resultantData = new List<WatchListAlertExecutedDataDTO>();
            foreach (var Item in list)
            {
                if(alertType==WatchlistAlertFrequency.Daily)
                   ruleList = Item.RuleList.Where(a => a.AlertSent && a.AlertFrequency==WatchlistAlertFrequency.Daily).ToList();
                else if (alertType == WatchlistAlertFrequency.WhenAvailable)
                   ruleList = Item.RuleList.Where(a=>a.AlertSent && a.AlertFrequency==WatchlistAlertFrequency.WhenAvailable).ToList();

                if (ruleList.Count == 0) { continue; }

                WatchListAlertExecutedDataDTO dataList = new WatchListAlertExecutedDataDTO();
                dataList.watchList = Item;
                dataList.UserId = Item.UserId;
                switch (Item.DatasetId)
                {
                    case EnercrossDataSets.OACY:
                        dataList.OacyDataList = ExecuteWatchListOACY(ruleList,typeof(OACYPerTransaction));
                        dataList.OacyDataList = GetLatestOACYForMailing(dataList.OacyDataList,Item.id,Item.UserId);
                        dataList.OacyDataList = ApplyOACYGroupBy(dataList.OacyDataList);
                        if (dataList.OacyDataList.Count > 0) { resultantData.Add(dataList); }
                        break;
                    case EnercrossDataSets.UNSC:
                        dataList.UnscDataList = ExecuteWatchListUNSC(ruleList,typeof(UnscPerTransaction));
                        dataList.UnscDataList = GetLatestUNSCForMailing(dataList.UnscDataList, Item.id, Item.UserId);
                        dataList.UnscDataList = ApplyUnscGroupby(dataList.UnscDataList);
                        if (dataList.UnscDataList.Count > 0) { resultantData.Add(dataList); }
                        break;
                    case EnercrossDataSets.SWNT:
                        dataList.SwntDataList = ExecuteWatchListSWNT(ruleList,typeof(SwntPerTransaction));
                        dataList.SwntDataList = GetLatestSWNTForMailing(dataList.SwntDataList, Item.id, Item.UserId);
                        dataList.SwntDataList = ApplySwntGroupBy(dataList.SwntDataList);
                        if (dataList.SwntDataList.Count > 0) { resultantData.Add(dataList); }
                        break;

                }   
            }
            
            if (resultantData.Count > 0) { isSent = SendNotificationByTextOrEmail(resultantData); }            
            return isSent;
        }


        public List<OACYPerTransactionDTO> ApplyOACYGroupBy(List<OACYPerTransactionDTO> oldList)
        {
            List<OACYPerTransactionDTO> newData = new List<OACYPerTransactionDTO>();
            if (oldList.Count > 1) {
                 newData = (from a in oldList
                            group a by new
                            {
                                a.Loc,
                                a.CycleIndicator,
                                a.TotalScheduleQty,
                                eff = a.EffectiveGasDay.Date,
                                post = a.PostingDate.Date,
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

        public List<UnscPerTransactionDTO> ApplyUnscGroupby(List<UnscPerTransactionDTO> oldList)
        {
            List<UnscPerTransactionDTO> newData = new List<UnscPerTransactionDTO>();
            if (oldList.Count > 1)
            {
                newData = (from a in oldList
                           group a by new
                           {
                               a.Loc,
                               post = a.PostingDate.Date,
                               eff = a.EffectiveGasDay.Date,
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


        public List<OACYPerTransactionDTO> GetLatestOACYForMailing(List<OACYPerTransactionDTO> oacydata,int watchListId,string userId)
        {
            var mapdata = IWatchListMailAlertUPRDMappingRepo.GetData(userId,watchListId,EnercrossDataSets.OACY).Select(a=>a.UprdID).ToList();
            if (mapdata == null || mapdata.Count == 0 || oacydata == null || oacydata.Count == 0) { return oacydata; }
            var result = oacydata.Where(a => !mapdata.Contains(a.OACYID)).ToList();
            return result;
        }

        public List<UnscPerTransactionDTO> GetLatestUNSCForMailing(List<UnscPerTransactionDTO> unscdata, int watchListId, string userId)
        {
            var mapdata = IWatchListMailAlertUPRDMappingRepo.GetData(userId, watchListId, EnercrossDataSets.UNSC).Select(a => a.UprdID).ToList();
            if (mapdata == null || mapdata.Count == 0 || unscdata == null || unscdata.Count == 0) { return unscdata; }
            var result = unscdata.Where(a => !mapdata.Contains(a.UnscID)).ToList();
            return result;
        }

        public List<SwntPerTransactionDTO> GetLatestSWNTForMailing(List<SwntPerTransactionDTO> swntdata, int watchListId, string userId)
        {
            var mapdata = IWatchListMailAlertUPRDMappingRepo.GetData(userId, watchListId, EnercrossDataSets.SWNT).Select(a => a.UprdID).ToList();
            if (mapdata == null || mapdata.Count == 0 || swntdata == null || swntdata.Count == 0) { return swntdata; }
            var result = swntdata.Where(a => !mapdata.Contains(a.Id)).ToList();
            return result;
        }

        public bool SendNotificationByTextOrEmail(List<WatchListAlertExecutedDataDTO> alertExeDataCollection)
        {
            try
            {
                var userIds = alertExeDataCollection.Select(a => a.UserId).Distinct();
                foreach (var userId in userIds)
                {
                    var WatchListDataByuser = alertExeDataCollection.Where(a => a.UserId == userId).ToList();

                    bool IsMappingSave = SaveWatchListAlertUPRDMap(WatchListDataByuser); // TO keep record of sending data in Mail

                    #region Get UserInfo - mailaddress and PhoneNumber

                    var userEmail = IdentityUsersRepo.GetUserEmailByUserId(userId);
                    var UserPhone = IdentityUsersRepo.GetUserPhoneByUserId(userId);

                    #endregion

                    #region SendMail

                    string subject = "NatGasHub WatchList Alerts";
                    if (WatchListDataByuser.FirstOrDefault().watchList.DatasetId == EnercrossDataSets.OACY)
                    {
                        subject = "Pipeline Alert: Operationally Available Capacity";
                    } else if (WatchListDataByuser.FirstOrDefault().watchList.DatasetId == EnercrossDataSets.UNSC)
                    {
                        subject = "Pipeline Alert: Unsubscribed Capacity";
                    }
                    else if (WatchListDataByuser.FirstOrDefault().watchList.DatasetId == EnercrossDataSets.SWNT)
                    {
                        subject = "Pipeline Alert: System Wide Notices";
                    }
                    string content = BuildEmailHtmlTemplate(WatchListDataByuser);


                 // string[] recipients = new[] { "vijay.kumar@invertedi.com" };//,"neha.sharma@invertedi.com","Jay.singh@enercross.com" };
                  // subject+=userEmail;


                    string from = ConfigurationManager.AppSettings.Get("EmailIdForAlert");
                    string[] recipients = new[] { userEmail };                   

                    var isSend = EmailandSMSservice.SendGmail(subject, content, recipients, from);
                    if (isSend) {
                        EmailQueueDto emailDto = new EmailQueueDto();
                        emailDto.ToUserID = userId;
                        emailDto.Subject = subject;
                        emailDto.Email = content;
                        emailDto.Recipient = recipients.FirstOrDefault();
                        emailDto.IsSent = isSend;
                        emailDto.CreatedDate = DateTime.Now;
                        emailDto.SentDate = DateTime.Now;
                        IemailQueueService.AddEmailQueue(emailDto);

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
                return false;
            }
        }


        

        public bool SaveWatchListAlertUPRDMap(List<WatchListAlertExecutedDataDTO> alertExeDataCollection)
        {
            try
            {
                foreach (var dataItem in alertExeDataCollection)
                {

                    if (dataItem.OacyDataList != null && dataItem.OacyDataList.Count > 0)
                    {
                        foreach (var uprdItem in dataItem.OacyDataList)
                        {
                            WatchListMailAlertUPRDMapping model = new WatchListMailAlertUPRDMapping();
                            model.WatchListId = dataItem.watchList.id;
                            model.DataSetId = dataItem.watchList.DatasetId;
                            model.UserId = dataItem.UserId;
                            model.EmailSentDateTime = DateTime.Now;
                            model.UprdID = uprdItem.OACYID;
                            IWatchListMailAlertUPRDMappingRepo.Add(model);
                            IWatchListMailAlertUPRDMappingRepo.SaveChages();

                        }
                    }
                    else if (dataItem.UnscDataList != null && dataItem.UnscDataList.Count > 0)
                    {
                        foreach (var uprdItem in dataItem.UnscDataList)
                        {
                            WatchListMailAlertUPRDMapping model = new WatchListMailAlertUPRDMapping();
                            model.WatchListId = dataItem.watchList.id;
                            model.DataSetId = dataItem.watchList.DatasetId;
                            model.UserId = dataItem.UserId;
                            model.EmailSentDateTime = DateTime.Now;
                            model.UprdID = uprdItem.UnscID;
                            IWatchListMailAlertUPRDMappingRepo.Add(model);
                            IWatchListMailAlertUPRDMappingRepo.SaveChages();
                        }
                    }
                    else if (dataItem.SwntDataList != null && dataItem.SwntDataList.Count > 0)
                    {
                        foreach (var uprdItem in dataItem.SwntDataList)
                        {
                            WatchListMailAlertUPRDMapping model = new WatchListMailAlertUPRDMapping();
                            model.WatchListId = dataItem.watchList.id;
                            model.DataSetId = dataItem.watchList.DatasetId;
                            model.UserId = dataItem.UserId;
                            model.EmailSentDateTime = DateTime.Now;
                            model.UprdID = uprdItem.Id;
                            IWatchListMailAlertUPRDMappingRepo.Add(model);
                            IWatchListMailAlertUPRDMappingRepo.SaveChages();
                        }
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public string BuildEmailHtmlTemplate(List<WatchListAlertExecutedDataDTO> alertExeDataCollection)
        {
           
            string viewmoreurl = ConfigurationManager.AppSettings.Get("ViewMoreLinkForAlert");
            string emailbody = "<div style=\" display: inline-block; width:100%;height:600px;\"><div style=\" display: inline-block;width:10%;height:20px;\"></div><div class=\"content\" style=\" display: inline-block;width:75%;height:600px; background:#fff;\"><div style=\"width:100%;height:80px; background:#ffff;\"><img src=\"http://test.natgashub.com/Assets/OrangeNom1DoneLogo.jpg\" alt =\"NatGasHub\" height=\"50px\" width=\"70px\"/><b>";
           // emailbody += "NatGasHub Alert</b></div><div style=\"width:100%;min-height: 100px;overflow: hidden; background:#ffff;\">";
            if (alertExeDataCollection.FirstOrDefault().watchList.DatasetId == EnercrossDataSets.OACY)
            {
                emailbody += "  OPERATIONALLY AVAILABLE CAPACITY ALERT";
            }
            else if (alertExeDataCollection.FirstOrDefault().watchList.DatasetId == EnercrossDataSets.UNSC)
            {
                emailbody += "  UNSUBSCRIBED CAPACITY ALERT";
            }
            else if (alertExeDataCollection.FirstOrDefault().watchList.DatasetId == EnercrossDataSets.SWNT)
            {
                emailbody += "  SYSTEM WIDE NOTICES ALERT";
            }

            emailbody += "</b></div><div style=\"width:100%;min-height: 100px;overflow: hidden; background:#ffff;\">";

            foreach (var watchListdata in alertExeDataCollection)
            {
                int count = 1;
                int totalRowLimit = 5;
                string tableStr = "";
                if (watchListdata.OacyDataList != null && watchListdata.OacyDataList.Count > 0) {
                    tableStr += "<h3> WatchList Name: " + watchListdata.watchList.ListName + "</h3>";
                    tableStr += "<table width=\"100%\" bgcolor=\"#f6f8f1\" border=\"0\" cellpadding=\"5\" cellspacing=\"0\">";
                    tableStr += "<thead><tr style=\"background-color:#FF6C3A; color:#fff; height:30px\"><th style=\"text-align:center;\">Cycle</th><th style=\"text-align:center;\">Pipeline</th><th style=\"text-align:center;\">Loc Prop</th><th style=\"text-align:center;\">Loc Name</th><th style=\"text-align:center;\">Operating Cap</th><th style=\"text-align:center;\">Scheduled Qty</th><th style=\"text-align:center;\">% Available</th></tr></thead>";
                    tableStr += "<tbody>";
                    foreach (var oacy in watchListdata.OacyDataList.OrderBy(a=>a.PipelineNameDuns).ThenBy(a=>a.Loc))
                    {                       
                        tableStr += "<tr><td style=\"text-align:center;\">" + oacy.CycleIndicator + "</td>";
                        tableStr += "<td style=\"text-align:center;\"><a href=\""+ viewmoreurl + "/MOperationalCapacity/index?pipelineId="+oacy.PipelineID + "\" style =\"color:#FF6C3A\">" + oacy.PipelineNameDuns + "</a></td>";
                        tableStr += "<td style=\"text-align:center;\">" + oacy.Loc + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + oacy.LocName + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + oacy.OperatingCapacity + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + oacy.TotalScheduleQty + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + oacy.AvailablePercentage + "</td></tr>";
                        count++;
                        if (count > totalRowLimit) {                            
                            break;
                        }
                    }
                    tableStr += "</tbody></table>";
                    //if (count > totalRowLimit) {
                    //    tableStr += "<div style=\"width:100%;height:50px; background:#ffff;padding-top: 10px;\"><a href=\""+ viewmoreurl + "/WatchList/List?pipelineId=1\" style =\"color:#FF6C3A\"> View More</a></div>";
                    //   // emailbody = emailbody + tableStr;
                    //   // break;
                    //}
                   
                }
                else if(watchListdata.UnscDataList != null && watchListdata.UnscDataList.Count > 0) {
                    tableStr += "<h3> WatchList Name: " + watchListdata.watchList.ListName + "</h3>";
                    tableStr += "<table width=\"100%\" bgcolor=\"#f6f8f1\" border=\"0\" cellpadding=\"5\" cellspacing=\"0\">";
                    tableStr += "<thead><tr style=\"background-color:#FF6C3A; color:#fff; height:30px\"><th style=\"text-align:center;\">Pipeline</th><th style=\"text-align:center;\">Loc Prop</th><th style=\"text-align:center;\">Loc Name</th><th style=\"text-align:center;\">Unsubscribed Cap Today</th><th style=\"text-align:center;\">% Change</th></tr></thead>";
                    tableStr += "<tbody>";
                    foreach (var unsc in watchListdata.UnscDataList.OrderBy(a => a.PipelineNameDuns).ThenBy(a => a.Loc))
                    {                       
                        tableStr += "<tr><td style=\"text-align:center;\"><a href=\"" + viewmoreurl + "/MUnsubscribedCapacity/index?pipelineId=" + unsc.PipelineID + "\" style =\"color:#FF6C3A\">" + unsc.PipelineNameDuns + "</a></td>";
                        tableStr += "<td style=\"text-align:center;\">" + unsc.Loc + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + unsc.LocName + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + unsc.UnsubscribeCapacity + "</td>";                        
                        tableStr += "<td style=\"text-align:center;\">" + unsc.ChangePercentage + "</td></tr>";
                        count++;
                        if (count > totalRowLimit)
                        {
                            break;
                        }
                    }
                    tableStr += "</tbody></table>";
                    //if (count > totalRowLimit)
                    //{
                    //    tableStr += "<div style=\"width:100%;height:50px; background:#ffff;padding-top: 10px;\"><a href=\"" + viewmoreurl + "/WatchList/List?pipelineId=1\" style=\"color:#FF6C3A\"> View More</a></div>";
                    //   // emailbody = emailbody + tableStr;
                    //    //break;
                    //}
                }
                else if (watchListdata.SwntDataList != null && watchListdata.SwntDataList.Count > 0) {
                    tableStr += "<h3>  WatchList Name: " + watchListdata.watchList.ListName + "</h3>";
                    tableStr += "<table width=\"100%\" bgcolor=\"#f6f8f1\" border=\"0\" cellpadding=\"5\" cellspacing=\"0\">";
                    tableStr += "<thead><tr style=\"background-color:#FF6C3A; color:#fff; height:30px\"><th style=\"text-align:center;\">Type</th><th style=\"text-align:center;\">NoticeID</th><th style=\"text-align:center;\">Pipeline</th><th style=\"text-align:center;\">Eff Date</th><th style=\"text-align:center;\">Post Date</th><th style=\"text-align:center;\"> Category</th><th style=\"text-align:center;\">Subject</th></tr></thead>";
                    tableStr += "<tbody>";
                    var noticeUrl = "&IsCritical=false";
                    foreach (var swnt in watchListdata.SwntDataList.OrderBy(a => a.PostingDateTime).ThenBy(a => a.PipelineNameDuns))
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
                        count++;
                        if (count > totalRowLimit)
                        {
                            break;
                        }
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

        public List<SwntPerTransactionDTO> ExecuteWatchListWithNotices(int pipelineId,WatchListDTO watchListDto,Type type)
        {
            List<AbstractSearch> searchList = new List<AbstractSearch>();               
            foreach (var rule in watchListDto.RuleList)
            {                
              string name = IMasterColumnRepo.GetById(rule.PropertyId).Name;              
              var property = type.GetProperties()
              .Where(p => p.CanRead && p.CanWrite && (p.Name==name)).FirstOrDefault();
              var searchProperty = SearchExtensions.CreateSearchCriteria(type, property.PropertyType, property.Name,ILogicalOperatorRepo.GetById(rule.ComparatorsId).Name,rule.value);
              searchList.Add(searchProperty);
            }
            /////Only For SWNT
            var noticeList = new List<SwntPerTransaction>(); //INoticesRepository.GetNotice(pipelineId, watchListDto.isCriticalnotice).ApplySearchCriterias(searchList).ToList();//.Select(a=> ImodelFactory.Parse(a)).ToList();
           
            return noticeList.Select(a => ImodelFactory.Parse(a)).ToList();
        }


        public List<OACYPerTransactionDTO> ExecuteWatchListWithOACY(int pipelineId,WatchListDTO watchListDto, Type type)
        {
            List<AbstractSearch> searchList = new List<AbstractSearch>();
            foreach (var rule in watchListDto.RuleList)
            {
                string name = IMasterColumnRepo.GetById(rule.PropertyId).Name;
                var property = type.GetProperties()
                .Where(p => p.CanRead && p.CanWrite && (p.Name == name)).FirstOrDefault();
                var searchProperty = SearchExtensions.CreateSearchCriteria(type, property.PropertyType, property.Name, ILogicalOperatorRepo.GetById(rule.ComparatorsId).Name, rule.value);
                searchList.Add(searchProperty);
            }
            /////Only For OACY
            var pipeline = IPipelineRepository.GetById(pipelineId);
            var oacyList = IOacyRepository.GetByPipeline(pipeline.DUNSNo).ApplySearchCriterias(searchList).ToList();//.Select(a=> ImodelFactory.Parse(a)).ToList();

            return oacyList.Select(a => ImodelFactory.Parse(a)).ToList();
        }


        public List<SwntPerTransactionDTO> ExecuteWatchListSWNT(List<WatchListRule> RuleList, Type type)
        {
            IEnumerable<SwntPerTransaction> dataList = new List<SwntPerTransaction>();
            List<AbstractSearch> searchList;
            foreach (var rule in RuleList)
            {
               // var pipeline = IPipelineRepository.GetPipelineByDuns(rule.PipelineDuns);
                searchList = new List<AbstractSearch>();
                if (rule.PropertyId == 0) {
                    var allnoticeList = INoticesRepository.GetNotice(rule.PipelineDuns, rule.IsCriticalNotice).ToList();
                    dataList = dataList.Union(allnoticeList);
                }
                else
                {
                    string name = IMasterColumnRepo.GetById(rule.PropertyId).Name;
                    var property = type.GetProperties()
                    .Where(p => p.CanRead && p.CanWrite && (p.Name == name)).FirstOrDefault();
                    var searchProperty = SearchExtensions.CreateSearchCriteria(type, property.PropertyType, property.Name, ILogicalOperatorRepo.GetById(rule.ComparatorsId).Name, rule.value);
                    searchList.Add(searchProperty);                    
                    var noticeList = INoticesRepository.GetNotice(rule.PipelineDuns, rule.IsCriticalNotice).ApplySearchCriterias(searchList).ToList();
                    dataList = dataList.Union(noticeList);
                }
               
            }
            return dataList.OrderByDescending(a=>a.PostingDateTime).Select(a => ImodelFactory.Parse(a)).ToList();
        }

        public List<SwntPerTransactionDTO> ExecuteWatchListSWNTOnScreen(List<WatchListRule> RuleList, Type type)
        {
            IEnumerable<SwntPerTransaction> dataList = new List<SwntPerTransaction>();
            List<AbstractSearch> searchList;
            foreach (var rule in RuleList)
            {
                // var pipeline = IPipelineRepository.GetPipelineByDuns(rule.PipelineDuns);
                searchList = new List<AbstractSearch>();
                if (rule.PropertyId == 0)
                {
                    var allnoticeList = INoticesRepository.GetAllNoticeForPipe(rule.PipelineDuns, rule.IsCriticalNotice).ToList();
                    dataList = dataList.Union(allnoticeList);
                }
                else
                {
                    string name = IMasterColumnRepo.GetById(rule.PropertyId).Name;
                    var property = type.GetProperties()
                    .Where(p => p.CanRead && p.CanWrite && (p.Name == name)).FirstOrDefault();
                    var searchProperty = SearchExtensions.CreateSearchCriteria(type, property.PropertyType, property.Name, ILogicalOperatorRepo.GetById(rule.ComparatorsId).Name, rule.value);
                    searchList.Add(searchProperty);
                    var noticeList = INoticesRepository.GetAllNoticeForPipe(rule.PipelineDuns, rule.IsCriticalNotice).ApplySearchCriterias(searchList).ToList();
                    dataList = dataList.Union(noticeList);
                }

            }
            return dataList.OrderByDescending(a => a.PostingDateTime).Select(a => ImodelFactory.Parse(a)).ToList();
        }


        public List<OACYPerTransactionDTO> ExecuteWatchListOACY(List<WatchListRule> RuleList,Type type)
        {
            IEnumerable<OACYPerTransaction> dataList = new List<OACYPerTransaction>();
            List<AbstractSearch> searchList;           
            foreach (var rule in RuleList)
            {
                searchList = new List<AbstractSearch>();
                string name = IMasterColumnRepo.GetById(rule.PropertyId).Name;
                var property = type.GetProperties()
                .Where(p => p.CanRead && p.CanWrite && (p.Name == name)).FirstOrDefault();
                var searchProperty = SearchExtensions.CreateSearchCriteria(type, property.PropertyType, property.Name, ILogicalOperatorRepo.GetById(rule.ComparatorsId).Name, rule.value);
                searchList.Add(searchProperty);
               var oacyList = IOacyRepository.GetByPipelineLoc(rule.PipelineDuns, rule.LocationIdentifier).ApplySearchCriterias(searchList).ToList();
                dataList = dataList.Union(oacyList);
            }
            return dataList.OrderByDescending(a => a.PostingDateTime).Select(a => ImodelFactory.Parse(a)).ToList();
        }

        public List<OACYPerTransactionDTO> ExecuteWatchListOACYonScreen(List<WatchListRule> RuleList, Type type)
        {
            IEnumerable<OACYPerTransaction> dataList = new List<OACYPerTransaction>();
            List<AbstractSearch> searchList;
            foreach (var rule in RuleList)
            {
                searchList = new List<AbstractSearch>();
                string name = IMasterColumnRepo.GetById(rule.PropertyId).Name;
                var property = type.GetProperties()
                .Where(p => p.CanRead && p.CanWrite && (p.Name == name)).FirstOrDefault();
                var searchProperty = SearchExtensions.CreateSearchCriteria(type, property.PropertyType, property.Name, ILogicalOperatorRepo.GetById(rule.ComparatorsId).Name, rule.value);
                searchList.Add(searchProperty);
                var oacyList = IOacyRepository.GetAllByPipelineLoc(rule.PipelineDuns, rule.LocationIdentifier).ApplySearchCriterias(searchList).ToList();
                dataList = dataList.Union(oacyList);
            }
            return dataList.OrderByDescending(a => a.PostingDateTime).Select(a => ImodelFactory.Parse(a)).ToList();
        }


        public List<UnscPerTransactionDTO> ExecuteWatchListUNSC(List<WatchListRule> RuleList, Type type)
        {
            IEnumerable<UnscPerTransaction> dataList = new List<UnscPerTransaction>();
            List<AbstractSearch> searchList;
            foreach (var rule in RuleList)
            {
                searchList = new List<AbstractSearch>();
                string name = IMasterColumnRepo.GetById(rule.PropertyId).Name;
                var property = type.GetProperties()
                .Where(p => p.CanRead && p.CanWrite && (p.Name == name)).FirstOrDefault();
                var searchProperty = SearchExtensions.CreateSearchCriteria(type, property.PropertyType, property.Name, ILogicalOperatorRepo.GetById(rule.ComparatorsId).Name, rule.value);
                searchList.Add(searchProperty);
               var unscList = IUNSCRepository.GetByPipelineLoc(rule.PipelineDuns, rule.LocationIdentifier).ApplySearchCriterias(searchList).ToList();
                dataList=dataList.Union(unscList);
            }
            return dataList.OrderByDescending(a=>a.PostingDateTime).Select(a => ImodelFactory.Parse(a)).ToList();
        }



        public List<UnscPerTransactionDTO> ExecuteWatchListUNSConScreen(List<WatchListRule> RuleList, Type type)
        {
            IEnumerable<UnscPerTransaction> dataList = new List<UnscPerTransaction>();
            List<AbstractSearch> searchList;
            foreach (var rule in RuleList)
            {
                searchList = new List<AbstractSearch>();
                string name = IMasterColumnRepo.GetById(rule.PropertyId).Name;
                var property = type.GetProperties()
                .Where(p => p.CanRead && p.CanWrite && (p.Name == name)).FirstOrDefault();
                var searchProperty = SearchExtensions.CreateSearchCriteria(type, property.PropertyType, property.Name, ILogicalOperatorRepo.GetById(rule.ComparatorsId).Name, rule.value);
                searchList.Add(searchProperty);
                var unscList = IUNSCRepository.GetAllByPipelineLoc(rule.PipelineDuns, rule.LocationIdentifier).ApplySearchCriterias(searchList).ToList();
                dataList = dataList.Union(unscList);
            }
            return dataList.OrderByDescending(a => a.PostingDateTime).Select(a => ImodelFactory.Parse(a)).ToList();
        }

        public List<UnscPerTransactionDTO> ExecuteWatchListWithUNSC(int pipelineId,WatchListDTO watchListDto, Type type)
        {
            List<AbstractSearch> searchList = new List<AbstractSearch>();
            foreach (var rule in watchListDto.RuleList)
            {
                string name = IMasterColumnRepo.GetById(rule.PropertyId).Name;
                var property = type.GetProperties()
                .Where(p => p.CanRead && p.CanWrite && (p.Name == name)).FirstOrDefault();
                var searchProperty = SearchExtensions.CreateSearchCriteria(type, property.PropertyType, property.Name, ILogicalOperatorRepo.GetById(rule.ComparatorsId).Name, rule.value);
                searchList.Add(searchProperty);
            }
            /////Only For UNSC
            var pipeline = IPipelineRepository.GetById(pipelineId);
            var unscList = IUNSCRepository.GetByPipeline(pipeline.DUNSNo).ApplySearchCriterias(searchList).ToList();
            return unscList.Select(a => ImodelFactory.Parse(a)).ToList();
        }

        public IQueryable<UnscPerTransaction> ExecuteWatchListQueryUNSC(int pipelineId, WatchListDTO watchListDto, Type type) {
            IQueryable<UnscPerTransaction> query;
            List<AbstractSearch> searchList = new List<AbstractSearch>();
            foreach (var rule in watchListDto.RuleList)
            {
                string name = IMasterColumnRepo.GetById(rule.PropertyId).Name;
                var property = type.GetProperties()
                .Where(p => p.CanRead && p.CanWrite && (p.Name == name)).FirstOrDefault();
                var searchProperty = SearchExtensions.CreateSearchCriteria(type, property.PropertyType, property.Name, ILogicalOperatorRepo.GetById(rule.ComparatorsId).Name, rule.value);
                searchList.Add(searchProperty);
            }
            /////Only For UNSC
            var pipeline = IPipelineRepository.GetById(pipelineId);
            query = IUNSCRepository.GetByPipeline(pipeline.DUNSNo).ApplySearchCriterias(searchList);
            return query;
        }

        public int GetTotalCountUNSC(IQueryable<UnscPerTransaction> query)
        {
            int totalRec = 0;
            totalRec = query.Count();
            return totalRec;
        }

        public IQueryable<UnscPerTransaction> ApplySortingUNSC(IQueryable<UnscPerTransaction> query, string order, string orderDir)
        {
            // Sorting
            switch (order)
            {
                case "0":

                    query = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? query.OrderByDescending(p => p.Loc)
                                                                                             : query.OrderBy(p => p.Loc);
                    break;

                case "1":

                    query = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? query.OrderByDescending(p => p.LocName)
                                                                                             : query.OrderBy(p => p.LocName);
                    break;

                case "2":

                    query = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? query.OrderByDescending(p => p.LocQTIDesc)
                                                                                             : query.OrderBy(p => p.LocQTIDesc);
                    break;

                case "3":

                    query = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? query.OrderByDescending(p => (p.PostingDateTime))
                                                                                             : query.OrderBy(p => (p.PostingDateTime));
                    break;

                case "4":

                    query = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? query.OrderByDescending(p => (p.EffectiveGasDayTime))
                                                                                             : query.OrderBy(p => (p.EffectiveGasDayTime));
                    break;

                case "5":

                    query = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? query.OrderByDescending(p =>(p.EndingEffectiveDay))
                                                                                             : query.OrderBy(p => (p.EndingEffectiveDay));
                    break;

                case "6":

                    query = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? query.OrderByDescending(p => p.MeasBasisDesc)
                                                                                             : query.OrderBy(p => p.MeasBasisDesc);
                    break;

                case "7":

                    query = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? query.OrderByDescending(p => p.LocZn)
                                                                                               : query.OrderBy(p => p.LocZn);
                    break;

                case "8":

                    query = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? query.OrderByDescending(p => p.UnsubscribeCapacity )
                                                                                             : query.OrderBy(p => p.UnsubscribeCapacity);

                    break;

                case "9":
                    query = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? query.OrderByDescending(p => p.ChangePercentage)
                                                                       : query.OrderBy(p => p.ChangePercentage);
                    break;

            }
            return query;
        }

        public IQueryable<UnscPerTransaction> ApplyPagingUNSC(IQueryable<UnscPerTransaction> query, int currentPageNo, int pageSize)
        {
            return query.Skip(currentPageNo*pageSize).Take(pageSize);
        }

        public List<WatchListOperator> GetOperatorByDataType(int dataTypeId)
        {
            var list = ILogicalOperatorRepo.GetByDataTypeId(dataTypeId);
            List<WatchListOperator> operatorList = new List<WatchListOperator>();
            foreach (var item in list)
            {
                WatchListOperator operatr = new WatchListOperator();
                operatr.Id = item.Id;
                operatr.Name = item.Name;
                operatr.Title = item.Title;
                operatr.DataType =IdataTypeGroupRepo.GetById(item.DataTypeId).Name;
                operatorList.Add(operatr);
            }
            return operatorList;
        }

        public WatchListProperty GetPropertyById(int propertyId)
        {
            var item = IMasterColumnRepo.GetById(propertyId);
            WatchListProperty property = new WatchListProperty();
            property.Id = item.Id;
            property.Name = item.Name;
            property.Title = item.Title;
            property.DataSet = item.DataSetId;
            property.Datatype = IdataTypeGroupRepo.GetById(item.DataTypeId).Name;
            property.DatatypeId = item.DataTypeId;
            property.Operators = GetOperatorByDataType(property.DatatypeId);
            return property;
        }

        public List<WatchListProperty> GetPropertiesByDataSet(EnercrossDataSets dataset)
        {
            var list = IMasterColumnRepo.GetByDataSet(dataset).ToList();
            List<WatchListProperty> propertylist = new List<WatchListProperty>();
            foreach (var item in list)
            {
                WatchListProperty property = new WatchListProperty();
                property.Id = item.Id;
                property.Name = item.Name;
                property.Title = item.Title;
                property.DataSet = dataset;
                property.Datatype = IdataTypeGroupRepo.GetById(item.DataTypeId).Name;
                property.DatatypeId = item.DataTypeId;
                property.Operators = GetOperatorByDataType(property.DatatypeId);
                propertylist.Add(property);
            }
            return propertylist;
        }


        public List<LocationsDTO> GetLocationsFromUPRDs(string PipelineDuns, EnercrossDataSets datasetType)
        {
            List<LocationsDTO> locs = new List<LocationsDTO>();              
            switch (datasetType)
            {
                case EnercrossDataSets.OACY:
                    DateTime? recentPostDate = IOacyRepository.GetRecentPostDateUsngDuns(PipelineDuns);
                    DateTime dateForData = recentPostDate == null ? DateTime.Now.AddDays(-2) : recentPostDate.GetValueOrDefault().Date;
                    locs = IOacyRepository.GetByPipeline(PipelineDuns).Where(a=>a.PostingDateTime >= dateForData).Select(a => new LocationsDTO() { Identifier = a.Loc, Name = string.IsNullOrEmpty(a.LocName) ? a.Loc : (a.LocName + " / " + a.Loc) }).Distinct().ToList();
                    break;
                case EnercrossDataSets.UNSC:
                    DateTime? recentPostDateUnsc = IUNSCRepository.GetRecentPostDateUsingDuns(PipelineDuns);
                    DateTime dataForDataUnsc = recentPostDateUnsc == null ? DateTime.Now.AddDays(-2) : recentPostDateUnsc.GetValueOrDefault().Date; 
                    locs = IUNSCRepository.GetByPipeline(PipelineDuns).Where(a => a.PostingDateTime >= dataForDataUnsc).Select(a => new LocationsDTO() { Identifier = a.Loc, Name = string.IsNullOrEmpty(a.LocName) ? a.Loc : (a.LocName + " / " + a.Loc) }).Distinct().ToList();
                    break;
            }
            return locs.OrderBy(a => a.Name).Take(300).ToList();
        }
              

    }
}
