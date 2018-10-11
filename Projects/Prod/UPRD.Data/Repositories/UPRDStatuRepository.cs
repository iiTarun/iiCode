using System;
using System.Collections.Generic;
using System.Linq;
using Nom1Done.DTO;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdStatuRepository : RepositoryBase<UPRDStatus>, IUprdStatuRepository
    {
        public UprdStatuRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public UPRDStatus GetUprdByPipelineOnDate(string pipeDuns, DateTime onDate,int dataset)
        {
            return this.DbContext.UPRDStatus.Where(a => a.PipeDuns == pipeDuns && a.CreatedDate.Value.Day == onDate.Day && a.CreatedDate.Value.Month == onDate.Month && a.CreatedDate.Value.Year == onDate.Year && a.DatasetRequested.Value==dataset && !a.IsDatasetReceived).FirstOrDefault();
        }

        public List<UPRDStatus> GetUprdByPipelineOnDate(string pipeDuns, DateTime onDate)
        {
            return this.DbContext.UPRDStatus.Where(a => a.PipeDuns == pipeDuns && a.CreatedDate.Value.Day == onDate.Day && a.CreatedDate.Value.Month == onDate.Month && a.CreatedDate.Value.Year == onDate.Year).ToList();
        }

        public List<UPRDStatusDTO> GetUprdOnDate(DateTime date)
        {
            List<UPRDStatusDTO> statusList = new List<UPRDStatusDTO>();
            try
            {
                var data = (from a in this.DbContext.UPRDStatus
                            where a.CreatedDate.HasValue
                            && a.CreatedDate.Value.Day == date.Day
                            && a.CreatedDate.Value.Month == date.Month
                            && a.CreatedDate.Value.Year == date.Year
                            group a by new
                            {
                                a.PipeDuns,
                                a.DatasetRequested
                            } into ga
                            select ga).ToList();
                foreach(var d in data)
                {
                    var pipeDuns = d.FirstOrDefault().PipeDuns;
                    var pipe = this.DbContext.Pipeline.Where(a => a.DUNSNo == pipeDuns).FirstOrDefault();
                    statusList.Add(new UPRDStatusDTO
                    {
                        Pipeline = pipe.Name + " (" + pipe.DUNSNo + ")",
                        CreatedDate = d.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().CreatedDate.Value,
                        DatasetRequested = d.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().DatasetRequested,
                        DatasetSummary = d.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().DatasetSummary,
                        IsDataSetAvailable = d.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().IsDataSetAvailable,
                        IsDatasetReceived = d.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().IsDatasetReceived,
                        IsRURDReceived = d.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().IsRURDReceived,
                        RequestID = d.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().RequestID,
                        RURD_ID = d.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().RURD_ID,
                        UPRD_ID = d.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().UPRD_ID,
                        TransactionId = d.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().TransactionId
                    });
                }
                return statusList;

                //join b in this.DbContext.Pipeline on ga.FirstOrDefault().PipeDuns equals b.DUNSNo
                //select ga).ToList();
                //select new /*UPRDStatusDTO()*/
                //{
                //    Pipeline = b.Name + " (" + b.DUNSNo + ")",
                //    CreatedDate = ga.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().CreatedDate.Value,
                //    DatasetRequested = ga.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().DatasetRequested,
                //    DatasetSummary = ga.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().DatasetSummary,
                //    IsDataSetAvailable = ga.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().IsDataSetAvailable,
                //    IsDatasetReceived = ga.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().IsDatasetReceived,
                //    IsRURDReceived = ga.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().IsRURDReceived,
                //    RequestID = ga.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().RequestID,
                //    RURD_ID = ga.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().RURD_ID,
                //    UPRD_ID = ga.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().UPRD_ID,
                //    TransactionId = ga.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault().TransactionId
                //}).ToList();
                //return data.OrderByDescending(a => a.CreatedDate).ToList();
                //return new List<UPRDStatusDTO>();
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public UPRDStatus GetUprdStatus(string referNumber)
        {
            return this.DbContext.UPRDStatus.Where(a => a.RequestID == referNumber).FirstOrDefault();
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }

        public bool SaveRurdList(List<UPRDStatus> uprdStatusList, int pipeId)
        {
            this.DbContext.UPRDStatus.AddRange(uprdStatusList);
            this.DbContext.SaveChanges();
            return true;
        }
    }

    public interface IUprdStatuRepository : IRepository<UPRDStatus>
    {
        void Save();
        bool SaveRurdList(List<UPRDStatus> uprdStatusList, int pipeId);
        UPRDStatus GetUprdStatus(string referNumber);
        UPRDStatus GetUprdByPipelineOnDate(string pipeDuns,DateTime onDate,int dataset);
        List<UPRDStatus> GetUprdByPipelineOnDate(string pipeDuns, DateTime onDate);
        List<UPRDStatusDTO> GetUprdOnDate(DateTime date);
    }
}
