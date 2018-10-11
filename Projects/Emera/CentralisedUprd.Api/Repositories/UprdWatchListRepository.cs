using CentralisedUprd.Api.Enum;
using CentralisedUprd.Api.Models;
using CentralisedUprd.Api.UPRD.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralisedUprd.Api.Repositories
{
    public  class UprdWatchListRepository
    {
        public UprdDbEntities1 DbContext ;

        public UprdWatchListRepository() {
            DbContext = new UprdDbEntities1();
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
                AlertFrequency = (int)watchListRule.AlertFrequency,
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
                newWatchList.DataSetId = (int) UprdDataSet.SWNT;
            else if (dataset == UprdDataSet.OACY)
                newWatchList.DataSetId = (int)UprdDataSet.OACY;
            else
                newWatchList.DataSetId = (int)UprdDataSet.UNSC;
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
                return DbContext.MasterColumns.Where(a => a.DataSetId == (int)(UprdDataSet.SWNT));
            else if (dataSet == UprdDataSet.OACY)
                return DbContext.MasterColumns.Where(a => a.DataSetId == (int)(UprdDataSet.OACY));
            else
                return DbContext.MasterColumns.Where(a => a.DataSetId == (int)(UprdDataSet.UNSC));
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
            return DbContext.Watchlists.Where(a => (int)dataset == a.DataSetId);
        }

        public  IQueryable<WatchListMailAlertUPRDMapping> GetDataForUPRDMapping(string UserId, int watchListId, UprdDataSet dataSet)
        {
            return DbContext.WatchListMailAlertUPRDMappings.Where(a => a.UserId == UserId
            && a.WatchListId == watchListId
            && a.DataSetId == (int)dataSet
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

               DbContext.EmailQueues.Add(model);
               DbContext.SaveChanges();
            }
        }

    }
}