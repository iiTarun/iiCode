using CentralisedUprd.Api.Models;
using CentralisedUprd.Api.UPRD.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CentralisedUprd.Api.Repositories
{
    public class UprdStatusRepository
    {
        UprdDbEntities1 DbContext = new UprdDbEntities1();
        ModalFactory modalFactory = new ModalFactory();
        public List<UPRDStatusDTO> GetUprdOnDateAndPipelineDuns(DateTime date)
        {
            List<UPRDStatusDTO> statusList = new List<UPRDStatusDTO>();
            try
            {
                var data = (from a in this.DbContext.UPRDStatus
                            join pipe in this.DbContext.Pipelines on a.PipeDuns equals pipe.DUNSNo
                            where a.CreatedDate.HasValue
                            && a.CreatedDate.Value.Day == date.Day
                            && a.CreatedDate.Value.Month == date.Month
                            && a.CreatedDate.Value.Year == date.Year
                            group a by new
                            {
                                a.PipeDuns,
                                a.DatasetRequested,
                                pipe.Name
                            } into ga
                            select
                            new
                            {
                                uprd = ga.OrderByDescending(a => a.CreatedDate.Value).FirstOrDefault(),
                                Name = ga.Key.Name
                            }).ToList();

                var oacyExistForPipes = this.DbContext.OACYPerTransactions.Where(a =>
                    a.PostingDateTime.Value.Day == date.Day
                    && a.PostingDateTime.Value.Month == date.Month
                    && a.PostingDateTime.Value.Year == date.Year).Select(a => a.TransactionServiceProvider).Distinct().ToArray();

                var unscExistForPipes = this.DbContext.UnscPerTransactions.Where(a =>
                      a.PostingDateTime.Value.Day == date.Day
                      && a.PostingDateTime.Value.Month == date.Month
                      && a.PostingDateTime.Value.Year == date.Year).Select(a => a.TransactionServiceProvider).Distinct().ToArray();

                var swntExistForPipes = this.DbContext.SwntPerTransactions.Where(a =>
                      a.PostingDateTime.Value.Day == date.Day
                      && a.PostingDateTime.Value.Month == date.Month
                      && a.PostingDateTime.Value.Year == date.Year).Select(a => a.TransportationserviceProvider).Distinct().ToArray();



                foreach (var d in data)
                {
                    bool IsDatasetReceived = false;
                    var pipeDuns = d.uprd.PipeDuns;
                    var dataset = d.uprd.DatasetRequested;
                    switch (dataset)
                    {
                        case 1://oacy
                            IsDatasetReceived = oacyExistForPipes.Contains(pipeDuns);
                            break;
                        case 3://Unsc
                            IsDatasetReceived = unscExistForPipes.Contains(pipeDuns);
                            break;
                        case 10://swnt
                            IsDatasetReceived = swntExistForPipes.Contains(pipeDuns);
                            break;
                    }
                    statusList.Add(new UPRDStatusDTO
                    {
                        Pipeline = d.Name + " (" + d.uprd.PipeDuns + ")",
                        CreatedDate = d.uprd.CreatedDate.Value,
                        DatasetRequested = dataset,
                        DatasetSummary = d.uprd.DatasetSummary,
                        IsDataSetAvailable = d.uprd.IsDataSetAvailable,
                        IsDatasetReceived = IsDatasetReceived,
                        IsRURDReceived = d.uprd.IsRURDReceived,
                        RequestID = d.uprd.RequestID,
                        RURD_ID = d.uprd.RURD_ID,
                        UPRD_ID = d.uprd.UPRD_ID,
                        TransactionId = d.uprd.TransactionId,
                        PipeDuns = pipeDuns
                    });
                }
                return statusList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}