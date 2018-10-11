using CentralisedUprd.Api.CustomQueryHelper;
using CentralisedUprd.Api.Enum;
using CentralisedUprd.Api.Helpers;
using CentralisedUprd.Api.Models;
using CentralisedUprd.Api.Repositories;
using CentralisedUprd.Api.UPRD.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CentralisedUprd.Api.Services
{
    public class WatchlistService
    {

        public ApplicationLogRepository applogs;
        public UprdWatchListRepository UprdWatchListRepository;
        ModalFactory modalFactory;
        public UprdDbEntities1 DbContext;

        public WatchlistService() {
            applogs = new ApplicationLogRepository();
            UprdWatchListRepository = new UprdWatchListRepository();
            modalFactory = new ModalFactory();
            DbContext = new UprdDbEntities1();
        }

        public  void SaveWatchListUprdForOACY(List<OACYPerTransactionDTO> oacyList,WatchListDTO watchlist )
        {
            foreach (var uprdItem in oacyList)
            {
                WatchListMailAlertUPRDMapping model = new WatchListMailAlertUPRDMapping();
                model.WatchListId = watchlist.id;
                model.DataSetId = (int)UprdDataSet.OACY;
                model.UserId = watchlist.UserId;
                model.EmailSentDateTime = DateTime.Now;
                model.UprdID = uprdItem.OACYID;
                UprdWatchListRepository.AddWatchListUprdMapping(model);

            }          
        }

        public  void SaveWatchListUprdForUNSC(List<UnscPerTransactionDTO> unscItem, WatchListDTO watchlist)
        {
            foreach (var uprdItem in unscItem)
            {
                WatchListMailAlertUPRDMapping model = new WatchListMailAlertUPRDMapping();
                model.WatchListId = watchlist.id;
                model.DataSetId = (int)UprdDataSet.UNSC;
                model.UserId = watchlist.UserId;
                model.EmailSentDateTime = DateTime.Now;
                model.UprdID = uprdItem.UnscID;
                UprdWatchListRepository.AddWatchListUprdMapping(model);
            }
        }

        public  void SaveWatchListUprdForSWNT(List<SwntPerTransactionDTO> swntList,WatchListDTO watchlist)
        {
            foreach (var uprdItem in swntList)
            {
                WatchListMailAlertUPRDMapping model = new WatchListMailAlertUPRDMapping();
                model.WatchListId = watchlist.id;
                model.DataSetId = (int)UprdDataSet.SWNT;
                model.UserId = watchlist.UserId;
                model.EmailSentDateTime = DateTime.Now;
                model.UprdID = uprdItem.Id;
                UprdWatchListRepository.AddWatchListUprdMapping(model);
            }
        }

        public  string BuildEmailHtmlTemplate(List<WatchListAlertExecutedDataDTO> alertExeDataCollection)
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
                    foreach (var oacy in watchListdata.OacyDataList.OrderBy(a => a.PipelineNameDuns).ThenBy(a => a.Loc).ThenBy(a=>a.PostingDate))
                    {
                        tableStr += "<tr><td style=\"text-align:center;\">" + oacy.CycleIndicator + "</td>";
                        tableStr += "<td style=\"text-align:center;\"><a href=\"" + viewmoreurl + "/MOperationalCapacity/index?pipelineId=" + oacy.PipelineID + "\" style =\"color:#FF6C3A\">" + oacy.PipelineNameDuns + "</a></td>";
                        tableStr += "<td style=\"text-align:center;\">" + oacy.Loc + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + oacy.LocName + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + string.Format("{0:n0}", oacy.OperatingCapacity)   + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + string.Format("{0:n0}", oacy.TotalScheduleQty) + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + oacy.AvailablePercentage + "</td>";
                        tableStr += "<td style=\"text-align:center;\">" + oacy.PostingDate + "</td></tr>";
                        // count++;
                        //if (count > totalRowLimit)
                        //{
                        //    break;
                        //}
                    }
                    tableStr += "</tbody></table>";
                    //if (count > totalRowLimit) {
                    //    tableStr += "<div style=\"width:100%;height:50px; background:#ffff;padding-top: 10px;\"><a href=\""+ viewmoreurl + "/WatchList/List?pipelineId=1\" style =\"color:#FF6C3A\"> View More</a></div>";
                    //   // emailbody = emailbody + tableStr;
                    //   // break;
                    //}

                }
                else if (watchListdata.UnscDataList != null && watchListdata.UnscDataList.Count > 0)
                {
                    tableStr += "<h3> WatchList Name: " + watchListdata.watchList.ListName + "</h3>";
                    tableStr += "<table width=\"100%\" bgcolor=\"#f6f8f1\" border=\"0\" cellpadding=\"5\" cellspacing=\"0\">";
                    tableStr += "<thead><tr style=\"background-color:#FF6C3A; color:#fff; height:30px\"><th style=\"text-align:center;\">Pipeline</th><th style=\"text-align:center;\">Loc Prop</th><th style=\"text-align:center;\">Loc Name</th><th style=\"text-align:center;\">Unsubscribed Cap Today</th><th style=\"text-align:center;\">% Change</th><th style=\"text-align:center;\">Post Date</th></tr></thead>";
                    tableStr += "<tbody>";
                    foreach (var unsc in watchListdata.UnscDataList.OrderBy(a => a.PipelineNameDuns).ThenBy(a => a.Loc).ThenBy(a=>a.PostingDate))
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




        public  int SaveWatchList(WatchListDTO watchList)
        {
            int watchListId = SaveWatchListTableData(watchList);
            foreach (var watchlistRule in watchList.RuleList)
            {
                UprdWatchListRepository.AddWatchListRule(watchListId, watchlistRule);
            }
            return watchListId;
        }

        private  int SaveWatchListTableData(WatchListDTO watchList)
        {
            return UprdWatchListRepository.AddWatchList(watchList.ListName, watchList.DatasetId, watchList.UserId, watchList.UserEmail,watchList.MoreDetailURLinAlert);
        }

        public  bool UpdateWatchList(WatchListDTO watchListDto)
        {
            var watchlistModel = UprdWatchListRepository.GetById(watchListDto.id);

            watchlistModel.Name = watchListDto.ListName;
            watchlistModel.DataSetId = (int)watchListDto.DatasetId;
            watchlistModel.ModifiedDate = DateTime.UtcNow;
            watchlistModel.Email = watchListDto.UserEmail;
            watchlistModel.MoreDetailURLinAlert = watchListDto.MoreDetailURLinAlert;
            UprdWatchListRepository.Update(watchlistModel); 

            var list = UprdWatchListRepository.GetByWatchListId(watchlistModel.Id).ToList();
            foreach (var item in list)
            {
                UprdWatchListRepository.Delete(item);               
            }
            foreach (var newItem in watchListDto.RuleList)
            {
                UprdWatchListRepository.AddWatchListRule(watchListDto.id, newItem);
            }

            return true;
        }


        public  WatchListDTO GetWatchListById(int watchListId)
        {
            var watchlist = UprdWatchListRepository.GetById(watchListId);
            WatchListDTO watchlistdto = new WatchListDTO();
            watchlistdto.id = watchlist.Id;
            watchlistdto.UserId = watchlist.UserId;
            switch (watchlist.DataSetId) {
                case 1:
                    watchlistdto.DatasetId = Enum.UprdDataSet.OACY;
                    break;
                case 2:
                    watchlistdto.DatasetId = Enum.UprdDataSet.UNSC;
                    break;
                case 3:
                    watchlistdto.DatasetId = Enum.UprdDataSet.SWNT;
                    break;
            }         
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
                dto.AlertFrequency = item.AlertFrequency==1? Enum.WatchlistAlertFrequency.Daily : Enum.WatchlistAlertFrequency.WhenAvailable;
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
                switch (watchlist.DataSetId)
                {
                    case 1:
                        watchlistdto.DatasetId = Enum.UprdDataSet.OACY;
                        break;
                    case 2:
                        watchlistdto.DatasetId = Enum.UprdDataSet.UNSC;
                        break;
                    case 3:
                        watchlistdto.DatasetId = Enum.UprdDataSet.SWNT;
                        break;
                }              
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
            switch (item.DataSetId)
            {
                case 1:
                    property.DataSet = Enum.UprdDataSet.OACY;
                    break;
                case 2:
                    property.DataSet = Enum.UprdDataSet.UNSC;
                    break;
                case 3:
                    property.DataSet = Enum.UprdDataSet.SWNT;
                    break;
            }          
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

        public  IQueryable<Location> SortByColumnWithOrder(IQueryable<Location> dataQuery, Helpers.SortingPagingInfo sortingPagingInfo)
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
                               join l in DbContext.Locations on o.TransactionServiceProvider equals l.PipeDuns into oac
                               from oacyResults in oac.Where(x => x.PropCode == o.Loc).DefaultIfEmpty()

                               select new OACYPerTransactionDTO
                               {
                                    OACYID = o.OACYID,
                                    TransactionID = o.TransactionID,
                                    ReceiceFileID = o.ReceiceFileID,
                                    CreatedDate = o.CreatedDate,
                                    TransactionServiceProviderPropCode = o.TransactionServiceProviderPropCode,
                                    TransactionServiceProvider = o.TransactionServiceProvider,
                                    PostingDate = o.PostingDate,
                                    EffectiveGasDay = o.EffectiveGasDay,
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
                                    PipelineNameDuns =o.PipelineNameDuns
                                
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

            var FinalResult = (from u in UNSCResult join l in DbContext.Locations on u.TransactionServiceProvider equals l.PipeDuns into uns
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


        public  bool ExecuteWatchList(WatchlistAlertFrequency alertType, UprdDataSet dataSet)
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
                switch (watchlist.DataSetId)
                {
                    case 1:
                        watchlistdto.DatasetId = Enum.UprdDataSet.OACY;
                        break;
                    case 2:
                        watchlistdto.DatasetId = Enum.UprdDataSet.UNSC;
                        break;
                    case 3:
                        watchlistdto.DatasetId = Enum.UprdDataSet.SWNT;
                        break;
                }               
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

        public  List<SwntPerTransactionDTO> ApplySwntGroupBy(List<SwntPerTransactionDTO> oldList)
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

        public  List<UnscPerTransactionDTO> ExecuteWatchListUNSC(List<WatchListRule> RuleList, Type type)
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
                dataList = dataList.Union(oacyList);
            }
            return dataList.OrderByDescending(a => a.PostingDateTime).Select(a => factory.Parse(a)).ToList();
        }


    }
}