using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UPRD.Model;
using UPRD.Data;
using UPRD.Model.Enums;
using WatchlistMailManagement.UPRD.DTO;

namespace WatchlistMailManagement.Repositories
{
    public  class UprdWatchListRepository
    {
        public UPRDEntities DbContext ;

        public UprdWatchListRepository() {
            DbContext = new UPRDEntities();
        }

        public  int AddWatchListRule(int watchListId, WatchListRule watchListRule)
        {
            WatchlistRule newRule = new WatchlistRule()
            {
                WatchlistId = watchListId,
                ColumnId = watchListRule.PropertyId,
                OperatorId = watchListRule.ComparatorsId,
                RuleValue = watchListRule.value,
                PipelineDuns = watchListRule.PipelineDuns,
                LocationIdentifier = watchListRule.LocationIdentifier,
                IsCriticalNotice = watchListRule.IsCriticalNotice,
                AlertSent = watchListRule.AlertSent,
                AlertFrequency = watchListRule.AlertFrequency,
                UpperRuleValue = watchListRule.UpperValue
            };
            DbContext.WatchlistRules.Add(newRule);
            DbContext.SaveChanges();
            return newRule.Id;
        }

        public  int AddWatchList(string Name, UprdDataSet dataset, string UserId,string userEmail, string MoreDetailUrl)
        {
            Watchlist newWatchList = new Watchlist();
            newWatchList.Name = Name;
            if (dataset == UprdDataSet.SWNT)
                newWatchList.DataSetId =  UprdDataSet.SWNT;
            else if (dataset == UprdDataSet.OACY)
                newWatchList.DataSetId = UprdDataSet.OACY;
            else
                newWatchList.DataSetId = UprdDataSet.UNSC;
            newWatchList.UserId = UserId;
            newWatchList.Email = userEmail;
            newWatchList.CreatedDate = DateTime.UtcNow;
            newWatchList.ModifiedDate = DateTime.UtcNow;
            newWatchList.MoreDetailURLinAlert = MoreDetailUrl;
            DbContext.Watchlists.Add(newWatchList);
            DbContext.SaveChanges();
            return newWatchList.Id;
        }

        public  Watchlist GetById(int id)
        {
            return DbContext.Watchlists.Where(a => a.Id == id).FirstOrDefault();
                
        }

        public  MasterColumn GetByIdMasterColumn(int Id)
        {
            return DbContext.MasterColumns.Where(a => a.Id == Id).FirstOrDefault();
        }


        public  LogicalOperator GetByIdLogicalOperator(int Id)
        {
            return DbContext.LogicalOperators.Where(a => a.Id == Id).FirstOrDefault();
        }

        public  void Update(Watchlist obj)
        {           
            //DbContext.Watchlists.Attach(obj);           
            DbContext.SaveChanges();
        }

        public  IQueryable<WatchlistRule> GetByWatchListId(int watchListId)
        {
            return DbContext.WatchlistRules.Where(a => a.WatchlistId == watchListId);
        }

        public  void Delete(WatchlistRule item) {
            DbContext.WatchlistRules.Remove(item);
            DbContext.SaveChanges();
        }

        public  IQueryable<Watchlist> GetByUserId(string userId)
        {
            return DbContext.Watchlists.Where(a => userId == a.UserId);
        }

        public  IQueryable<MasterColumn> GetByDataSet(UprdDataSet dataSet)
        {
            if (dataSet == UprdDataSet.SWNT)
                return DbContext.MasterColumns.Where(a => a.DataSetId == (UprdDataSet.SWNT));
            else if (dataSet == UprdDataSet.OACY)
                return DbContext.MasterColumns.Where(a => a.DataSetId == (UprdDataSet.OACY));
            else
                return DbContext.MasterColumns.Where(a => a.DataSetId == (UprdDataSet.UNSC));
        }

        public  DataTypeGrouping GetByIdDataTypeGrouping(int id)
        {
            return DbContext.DataTypeGroupings.Where(a => a.Id == id).FirstOrDefault();
        }

        public  IQueryable<LogicalOperator> GetByDataTypeId(int dataTypeId)
        {
            return DbContext.LogicalOperators.Where(a => a.DataTypeId == dataTypeId);
        }
        public  void Delete(Watchlist entity)
        {
            DbContext.Watchlists.Remove(entity);
            DbContext.SaveChanges();
        }

        public  IQueryable<Watchlist> GetAllBydataSet(UprdDataSet dataset)
        {
            return DbContext.Watchlists.Where(a => dataset == a.DataSetId);
        }

        public  IQueryable<WatchListMailAlertUPRDMapping> GetDataForUPRDMapping(string UserId, int watchListId, UprdDataSet dataSet)
        {
            return DbContext.WatchListMailAlertUPRDMappings.Where(a => a.UserId == UserId
            && a.WatchListId == watchListId
            && a.DataSetId == dataSet
            );
        }

        public IQueryable<WatchlistMailMappingSWNT> GetDataForSWNTmappingOnly(string UserId, int watchListId)
        {
            return DbContext.WatchlistMailMappingSWNTs.Where(a => a.UserId == UserId
            && a.WatchListId == watchListId         
            );
        }


        public IQueryable<WatchlistMailMappingUNSC> GetDataForUNSCMappingOnly(string UserId, int watchListId)
        {
            return DbContext.WatchlistMailMappingUNSCs.Where(a => a.UserId == UserId
            && a.WatchListId == watchListId           
            );
        }

        public IQueryable<WatchlistMailMappingOACY> GetDataFromMailMappingOACY(string UserId, int watchListId)
        {
            return DbContext.WatchlistMailMappingOACYs.Where(a => a.UserId == UserId
            && a.WatchListId == watchListId           
            );
        }



        public  void AddWatchListUprdMapping(WatchListMailAlertUPRDMapping Item) {

            DbContext.WatchListMailAlertUPRDMappings.Add(Item);
            DbContext.SaveChanges();
        }

        

        public  void AddEmailQueue(EmailQueueDto email)
        {
            if (email != null)
            {
                EmailQueue model = new EmailQueue()
                {
                    ToUserID = email.ToUserID,
                    Subject = email.Subject,
                    Email = email.Email,
                    Recipient = email.Recipient,
                    CC = email.CC,
                    Bcc = email.Bcc,
                    IsSent = email.IsSent,
                    CreatedDate = email.CreatedDate,
                    SentDate = email.SentDate
                };

               DbContext.EmailQueue.Add(model);
               DbContext.SaveChanges();
            }
        }

        //***************************** Seperate OACY table thread Approach ******************
        public IQueryable<WatchlistMailMappingOACY> GetDataForOACYMapping(string UserId, int watchListId)
        {
            return DbContext.WatchlistMailMappingOACYs.Where(a => a.UserId == UserId
            && a.WatchListId == watchListId
            && a.IsMailSent == true
            );
        }

        public IQueryable<WatchlistMailMappingOACY> GetAllUnSendDataForOACYMapping()
        {
            return DbContext.WatchlistMailMappingOACYs.Where(a=> a.IsMailSent == false);
        }


        public IQueryable<WatchlistMailMappingUNSC> GetAllUnSendDataForUNSCMapping()
        {
            return DbContext.WatchlistMailMappingUNSCs.Where(a => a.IsMailSent == false);
        }

        public IQueryable<WatchlistMailMappingSWNT> GetAllUnSendDataForSWNTMapping()
        {
            return DbContext.WatchlistMailMappingSWNTs.Where(a => a.IsMailSent == false);
        }

        public List<WatchlistMailMappingOACY> GetAllUnSendDataForOACYMappingByUser(string UserId)
        {
            return DbContext.WatchlistMailMappingOACYs.Where(a => a.IsMailSent == false && a.UserId==UserId).ToList();
        }


        public List<WatchlistMailMappingUNSC> GetAllUnSendDataForUnscMappingByUser(string UserId)
        {
            return DbContext.WatchlistMailMappingUNSCs.Where(a => a.IsMailSent == false && a.UserId == UserId).ToList();
        }

        public List<WatchlistMailMappingSWNT> GetAllUnSendDataForSWNTMappingByUser(string UserId)
        {
            return DbContext.WatchlistMailMappingSWNTs.Where(a => a.IsMailSent == false && a.UserId == UserId).ToList();
        }

        public bool UpdateForOACYMapping(List<WatchlistMailMappingOACY> dataList)
        {
            foreach (var item in dataList)
            {
                WatchlistMailMappingOACY oldItem = DbContext.WatchlistMailMappingOACYs.Where(a => a.OACYID == item.OACYID && a.WatchListId==item.WatchListId && a.UserId==item.UserId).FirstOrDefault();
                oldItem.IsMailSent = true;
                oldItem.EmailSentDateTime = DateTime.Now;
                DbContext.SaveChanges();
            }
            return true; // DbContext.WatchlistMailMappingOACYs.Where(a => a.IsMailSent == false);
        }


        public bool UpdateForUNSCMapping(List<WatchlistMailMappingUNSC> dataList)
        {
            foreach (var item in dataList)
            {
                WatchlistMailMappingUNSC oldItem = DbContext.WatchlistMailMappingUNSCs.Where(a => a.UNSCID == item.UNSCID && a.WatchListId == item.WatchListId && a.UserId == item.UserId).FirstOrDefault();
                oldItem.IsMailSent = true;
                oldItem.EmailSentDateTime = DateTime.Now;
                DbContext.SaveChanges();
            }
            return true; // DbContext.WatchlistMailMappingOACYs.Where(a => a.IsMailSent == false);
        }


        public bool UpdateForSWNTMapping(List<WatchlistMailMappingSWNT> dataList)
        {
            foreach (var item in dataList)
            {
                WatchlistMailMappingSWNT oldItem = DbContext.WatchlistMailMappingSWNTs.Where(a => a.SWNTID == item.SWNTID && a.WatchListId == item.WatchListId && a.UserId == item.UserId).FirstOrDefault();
                oldItem.IsMailSent = true;
                oldItem.EmailSentDateTime = DateTime.Now;
                DbContext.SaveChanges();
            }
            return true; 
        }

        public void GetAllUnSendDataForOACYMapping(List<WatchlistMailMappingOACY> OACYDataList)
        {
            //TODO Bulk insert 
             DbContext.WatchlistMailMappingOACYs.AddRange(OACYDataList).ToList();
        }


    }
}