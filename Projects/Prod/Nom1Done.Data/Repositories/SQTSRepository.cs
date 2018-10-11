using Nom1Done.DTO;
using Nom1Done.Infrastructure;
using Nom1Done.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace Nom1Done.Data.Repositories
{
    public class SQTSRepository : RepositoryBase<SQTSPerTransaction>, ISQTSRepository
    {
        public SQTSRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
        public List<SummaryDTO> GetSqtsOrphanData(int month, string pipeDuns, string pathType, string shipperCompanyDuns,bool showZero, int pageSize=15, int pageNo = 1)
        {
            List<SummaryDTO> summaryDTOList = new List<SummaryDTO>();
            int currentYear = DateTime.Now.Year;
            int skipSize = (pageNo - 1) * pageSize;
            if (!showZero)
            {
                if (pathType == "P")
                {
                    var sqtsList = DbContext.SQTSPerTransaction.Where(sqts =>
                                !string.IsNullOrEmpty(sqts.DeliveryLocation) &&
                                !string.IsNullOrEmpty(sqts.ReceiptLocation) &&
                                !string.IsNullOrEmpty(sqts.ServiceRequestorContract) &&
                                sqts.ServiceRequestor == shipperCompanyDuns &&
                                sqts.ModelType == "P" &&
                                sqts.NomTrackingId.Contains("N/A") &&
                                sqts.TSPCode == pipeDuns &&
                                sqts.BeginingDateTime.Month == month &&
                                sqts.BeginingDateTime.Year == currentYear
                    ).ToList();
                    if (sqtsList != null)
                    {
                        var Partition = sqtsList.OrderByDescending(a => a.StatementDate).GroupBy(x => new
                        {
                            x.BeginingDateTime,
                            x.DeliveryLocation,
                            x.ReceiptLocation,
                            x.ServiceRequestorContract,
                            x.DownstreamID,
                            x.UpstreamId,
                            x.PackageId
                        })
                       .SelectMany(g =>
                           g.Select((j, i) => new { j, rn = i + 1 })
                       ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                       .Select(a => new SummaryDTO
                       {
                           DelPointQty = a.j.DeliveryQuantity,
                           RecPointQty = a.j.ReceiptQuantity,
                           nomStartDate = a.j.BeginingDateTime,
                           nomEndDate=a.j.EndingDateTime,
                           DelLoc = a.j.DeliveryLocation,
                           RecLoc = a.j.ReceiptLocation,
                           ContractSVC = a.j.ServiceRequestorContract,
                           DownStreamName = a.j.DownstreamID,
                           UpStreamName = a.j.UpstreamId,
                           DwnIdentifier = a.j.DownstreamID,
                           UpIdentifier = a.j.UpstreamId,
                           StatementDate = a.j.StatementDate,
                           ReductionReason = a.j.ReductionReason,
                           Cycle = a.j.CycleIndicator,
                           PkgId = a.j.PackageId,
                           NomTrackingId = a.j.NomTrackingId,
                           ReductionReasonDetail = a.j.ReductionReasonDescription
                       }).ToList();
                        summaryDTOList = Partition;
                    }
                }
                else if (pathType == "T")
                {
                    var sqtsList = DbContext.SQTSPerTransaction.Where(sqts =>
                                !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                && sqts.ServiceRequestor == shipperCompanyDuns
                                && sqts.ModelType == "T"
                                && sqts.NomTrackingId.Contains("N/A")
                                && sqts.TSPCode == pipeDuns
                                && sqts.BeginingDateTime.Month == month
                                && sqts.BeginingDateTime.Year == currentYear
                    ).ToList();
                    if (sqtsList.Count > 0)
                    {
                        var Partition = sqtsList.OrderByDescending(a => a.StatementDate).GroupBy(x => new
                        {
                            x.BeginingDateTime,
                            x.DeliveryLocation,
                            x.ReceiptLocation,
                            x.ServiceRequestorContract,
                            x.DownstreamID,
                            x.UpstreamId,
                            x.PackageId
                        })
                       .SelectMany(g =>
                           g.Select((j, i) => new { j, rn = i + 1 })
                       ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                       .Select(a => new SummaryDTO
                       {
                           DelPointQty = a.j.DeliveryQuantity,
                           RecPointQty = a.j.ReceiptQuantity,
                           nomStartDate = a.j.BeginingDateTime,
                           nomEndDate = a.j.EndingDateTime,
                           DelLoc = a.j.DeliveryLocation,
                           RecLoc = a.j.ReceiptLocation,
                           ContractSVC = a.j.ServiceRequestorContract,
                           DownStreamName = a.j.DownstreamID,
                           UpStreamName = a.j.UpstreamId,
                           DwnIdentifier = a.j.DownstreamID,
                           UpIdentifier = a.j.UpstreamId,
                           StatementDate = a.j.StatementDate,
                           ReductionReason = a.j.ReductionReason,
                           Cycle = a.j.CycleIndicator,
                           PkgId = a.j.PackageId,
                           NomTrackingId = a.j.NomTrackingId,
                           ReductionReasonDetail = a.j.ReductionReasonDescription
                       }).ToList();
                        summaryDTOList = Partition;
                    }
                }
                else if (pathType == "R")
                {
                    var sqtsList = DbContext.SQTSPerTransaction.Where(sqts =>
                                string.IsNullOrEmpty(sqts.DeliveryLocation)
                                && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                && sqts.ServiceRequestor == shipperCompanyDuns
                                && sqts.ModelType == "N"
                                && sqts.NomTrackingId.Contains("N/A")
                                && sqts.TSPCode == pipeDuns
                                && sqts.BeginingDateTime.Month == month
                                && sqts.BeginingDateTime.Year == currentYear
                    ).ToList();
                    if (sqtsList.Count > 0)
                    {
                        var Partition = sqtsList.OrderByDescending(a => a.StatementDate).GroupBy(x => new
                        {
                            x.BeginingDateTime,
                            x.DeliveryLocation,
                            x.ReceiptLocation,
                            x.ServiceRequestorContract,
                            x.DownstreamID,
                            x.UpstreamId,
                            x.PackageId
                        })
                       .SelectMany(g =>
                           g.Select((j, i) => new { j, rn = i + 1 })
                       ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                       .Select(a => new SummaryDTO
                       {
                           DelPointQty = a.j.DeliveryQuantity,
                           RecPointQty = a.j.ReceiptQuantity,
                           nomStartDate = a.j.BeginingDateTime,
                           nomEndDate = a.j.EndingDateTime,
                           DelLoc = a.j.DeliveryLocation,
                           RecLoc = a.j.ReceiptLocation,
                           ContractSVC = a.j.ServiceRequestorContract,
                           DownStreamName = a.j.DownstreamID,
                           UpStreamName = a.j.UpstreamId,
                           DwnIdentifier = a.j.DownstreamID,
                           UpIdentifier = a.j.UpstreamId,
                           StatementDate = a.j.StatementDate,
                           ReductionReason = a.j.ReductionReason,
                           Cycle = a.j.CycleIndicator,
                           PkgId = a.j.PackageId,
                           NomTrackingId = a.j.NomTrackingId,
                           ReductionReasonDetail = a.j.ReductionReasonDescription
                       }).ToList();
                        summaryDTOList = Partition;
                    }

                }
                else if (pathType == "D")
                {
                    var sqtsList = DbContext.SQTSPerTransaction.Where(sqts =>
                                !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                && string.IsNullOrEmpty(sqts.ReceiptLocation)
                                && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                && sqts.ServiceRequestor == shipperCompanyDuns
                                && sqts.ModelType == "N"
                                && sqts.NomTrackingId.Contains("N/A")
                                && sqts.TSPCode == pipeDuns
                                && sqts.BeginingDateTime.Month == month
                                && sqts.BeginingDateTime.Year == currentYear
                    ).ToList();
                    if (sqtsList.Count > 0)
                    {
                        var Partition = sqtsList.OrderByDescending(a => a.StatementDate).GroupBy(x => new
                        {
                            x.BeginingDateTime,
                            x.DeliveryLocation,
                            x.ReceiptLocation,
                            x.ServiceRequestorContract,
                            x.DownstreamID,
                            x.UpstreamId,
                            x.PackageId
                        })
                       .SelectMany(g =>
                           g.Select((j, i) => new { j, rn = i + 1 })
                       ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                       .Select(a => new SummaryDTO
                       {
                           DelPointQty = a.j.DeliveryQuantity,
                           RecPointQty = a.j.ReceiptQuantity,
                           nomStartDate = a.j.BeginingDateTime,
                           nomEndDate = a.j.EndingDateTime,
                           DelLoc = a.j.DeliveryLocation,
                           RecLoc = a.j.ReceiptLocation,
                           ContractSVC = a.j.ServiceRequestorContract,
                           DownStreamName = a.j.DownstreamID,
                           UpStreamName = a.j.UpstreamId,
                           DwnIdentifier = a.j.DownstreamID,
                           UpIdentifier = a.j.UpstreamId,
                           StatementDate = a.j.StatementDate,
                           ReductionReason = a.j.ReductionReason,
                           Cycle = a.j.CycleIndicator,
                           PkgId = a.j.PackageId,
                           NomTrackingId = a.j.NomTrackingId,
                           ReductionReasonDetail = a.j.ReductionReasonDescription
                       }).ToList();
                        summaryDTOList = Partition;
                    }
                }

            }
            else
            {
                if (pathType == "P")
                {
                    var sqtsList = DbContext.SQTSPerTransaction.Where(sqts =>
                                !string.IsNullOrEmpty(sqts.DeliveryLocation) &&
                                !string.IsNullOrEmpty(sqts.ReceiptLocation) &&
                                !string.IsNullOrEmpty(sqts.ServiceRequestorContract) &&
                                sqts.ServiceRequestor == shipperCompanyDuns &&
                                sqts.ModelType == "P" &&
                                sqts.NomTrackingId.Contains("N/A") &&
                                sqts.TSPCode == pipeDuns &&
                                (sqts.ReceiptQuantity!=0 || sqts.DeliveryQuantity!=0) &&
                                sqts.BeginingDateTime.Month == month &&
                                sqts.BeginingDateTime.Year == currentYear
                    ).ToList();
                    if (sqtsList != null)
                    {
                        var Partition = sqtsList.OrderByDescending(a => a.StatementDate).GroupBy(x => new
                        {
                            x.BeginingDateTime,
                            x.DeliveryLocation,
                            x.ReceiptLocation,
                            x.ServiceRequestorContract,
                            x.DownstreamID,
                            x.UpstreamId,
                            x.PackageId
                        })
                       .SelectMany(g =>
                           g.Select((j, i) => new { j, rn = i + 1 })
                       ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                       .Select(a => new SummaryDTO
                       {
                           DelPointQty = a.j.DeliveryQuantity,
                           RecPointQty = a.j.ReceiptQuantity,
                           nomStartDate = a.j.BeginingDateTime,
                           nomEndDate = a.j.EndingDateTime,
                           DelLoc = a.j.DeliveryLocation,
                           RecLoc = a.j.ReceiptLocation,
                           ContractSVC = a.j.ServiceRequestorContract,
                           DownStreamName = a.j.DownstreamID,
                           UpStreamName = a.j.UpstreamId,
                           DwnIdentifier = a.j.DownstreamID,
                           UpIdentifier = a.j.UpstreamId,
                           StatementDate = a.j.StatementDate,
                           ReductionReason = a.j.ReductionReason,
                           Cycle = a.j.CycleIndicator,
                           PkgId = a.j.PackageId,
                           NomTrackingId = a.j.NomTrackingId,
                           ReductionReasonDetail = a.j.ReductionReasonDescription
                       }).ToList();
                        summaryDTOList = Partition;
                    }
                }
                else if (pathType == "T")
                {
                    var sqtsList = DbContext.SQTSPerTransaction.Where(sqts =>
                                !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                && sqts.ServiceRequestor == shipperCompanyDuns
                                && sqts.ModelType == "T"
                                && sqts.NomTrackingId.Contains("N/A")
                                && sqts.TSPCode == pipeDuns
                                && (sqts.ReceiptQuantity != 0 || sqts.DeliveryQuantity != 0)
                                && sqts.BeginingDateTime.Month == month
                                && sqts.BeginingDateTime.Year == currentYear
                    ).ToList();
                    if (sqtsList.Count > 0)
                    {
                        var Partition = sqtsList.OrderByDescending(a => a.StatementDate).GroupBy(x => new
                        {
                            x.BeginingDateTime,
                            x.DeliveryLocation,
                            x.ReceiptLocation,
                            x.ServiceRequestorContract,
                            x.DownstreamID,
                            x.UpstreamId,
                            x.PackageId
                        })
                       .SelectMany(g =>
                           g.Select((j, i) => new { j, rn = i + 1 })
                       ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                       .Select(a => new SummaryDTO
                       {
                           DelPointQty = a.j.DeliveryQuantity,
                           RecPointQty = a.j.ReceiptQuantity,
                           nomStartDate = a.j.BeginingDateTime,
                           nomEndDate = a.j.EndingDateTime,
                           DelLoc = a.j.DeliveryLocation,
                           RecLoc = a.j.ReceiptLocation,
                           ContractSVC = a.j.ServiceRequestorContract,
                           DownStreamName = a.j.DownstreamID,
                           UpStreamName = a.j.UpstreamId,
                           DwnIdentifier = a.j.DownstreamID,
                           UpIdentifier = a.j.UpstreamId,
                           StatementDate = a.j.StatementDate,
                           ReductionReason = a.j.ReductionReason,
                           Cycle = a.j.CycleIndicator,
                           PkgId = a.j.PackageId,
                           NomTrackingId = a.j.NomTrackingId,
                           ReductionReasonDetail = a.j.ReductionReasonDescription
                       }).ToList();
                        summaryDTOList = Partition;
                    }
                }
                else if (pathType == "R")
                {
                    var sqtsList = DbContext.SQTSPerTransaction.Where(sqts =>
                                string.IsNullOrEmpty(sqts.DeliveryLocation)
                                && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                && sqts.ServiceRequestor == shipperCompanyDuns
                                && sqts.ModelType == "N"
                                && sqts.NomTrackingId.Contains("N/A")
                                && sqts.TSPCode == pipeDuns
                                && (sqts.ReceiptQuantity != 0 || sqts.DeliveryQuantity != 0)
                                && sqts.BeginingDateTime.Month == month
                                && sqts.BeginingDateTime.Year == currentYear
                    ).ToList();
                    if (sqtsList.Count > 0)
                    {
                        var Partition = sqtsList.OrderByDescending(a => a.StatementDate).GroupBy(x => new
                        {
                            x.BeginingDateTime,
                            x.DeliveryLocation,
                            x.ReceiptLocation,
                            x.ServiceRequestorContract,
                            x.DownstreamID,
                            x.UpstreamId,
                            x.PackageId
                        })
                       .SelectMany(g =>
                           g.Select((j, i) => new { j, rn = i + 1 })
                       ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                       .Select(a => new SummaryDTO
                       {
                           DelPointQty = a.j.DeliveryQuantity,
                           RecPointQty = a.j.ReceiptQuantity,
                           nomStartDate = a.j.BeginingDateTime,
                           nomEndDate = a.j.EndingDateTime,
                           DelLoc = a.j.DeliveryLocation,
                           RecLoc = a.j.ReceiptLocation,
                           ContractSVC = a.j.ServiceRequestorContract,
                           DownStreamName = a.j.DownstreamID,
                           UpStreamName = a.j.UpstreamId,
                           DwnIdentifier = a.j.DownstreamID,
                           UpIdentifier = a.j.UpstreamId,
                           StatementDate = a.j.StatementDate,
                           ReductionReason = a.j.ReductionReason,
                           Cycle = a.j.CycleIndicator,
                           PkgId = a.j.PackageId,
                           NomTrackingId = a.j.NomTrackingId,
                           ReductionReasonDetail = a.j.ReductionReasonDescription
                       }).ToList();
                        summaryDTOList = Partition;
                    }

                }
                else if (pathType == "D")
                {
                    var sqtsList = DbContext.SQTSPerTransaction.Where(sqts =>
                                !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                && string.IsNullOrEmpty(sqts.ReceiptLocation)
                                && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                && sqts.ServiceRequestor == shipperCompanyDuns
                                && sqts.ModelType == "N"
                                && sqts.NomTrackingId.Contains("N/A")
                                && sqts.TSPCode == pipeDuns
                                && (sqts.ReceiptQuantity != 0 || sqts.DeliveryQuantity != 0)
                                && sqts.BeginingDateTime.Month == month
                                && sqts.BeginingDateTime.Year == currentYear
                    ).ToList();
                    if (sqtsList.Count > 0)
                    {
                        var Partition = sqtsList.OrderByDescending(a => a.StatementDate).GroupBy(x => new
                        {
                            x.BeginingDateTime,
                            x.DeliveryLocation,
                            x.ReceiptLocation,
                            x.ServiceRequestorContract,
                            x.DownstreamID,
                            x.UpstreamId,
                            x.PackageId
                        })
                       .SelectMany(g =>
                           g.Select((j, i) => new { j, rn = i + 1 })
                       ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                       .Select(a => new SummaryDTO
                       {
                           DelPointQty = a.j.DeliveryQuantity,
                           RecPointQty = a.j.ReceiptQuantity,
                           nomStartDate = a.j.BeginingDateTime,
                           nomEndDate = a.j.EndingDateTime,
                           DelLoc = a.j.DeliveryLocation,
                           RecLoc = a.j.ReceiptLocation,
                           ContractSVC = a.j.ServiceRequestorContract,
                           DownStreamName = a.j.DownstreamID,
                           UpStreamName = a.j.UpstreamId,
                           DwnIdentifier = a.j.DownstreamID,
                           UpIdentifier = a.j.UpstreamId,
                           StatementDate = a.j.StatementDate,
                           ReductionReason = a.j.ReductionReason,
                           Cycle = a.j.CycleIndicator,
                           PkgId = a.j.PackageId,
                           NomTrackingId = a.j.NomTrackingId,
                           ReductionReasonDetail=a.j.ReductionReasonDescription
                       }).ToList();
                        summaryDTOList = Partition;
                    }
                }

            }

            return summaryDTOList;
        }
        
        public List<SqtsDTO> GetSQTSForNom(string NomTrackingID)
        {
            return DbContext.SQTSPerTransaction.Where(a => a.NomTrackingId == NomTrackingID)
                .OrderByDescending(a => a.StatementDate)
                   .Select(a => new SqtsDTO
                   {
                       StatementDatetime = a.StatementDate,
                       BeginingDate = a.BeginingDateTime,
                       Cycle = a.CycleIndicator,
                       DelLoc = a.DeliveryLocation,
                       DelQty = a.DeliveryQuantity,
                       EndDate = a.EndingDateTime,
                       RecLoc = a.ReceiptLocation,
                       RecQty = a.ReceiptQuantity,
                       ReductionReason = a.ReductionReason
                   }).ToList();
        }
        
        public List<SummaryDTO> GetSqtsData(int month, string pipeDuns, string UserId, string pathType, string shipperCompanyDuns,bool showZero, int pageSize = 15, int pageNo = 1)
        {

            List<SummaryDTO> summaryDTOList = new List<SummaryDTO>();
            int currentYear = DateTime.Now.Year;
            int skipSize = (pageNo - 1) * pageSize;
            if (!showZero)
            {
                if (string.IsNullOrEmpty(UserId))
                {
                    if (pathType == "P" || pathType == "T")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && sqts.ModelType == pathType.Trim()
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).ToList();
                        if (sqtsList.Count > 0)
                        {
                            var Partition = sqtsList.OrderByDescending(a => a.sqts.StatementDate).GroupBy(x => new
                            {
                                x.sqts.BeginingDateTime,
                                x.sqts.DeliveryLocation,
                                x.sqts.ReceiptLocation,
                                x.sqts.ServiceRequestorContract,
                                x.sqts.DownstreamID,
                                x.sqts.UpstreamId,
                                x.sqts.PackageId,
                                x.sqts.NomTrackingId
                            })
                           .SelectMany(g =>
                               g.Select((j, i) => new { j, rn = i + 1 })
                           ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                           .Select(a => new SummaryDTO
                           {
                               DelPointQty = a.j.sqts.DeliveryQuantity,
                               RecPointQty = a.j.sqts.ReceiptQuantity,
                               nomStartDate = a.j.sqts.BeginingDateTime,
                               nomEndDate = a.j.sqts.EndingDateTime,
                               DelLoc = a.j.sqts.DeliveryLocation,
                               RecLoc = a.j.sqts.ReceiptLocation,
                               ContractSVC = a.j.sqts.ServiceRequestorContract,
                               DownStreamName = a.j.sqts.DownstreamID,
                               UpStreamName = a.j.sqts.UpstreamId,
                               DwnIdentifier = a.j.sqts.DownstreamID,
                               UpIdentifier = a.j.sqts.UpstreamId,
                               StatementDate = a.j.sqts.StatementDate,
                               ReductionReason = a.j.sqts.ReductionReason,
                               Cycle = a.j.sqts.CycleIndicator,
                               PkgId = a.j.sqts.PackageId,
                               NomTrackingId = a.j.sqts.NomTrackingId,
                               Username = a.j.shipper.FirstName + " " + a.j.shipper.LastName,
                               ReductionReasonDetail = a.j.sqts.ReductionReasonDescription
                           }).ToList();
                            summaryDTOList = Partition;
                        }
                    }
                    else if (pathType == "R")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && sqts.ModelType == "N"
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).ToList();
                        if (sqtsList.Count > 0)
                        {
                            var Partition = sqtsList.OrderByDescending(a => a.sqts.StatementDate).GroupBy(x => new
                            {
                                x.sqts.BeginingDateTime,
                                x.sqts.DeliveryLocation,
                                x.sqts.ReceiptLocation,
                                x.sqts.ServiceRequestorContract,
                                x.sqts.DownstreamID,
                                x.sqts.UpstreamId,
                                x.sqts.PackageId,
                                x.sqts.NomTrackingId
                            })
                           .SelectMany(g =>
                               g.Select((j, i) => new { j, rn = i + 1 })
                           ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                           .Select(a => new SummaryDTO
                           {
                               DelPointQty = a.j.sqts.DeliveryQuantity,
                               RecPointQty = a.j.sqts.ReceiptQuantity,
                               nomStartDate = a.j.sqts.BeginingDateTime,
                               nomEndDate = a.j.sqts.EndingDateTime,
                               DelLoc = a.j.sqts.DeliveryLocation,
                               RecLoc = a.j.sqts.ReceiptLocation,
                               ContractSVC = a.j.sqts.ServiceRequestorContract,
                               DownStreamName = a.j.sqts.DownstreamID,
                               UpStreamName = a.j.sqts.UpstreamId,
                               DwnIdentifier = a.j.sqts.DownstreamID,
                               UpIdentifier = a.j.sqts.UpstreamId,
                               StatementDate = a.j.sqts.StatementDate,
                               ReductionReason = a.j.sqts.ReductionReason,
                               Cycle = a.j.sqts.CycleIndicator,
                               PkgId = a.j.sqts.PackageId,
                               NomTrackingId = a.j.sqts.NomTrackingId,
                               Username = a.j.shipper.FirstName + " " + a.j.shipper.LastName,
                               ReductionReasonDetail = a.j.sqts.ReductionReasonDescription
                           }).ToList();
                            summaryDTOList = Partition;
                        }
                    }
                    else if (pathType == "D")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && sqts.ModelType == "N"
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).ToList();
                        if (sqtsList.Count > 0)
                        {
                            var Partition = sqtsList.OrderByDescending(a => a.sqts.StatementDate).GroupBy(x => new
                            {
                                x.sqts.BeginingDateTime,
                                x.sqts.DeliveryLocation,
                                x.sqts.ReceiptLocation,
                                x.sqts.ServiceRequestorContract,
                                x.sqts.DownstreamID,
                                x.sqts.UpstreamId,
                                x.sqts.PackageId,
                                x.sqts.NomTrackingId
                            })
                           .SelectMany(g =>
                               g.Select((j, i) => new { j, rn = i + 1 })
                           ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                           .Select(a => new SummaryDTO
                           {
                               DelPointQty = a.j.sqts.DeliveryQuantity,
                               RecPointQty = a.j.sqts.ReceiptQuantity,
                               nomStartDate = a.j.sqts.BeginingDateTime,
                               nomEndDate = a.j.sqts.EndingDateTime,
                               DelLoc = a.j.sqts.DeliveryLocation,
                               RecLoc = a.j.sqts.ReceiptLocation,
                               ContractSVC = a.j.sqts.ServiceRequestorContract,
                               DownStreamName = a.j.sqts.DownstreamID,
                               UpStreamName = a.j.sqts.UpstreamId,
                               DwnIdentifier = a.j.sqts.DownstreamID,
                               UpIdentifier = a.j.sqts.UpstreamId,
                               StatementDate = a.j.sqts.StatementDate,
                               ReductionReason = a.j.sqts.ReductionReason,
                               Cycle = a.j.sqts.CycleIndicator,
                               PkgId = a.j.sqts.PackageId,
                               NomTrackingId = a.j.sqts.NomTrackingId,
                               Username = a.j.shipper.FirstName + " " + a.j.shipper.LastName,
                               ReductionReasonDetail = a.j.sqts.ReductionReasonDescription
                           }).ToList();
                            summaryDTOList = Partition;
                        }
                    }
                }
                else
                {
                    if (pathType == "P" || pathType == "T")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && batch.CreatedBy == UserId
                                        && sqts.ModelType == pathType.Trim()
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).ToList();
                        if (sqtsList.Count > 0)
                        {
                            var Partition = sqtsList.OrderByDescending(a => a.sqts.StatementDate).GroupBy(x => new
                            {
                                x.sqts.BeginingDateTime,
                                x.sqts.DeliveryLocation,
                                x.sqts.ReceiptLocation,
                                x.sqts.ServiceRequestorContract,
                                x.sqts.DownstreamID,
                                x.sqts.UpstreamId,
                                x.sqts.PackageId,
                                x.sqts.NomTrackingId
                            })
                           .SelectMany(g =>
                               g.Select((j, i) => new { j, rn = i + 1 })
                           ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                           .Select(a => new SummaryDTO
                           {
                               DelPointQty = a.j.sqts.DeliveryQuantity,
                               RecPointQty = a.j.sqts.ReceiptQuantity,
                               nomStartDate = a.j.sqts.BeginingDateTime,
                               nomEndDate = a.j.sqts.EndingDateTime,
                               DelLoc = a.j.sqts.DeliveryLocation,
                               RecLoc = a.j.sqts.ReceiptLocation,
                               ContractSVC = a.j.sqts.ServiceRequestorContract,
                               DownStreamName = a.j.sqts.DownstreamID,
                               UpStreamName = a.j.sqts.UpstreamId,
                               DwnIdentifier = a.j.sqts.DownstreamID,
                               UpIdentifier = a.j.sqts.UpstreamId,
                               StatementDate = a.j.sqts.StatementDate,
                               ReductionReason = a.j.sqts.ReductionReason,
                               Cycle = a.j.sqts.CycleIndicator,
                               PkgId = a.j.sqts.PackageId,
                               NomTrackingId = a.j.sqts.NomTrackingId,
                               Username = a.j.shipper.FirstName + " " + a.j.shipper.LastName,
                               ReductionReasonDetail = a.j.sqts.ReductionReasonDescription
                           }).ToList();
                            summaryDTOList = Partition;
                        }
                    }
                    else if (pathType == "R")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && batch.CreatedBy == UserId
                                        && sqts.ModelType == "N"
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).ToList();
                        if (sqtsList.Count > 0)
                        {
                            var Partition = sqtsList.OrderByDescending(a => a.sqts.StatementDate).GroupBy(x => new
                            {
                                x.sqts.BeginingDateTime,
                                x.sqts.DeliveryLocation,
                                x.sqts.ReceiptLocation,
                                x.sqts.ServiceRequestorContract,
                                x.sqts.DownstreamID,
                                x.sqts.UpstreamId,
                                x.sqts.PackageId,
                                x.sqts.NomTrackingId
                            })
                           .SelectMany(g =>
                               g.Select((j, i) => new { j, rn = i + 1 })
                           ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                           .Select(a => new SummaryDTO
                           {
                               DelPointQty = a.j.sqts.DeliveryQuantity,
                               RecPointQty = a.j.sqts.ReceiptQuantity,
                               nomStartDate = a.j.sqts.BeginingDateTime,
                               nomEndDate = a.j.sqts.EndingDateTime,
                               DelLoc = a.j.sqts.DeliveryLocation,
                               RecLoc = a.j.sqts.ReceiptLocation,
                               ContractSVC = a.j.sqts.ServiceRequestorContract,
                               DownStreamName = a.j.sqts.DownstreamID,
                               UpStreamName = a.j.sqts.UpstreamId,
                               DwnIdentifier = a.j.sqts.DownstreamID,
                               UpIdentifier = a.j.sqts.UpstreamId,
                               StatementDate = a.j.sqts.StatementDate,
                               ReductionReason = a.j.sqts.ReductionReason,
                               Cycle = a.j.sqts.CycleIndicator,
                               PkgId = a.j.sqts.PackageId,
                               NomTrackingId = a.j.sqts.NomTrackingId,
                               Username = a.j.shipper.FirstName + " " + a.j.shipper.LastName,
                               ReductionReasonDetail = a.j.sqts.ReductionReasonDescription
                           }).ToList();
                            summaryDTOList = Partition;
                        }

                    }
                    else if (pathType == "D")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && batch.CreatedBy == UserId
                                        && sqts.ModelType == "N"
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).ToList();
                        if (sqtsList.Count > 0)
                        {
                            var Partition = sqtsList.OrderByDescending(a => a.sqts.StatementDate).GroupBy(x => new
                            {
                                x.sqts.BeginingDateTime,
                                x.sqts.DeliveryLocation,
                                x.sqts.ReceiptLocation,
                                x.sqts.ServiceRequestorContract,
                                x.sqts.DownstreamID,
                                x.sqts.UpstreamId,
                                x.sqts.PackageId,
                                x.sqts.NomTrackingId
                            })
                           .SelectMany(g =>
                               g.Select((j, i) => new { j, rn = i + 1 })
                           ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                           .Select(a => new SummaryDTO
                           {
                               DelPointQty = a.j.sqts.DeliveryQuantity,
                               RecPointQty = a.j.sqts.ReceiptQuantity,
                               nomStartDate = a.j.sqts.BeginingDateTime,
                               nomEndDate = a.j.sqts.EndingDateTime,
                               DelLoc = a.j.sqts.DeliveryLocation,
                               RecLoc = a.j.sqts.ReceiptLocation,
                               ContractSVC = a.j.sqts.ServiceRequestorContract,
                               DownStreamName = a.j.sqts.DownstreamID,
                               UpStreamName = a.j.sqts.UpstreamId,
                               DwnIdentifier = a.j.sqts.DownstreamID,
                               UpIdentifier = a.j.sqts.UpstreamId,
                               StatementDate = a.j.sqts.StatementDate,
                               ReductionReason = a.j.sqts.ReductionReason,
                               Cycle = a.j.sqts.CycleIndicator,
                               PkgId = a.j.sqts.PackageId,
                               NomTrackingId = a.j.sqts.NomTrackingId,
                               Username = a.j.shipper.FirstName + " " + a.j.shipper.LastName,
                               ReductionReasonDetail = a.j.sqts.ReductionReasonDescription
                           }).ToList();
                            summaryDTOList = Partition;
                        }
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(UserId))
                {
                    if (pathType == "P" || pathType == "T")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && sqts.ModelType == pathType.Trim()
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && (sqts.ReceiptQuantity!=0 || sqts.DeliveryQuantity!=0)
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).ToList();
                        if (sqtsList.Count > 0)
                        {
                            var Partition = sqtsList.OrderByDescending(a => a.sqts.StatementDate).GroupBy(x => new
                            {
                                x.sqts.BeginingDateTime,
                                x.sqts.DeliveryLocation,
                                x.sqts.ReceiptLocation,
                                x.sqts.ServiceRequestorContract,
                                x.sqts.DownstreamID,
                                x.sqts.UpstreamId,
                                x.sqts.PackageId,
                                x.sqts.NomTrackingId
                            })
                           .SelectMany(g =>
                               g.Select((j, i) => new { j, rn = i + 1 })
                           ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                           .Select(a => new SummaryDTO
                           {
                               DelPointQty = a.j.sqts.DeliveryQuantity,
                               RecPointQty = a.j.sqts.ReceiptQuantity,
                               nomStartDate = a.j.sqts.BeginingDateTime,
                               nomEndDate = a.j.sqts.EndingDateTime,
                               DelLoc = a.j.sqts.DeliveryLocation,
                               RecLoc = a.j.sqts.ReceiptLocation,
                               ContractSVC = a.j.sqts.ServiceRequestorContract,
                               DownStreamName = a.j.sqts.DownstreamID,
                               UpStreamName = a.j.sqts.UpstreamId,
                               DwnIdentifier = a.j.sqts.DownstreamID,
                               UpIdentifier = a.j.sqts.UpstreamId,
                               StatementDate = a.j.sqts.StatementDate,
                               ReductionReason = a.j.sqts.ReductionReason,
                               Cycle = a.j.sqts.CycleIndicator,
                               PkgId = a.j.sqts.PackageId,
                               NomTrackingId = a.j.sqts.NomTrackingId,
                               Username = a.j.shipper.FirstName + " " + a.j.shipper.LastName,
                               ReductionReasonDetail = a.j.sqts.ReductionReasonDescription
                           }).ToList();
                            summaryDTOList = Partition;
                        }
                    }
                    else if (pathType == "R")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && sqts.ModelType == "N"
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && sqts.BeginingDateTime.Month == month
                                        && (sqts.ReceiptQuantity != 0 || sqts.DeliveryQuantity != 0)
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).ToList();
                        if (sqtsList.Count > 0)
                        {
                            var Partition = sqtsList.OrderByDescending(a => a.sqts.StatementDate).GroupBy(x => new
                            {
                                x.sqts.BeginingDateTime,
                                x.sqts.DeliveryLocation,
                                x.sqts.ReceiptLocation,
                                x.sqts.ServiceRequestorContract,
                                x.sqts.DownstreamID,
                                x.sqts.UpstreamId,
                                x.sqts.PackageId,
                                x.sqts.NomTrackingId
                            })
                           .SelectMany(g =>
                               g.Select((j, i) => new { j, rn = i + 1 })
                           ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                           .Select(a => new SummaryDTO
                           {
                               DelPointQty = a.j.sqts.DeliveryQuantity,
                               RecPointQty = a.j.sqts.ReceiptQuantity,
                               nomStartDate = a.j.sqts.BeginingDateTime,
                               nomEndDate = a.j.sqts.EndingDateTime,
                               DelLoc = a.j.sqts.DeliveryLocation,
                               RecLoc = a.j.sqts.ReceiptLocation,
                               ContractSVC = a.j.sqts.ServiceRequestorContract,
                               DownStreamName = a.j.sqts.DownstreamID,
                               UpStreamName = a.j.sqts.UpstreamId,
                               DwnIdentifier = a.j.sqts.DownstreamID,
                               UpIdentifier = a.j.sqts.UpstreamId,
                               StatementDate = a.j.sqts.StatementDate,
                               ReductionReason = a.j.sqts.ReductionReason,
                               Cycle = a.j.sqts.CycleIndicator,
                               PkgId = a.j.sqts.PackageId,
                               NomTrackingId = a.j.sqts.NomTrackingId,
                               Username = a.j.shipper.FirstName + " " + a.j.shipper.LastName,
                               ReductionReasonDetail = a.j.sqts.ReductionReasonDescription
                           }).ToList();
                            summaryDTOList = Partition;
                        }
                    }
                    else if (pathType == "D")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && sqts.ModelType == "N"
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && sqts.BeginingDateTime.Month == month
                                        && (sqts.ReceiptQuantity != 0 || sqts.DeliveryQuantity != 0)
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).ToList();
                        if (sqtsList.Count > 0)
                        {
                            var Partition = sqtsList.OrderByDescending(a => a.sqts.StatementDate).GroupBy(x => new
                            {
                                x.sqts.BeginingDateTime,
                                x.sqts.DeliveryLocation,
                                x.sqts.ReceiptLocation,
                                x.sqts.ServiceRequestorContract,
                                x.sqts.DownstreamID,
                                x.sqts.UpstreamId,
                                x.sqts.PackageId,
                                x.sqts.NomTrackingId
                            })
                           .SelectMany(g =>
                               g.Select((j, i) => new { j, rn = i + 1 })
                           ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                           .Select(a => new SummaryDTO
                           {
                               DelPointQty = a.j.sqts.DeliveryQuantity,
                               RecPointQty = a.j.sqts.ReceiptQuantity,
                               nomStartDate = a.j.sqts.BeginingDateTime,
                               nomEndDate = a.j.sqts.EndingDateTime,
                               DelLoc = a.j.sqts.DeliveryLocation,
                               RecLoc = a.j.sqts.ReceiptLocation,
                               ContractSVC = a.j.sqts.ServiceRequestorContract,
                               DownStreamName = a.j.sqts.DownstreamID,
                               UpStreamName = a.j.sqts.UpstreamId,
                               DwnIdentifier = a.j.sqts.DownstreamID,
                               UpIdentifier = a.j.sqts.UpstreamId,
                               StatementDate = a.j.sqts.StatementDate,
                               ReductionReason = a.j.sqts.ReductionReason,
                               Cycle = a.j.sqts.CycleIndicator,
                               PkgId = a.j.sqts.PackageId,
                               NomTrackingId = a.j.sqts.NomTrackingId,
                               Username = a.j.shipper.FirstName + " " + a.j.shipper.LastName,
                               ReductionReasonDetail = a.j.sqts.ReductionReasonDescription
                           }).ToList();
                            summaryDTOList = Partition;
                        }
                    }
                }
                else
                {
                    if (pathType == "P" || pathType == "T")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && batch.CreatedBy == UserId
                                        && sqts.ModelType == pathType.Trim()
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && (sqts.ReceiptQuantity != 0 || sqts.DeliveryQuantity != 0)
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).ToList();
                        if (sqtsList.Count > 0)
                        {
                            var Partition = sqtsList.OrderByDescending(a => a.sqts.StatementDate).GroupBy(x => new
                            {
                                x.sqts.BeginingDateTime,
                                x.sqts.DeliveryLocation,
                                x.sqts.ReceiptLocation,
                                x.sqts.ServiceRequestorContract,
                                x.sqts.DownstreamID,
                                x.sqts.UpstreamId,
                                x.sqts.PackageId,
                                x.sqts.NomTrackingId
                            })
                           .SelectMany(g =>
                               g.Select((j, i) => new { j, rn = i + 1 })
                           ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                           .Select(a => new SummaryDTO
                           {
                               DelPointQty = a.j.sqts.DeliveryQuantity,
                               RecPointQty = a.j.sqts.ReceiptQuantity,
                               nomStartDate = a.j.sqts.BeginingDateTime,
                               nomEndDate = a.j.sqts.EndingDateTime,
                               DelLoc = a.j.sqts.DeliveryLocation,
                               RecLoc = a.j.sqts.ReceiptLocation,
                               ContractSVC = a.j.sqts.ServiceRequestorContract,
                               DownStreamName = a.j.sqts.DownstreamID,
                               UpStreamName = a.j.sqts.UpstreamId,
                               DwnIdentifier = a.j.sqts.DownstreamID,
                               UpIdentifier = a.j.sqts.UpstreamId,
                               StatementDate = a.j.sqts.StatementDate,
                               ReductionReason = a.j.sqts.ReductionReason,
                               Cycle = a.j.sqts.CycleIndicator,
                               PkgId = a.j.sqts.PackageId,
                               NomTrackingId = a.j.sqts.NomTrackingId,
                               Username = a.j.shipper.FirstName + " " + a.j.shipper.LastName,
                               ReductionReasonDetail = a.j.sqts.ReductionReasonDescription
                           }).ToList();
                            summaryDTOList = Partition;
                        }
                    }
                    else if (pathType == "R")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && batch.CreatedBy == UserId
                                        && sqts.ModelType == "N"
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && (sqts.ReceiptQuantity != 0 || sqts.DeliveryQuantity != 0)
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).ToList();
                        if (sqtsList.Count > 0)
                        {
                            var Partition = sqtsList.OrderByDescending(a => a.sqts.StatementDate).GroupBy(x => new
                            {
                                x.sqts.BeginingDateTime,
                                x.sqts.DeliveryLocation,
                                x.sqts.ReceiptLocation,
                                x.sqts.ServiceRequestorContract,
                                x.sqts.DownstreamID,
                                x.sqts.UpstreamId,
                                x.sqts.PackageId,
                                x.sqts.NomTrackingId
                            })
                           .SelectMany(g =>
                               g.Select((j, i) => new { j, rn = i + 1 })
                           ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                           .Select(a => new SummaryDTO
                           {
                               DelPointQty = a.j.sqts.DeliveryQuantity,
                               RecPointQty = a.j.sqts.ReceiptQuantity,
                               nomStartDate = a.j.sqts.BeginingDateTime,
                               nomEndDate = a.j.sqts.EndingDateTime,
                               DelLoc = a.j.sqts.DeliveryLocation,
                               RecLoc = a.j.sqts.ReceiptLocation,
                               ContractSVC = a.j.sqts.ServiceRequestorContract,
                               DownStreamName = a.j.sqts.DownstreamID,
                               UpStreamName = a.j.sqts.UpstreamId,
                               DwnIdentifier = a.j.sqts.DownstreamID,
                               UpIdentifier = a.j.sqts.UpstreamId,
                               StatementDate = a.j.sqts.StatementDate,
                               ReductionReason = a.j.sqts.ReductionReason,
                               Cycle = a.j.sqts.CycleIndicator,
                               PkgId = a.j.sqts.PackageId,
                               NomTrackingId = a.j.sqts.NomTrackingId,
                               Username = a.j.shipper.FirstName + " " + a.j.shipper.LastName,
                               ReductionReasonDetail = a.j.sqts.ReductionReasonDescription
                           }).ToList();
                            summaryDTOList = Partition;
                        }

                    }
                    else if (pathType == "D")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && batch.CreatedBy == UserId
                                        && sqts.ModelType == "N"
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && (sqts.ReceiptQuantity != 0 || sqts.DeliveryQuantity != 0)
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).ToList();
                        if (sqtsList.Count > 0)
                        {
                            var Partition = sqtsList.OrderByDescending(a => a.sqts.StatementDate).GroupBy(x => new
                            {
                                x.sqts.BeginingDateTime,
                                x.sqts.DeliveryLocation,
                                x.sqts.ReceiptLocation,
                                x.sqts.ServiceRequestorContract,
                                x.sqts.DownstreamID,
                                x.sqts.UpstreamId,
                                x.sqts.PackageId,
                                x.sqts.NomTrackingId
                            })
                           .SelectMany(g =>
                               g.Select((j, i) => new { j, rn = i + 1 })
                           ).Where(a => a.rn == 1).Skip(skipSize).Take(pageSize)
                           .Select(a => new SummaryDTO
                           {
                               DelPointQty = a.j.sqts.DeliveryQuantity,
                               RecPointQty = a.j.sqts.ReceiptQuantity,
                               nomStartDate = a.j.sqts.BeginingDateTime,
                               nomEndDate = a.j.sqts.EndingDateTime,
                               DelLoc = a.j.sqts.DeliveryLocation,
                               RecLoc = a.j.sqts.ReceiptLocation,
                               ContractSVC = a.j.sqts.ServiceRequestorContract,
                               DownStreamName = a.j.sqts.DownstreamID,
                               UpStreamName = a.j.sqts.UpstreamId,
                               DwnIdentifier = a.j.sqts.DownstreamID,
                               UpIdentifier = a.j.sqts.UpstreamId,
                               StatementDate = a.j.sqts.StatementDate,
                               ReductionReason = a.j.sqts.ReductionReason,
                               Cycle = a.j.sqts.CycleIndicator,
                               PkgId = a.j.sqts.PackageId,
                               NomTrackingId = a.j.sqts.NomTrackingId,
                               Username = a.j.shipper.FirstName + " " + a.j.shipper.LastName,
                               ReductionReasonDetail = a.j.sqts.ReductionReasonDescription
                           }).ToList();
                            summaryDTOList = Partition;
                        }
                    }
                }
            }
            return summaryDTOList;
        }

        public List<SummaryDTO> GetSqtsOrphanDataForExcel(int month, string pipeDuns, string pathType, string shipperCompanyDuns, bool showZero)
        {
            List<SummaryDTO> summaryDTOList = new List<SummaryDTO>();
            int currentYear = DateTime.Now.Year;
            //int skipSize = (pageNo - 1) * pageSize;
            if (!showZero)
            {
                if (pathType == "P")
                {
                    var sqtsList = DbContext.SQTSPerTransaction.Where(sqts =>
                                !string.IsNullOrEmpty(sqts.DeliveryLocation) &&
                                !string.IsNullOrEmpty(sqts.ReceiptLocation) &&
                                !string.IsNullOrEmpty(sqts.ServiceRequestorContract) &&
                                sqts.ServiceRequestor == shipperCompanyDuns &&
                                sqts.ModelType == "P" &&
                                sqts.NomTrackingId.Contains("N/A") &&
                                sqts.TSPCode == pipeDuns &&
                                sqts.BeginingDateTime.Month == month &&
                                sqts.BeginingDateTime.Year == currentYear
                    ).Select(a => new SummaryDTO
                    {
                        DelPointQty = a.DeliveryQuantity,
                        RecPointQty = a.ReceiptQuantity,
                        nomStartDate = a.BeginingDateTime,
                        nomEndDate = a.EndingDateTime,
                        DelLoc = a.DeliveryLocation,
                        RecLoc = a.ReceiptLocation,
                        ContractSVC = a.ServiceRequestorContract,
                        DwnIdentifier = a.DownstreamID,
                        UpIdentifier = a.UpstreamId,
                        StatementDate = a.StatementDate,
                        ReductionReason = a.ReductionReason,
                        Cycle = a.CycleIndicator,
                        PkgId = a.PackageId,
                        NomTrackingId = a.NomTrackingId,
                        ReductionReasonDetail=a.ReductionReasonDescription
                    }).OrderByDescending(a => a.StatementDate).ToList();
                    summaryDTOList = sqtsList;
                }
                else if (pathType == "T")
                {
                    var sqtsList = DbContext.SQTSPerTransaction.Where(sqts =>
                                !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                && sqts.ServiceRequestor == shipperCompanyDuns
                                && sqts.ModelType == "T"
                                && sqts.NomTrackingId.Contains("N/A")
                                && sqts.TSPCode == pipeDuns
                                && sqts.BeginingDateTime.Month == month
                                && sqts.BeginingDateTime.Year == currentYear
                    ).Select(a => new SummaryDTO
                    {
                        DelPointQty = a.DeliveryQuantity,
                        RecPointQty = a.ReceiptQuantity,
                        nomStartDate = a.BeginingDateTime,
                        nomEndDate = a.EndingDateTime,
                        DelLoc = a.DeliveryLocation,
                        RecLoc = a.ReceiptLocation,
                        ContractSVC = a.ServiceRequestorContract,
                        DwnIdentifier = a.DownstreamID,
                        UpIdentifier = a.UpstreamId,
                        StatementDate = a.StatementDate,
                        ReductionReason = a.ReductionReason,
                        Cycle = a.CycleIndicator,
                        PkgId = a.PackageId,
                        NomTrackingId = a.NomTrackingId,
                        ReductionReasonDetail = a.ReductionReasonDescription
                    }).OrderByDescending(a => a.StatementDate).ToList();
                    summaryDTOList = sqtsList;
                }
                else if (pathType == "R")
                {
                    var sqtsList = DbContext.SQTSPerTransaction.Where(sqts =>
                                string.IsNullOrEmpty(sqts.DeliveryLocation)
                                && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                && sqts.ServiceRequestor == shipperCompanyDuns
                                && sqts.ModelType == "N"
                                && sqts.NomTrackingId.Contains("N/A")
                                && sqts.TSPCode == pipeDuns
                                && sqts.BeginingDateTime.Month == month
                                && sqts.BeginingDateTime.Year == currentYear
                    ).Select(a => new SummaryDTO
                    {
                        DelPointQty = a.DeliveryQuantity,
                        RecPointQty = a.ReceiptQuantity,
                        nomStartDate = a.BeginingDateTime,
                        nomEndDate = a.EndingDateTime,
                        DelLoc = a.DeliveryLocation,
                        RecLoc = a.ReceiptLocation,
                        ContractSVC = a.ServiceRequestorContract,
                        DwnIdentifier = a.DownstreamID,
                        UpIdentifier = a.UpstreamId,
                        StatementDate = a.StatementDate,
                        ReductionReason = a.ReductionReason,
                        Cycle = a.CycleIndicator,
                        PkgId = a.PackageId,
                        NomTrackingId = a.NomTrackingId,
                        ReductionReasonDetail = a.ReductionReasonDescription
                    }).OrderByDescending(a => a.StatementDate).ToList();
                    summaryDTOList = sqtsList;
                }
                else if (pathType == "D")
                {
                    var sqtsList = DbContext.SQTSPerTransaction.Where(sqts =>
                                !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                && string.IsNullOrEmpty(sqts.ReceiptLocation)
                                && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                && sqts.ServiceRequestor == shipperCompanyDuns
                                && sqts.ModelType == "N"
                                && sqts.NomTrackingId.Contains("N/A")
                                && sqts.TSPCode == pipeDuns
                                && sqts.BeginingDateTime.Month == month
                                && sqts.BeginingDateTime.Year == currentYear
                    ).Select(a => new SummaryDTO
                    {
                        DelPointQty = a.DeliveryQuantity,
                        RecPointQty = a.ReceiptQuantity,
                        nomStartDate = a.BeginingDateTime,
                        nomEndDate = a.EndingDateTime,
                        DelLoc = a.DeliveryLocation,
                        RecLoc = a.ReceiptLocation,
                        ContractSVC = a.ServiceRequestorContract,
                        DwnIdentifier = a.DownstreamID,
                        UpIdentifier = a.UpstreamId,
                        StatementDate = a.StatementDate,
                        ReductionReason = a.ReductionReason,
                        Cycle = a.CycleIndicator,
                        PkgId = a.PackageId,
                        NomTrackingId = a.NomTrackingId,
                        ReductionReasonDetail = a.ReductionReasonDescription
                    }).OrderByDescending(a => a.StatementDate).ToList();
                    summaryDTOList = sqtsList;
                }
            }
            else
            {
                if (pathType == "P")
                {
                    var sqtsList = DbContext.SQTSPerTransaction.Where(sqts =>
                                !string.IsNullOrEmpty(sqts.DeliveryLocation) &&
                                !string.IsNullOrEmpty(sqts.ReceiptLocation) &&
                                !string.IsNullOrEmpty(sqts.ServiceRequestorContract) &&
                                sqts.ServiceRequestor == shipperCompanyDuns &&
                                sqts.ModelType == "P" &&
                                sqts.NomTrackingId.Contains("N/A") &&
                                sqts.TSPCode == pipeDuns &&
                                (sqts.ReceiptQuantity != 0 || sqts.DeliveryQuantity != 0) &&
                                sqts.BeginingDateTime.Month == month &&
                                sqts.BeginingDateTime.Year == currentYear
                    ).Select(a => new SummaryDTO
                    {
                        DelPointQty = a.DeliveryQuantity,
                        RecPointQty = a.ReceiptQuantity,
                        nomStartDate = a.BeginingDateTime,
                        nomEndDate = a.EndingDateTime,
                        DelLoc = a.DeliveryLocation,
                        RecLoc = a.ReceiptLocation,
                        ContractSVC = a.ServiceRequestorContract,
                        DwnIdentifier = a.DownstreamID,
                        UpIdentifier = a.UpstreamId,
                        StatementDate = a.StatementDate,
                        ReductionReason = a.ReductionReason,
                        Cycle = a.CycleIndicator,
                        PkgId = a.PackageId,
                        NomTrackingId = a.NomTrackingId,
                        ReductionReasonDetail = a.ReductionReasonDescription
                    }).OrderByDescending(a => a.StatementDate).ToList();
                    summaryDTOList = sqtsList;
                }
                else if (pathType == "T")
                {
                    var sqtsList = DbContext.SQTSPerTransaction.Where(sqts =>
                                !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                && sqts.ServiceRequestor == shipperCompanyDuns
                                && sqts.ModelType == "T"
                                && sqts.NomTrackingId.Contains("N/A")
                                && sqts.TSPCode == pipeDuns
                                && (sqts.ReceiptQuantity != 0 || sqts.DeliveryQuantity != 0)
                                && sqts.BeginingDateTime.Month == month
                                && sqts.BeginingDateTime.Year == currentYear
                    ).Select(a => new SummaryDTO
                    {
                        DelPointQty = a.DeliveryQuantity,
                        RecPointQty = a.ReceiptQuantity,
                        nomStartDate = a.BeginingDateTime,
                        nomEndDate = a.EndingDateTime,
                        DelLoc = a.DeliveryLocation,
                        RecLoc = a.ReceiptLocation,
                        ContractSVC = a.ServiceRequestorContract,
                        DwnIdentifier = a.DownstreamID,
                        UpIdentifier = a.UpstreamId,
                        StatementDate = a.StatementDate,
                        ReductionReason = a.ReductionReason,
                        Cycle = a.CycleIndicator,
                        PkgId = a.PackageId,
                        NomTrackingId = a.NomTrackingId,
                        ReductionReasonDetail = a.ReductionReasonDescription
                    }).OrderByDescending(a => a.StatementDate).ToList();
                    summaryDTOList = sqtsList;
                }
                else if (pathType == "R")
                {
                    var sqtsList = DbContext.SQTSPerTransaction.Where(sqts =>
                                string.IsNullOrEmpty(sqts.DeliveryLocation)
                                && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                && sqts.ServiceRequestor == shipperCompanyDuns
                                && sqts.ModelType == "N"
                                && sqts.NomTrackingId.Contains("N/A")
                                && sqts.TSPCode == pipeDuns
                                && (sqts.ReceiptQuantity != 0 || sqts.DeliveryQuantity != 0)
                                && sqts.BeginingDateTime.Month == month
                                && sqts.BeginingDateTime.Year == currentYear
                    ).Select(a => new SummaryDTO
                    {
                        DelPointQty = a.DeliveryQuantity,
                        RecPointQty = a.ReceiptQuantity,
                        nomStartDate = a.BeginingDateTime,
                        nomEndDate = a.EndingDateTime,
                        DelLoc = a.DeliveryLocation,
                        RecLoc = a.ReceiptLocation,
                        ContractSVC = a.ServiceRequestorContract,
                        DwnIdentifier = a.DownstreamID,
                        UpIdentifier = a.UpstreamId,
                        StatementDate = a.StatementDate,
                        ReductionReason = a.ReductionReason,
                        Cycle = a.CycleIndicator,
                        PkgId = a.PackageId,
                        NomTrackingId = a.NomTrackingId,
                        ReductionReasonDetail = a.ReductionReasonDescription
                    }).OrderByDescending(a => a.StatementDate).ToList();
                    summaryDTOList = sqtsList;
                }
                else if (pathType == "D")
                {
                    var sqtsList = DbContext.SQTSPerTransaction.Where(sqts =>
                                !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                && string.IsNullOrEmpty(sqts.ReceiptLocation)
                                && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                && sqts.ServiceRequestor == shipperCompanyDuns
                                && sqts.ModelType == "N"
                                && sqts.NomTrackingId.Contains("N/A")
                                && sqts.TSPCode == pipeDuns
                                && (sqts.ReceiptQuantity != 0 || sqts.DeliveryQuantity != 0)
                                && sqts.BeginingDateTime.Month == month
                                && sqts.BeginingDateTime.Year == currentYear
                    ).Select(a => new SummaryDTO
                    {
                        DelPointQty = a.DeliveryQuantity,
                        RecPointQty = a.ReceiptQuantity,
                        nomStartDate = a.BeginingDateTime,
                        nomEndDate = a.EndingDateTime,
                        DelLoc = a.DeliveryLocation,
                        RecLoc = a.ReceiptLocation,
                        ContractSVC = a.ServiceRequestorContract,
                        DwnIdentifier = a.DownstreamID,
                        UpIdentifier = a.UpstreamId,
                        StatementDate = a.StatementDate,
                        ReductionReason = a.ReductionReason,
                        Cycle = a.CycleIndicator,
                        PkgId = a.PackageId,
                        NomTrackingId = a.NomTrackingId,
                        ReductionReasonDetail = a.ReductionReasonDescription
                    }).OrderByDescending(a => a.StatementDate).ToList();
                    summaryDTOList = sqtsList;
                }
            }
            return summaryDTOList;
        }

        public List<SummaryDTO> GetSqtsDataForExcel(int month, string pipeDuns, string UserId, string pathType, string shipperCompanyDuns, bool showZero)
        {
            List<SummaryDTO> summaryDTOList = new List<SummaryDTO>();
            int currentYear = DateTime.Now.Year;
            //int skipSize = (pageNo - 1) * pageSize;
            if (!showZero)
            {
                if (string.IsNullOrEmpty(UserId))
                {
                    if (pathType == "P" || pathType == "T")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && sqts.ModelType == pathType.Trim()
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).Select(a => new SummaryDTO
                                        {
                                            DelPointQty = a.sqts.DeliveryQuantity,
                                            RecPointQty = a.sqts.ReceiptQuantity,
                                            nomStartDate = a.sqts.BeginingDateTime,
                                            nomEndDate = a.sqts.EndingDateTime,
                                            DelLoc = a.sqts.DeliveryLocation,
                                            RecLoc = a.sqts.ReceiptLocation,
                                            ContractSVC = a.sqts.ServiceRequestorContract,
                                            DwnIdentifier = a.sqts.DownstreamID,
                                            UpIdentifier = a.sqts.UpstreamId,
                                            StatementDate = a.sqts.StatementDate,
                                            ReductionReason = a.sqts.ReductionReason,
                                            Cycle = a.sqts.CycleIndicator,
                                            PkgId = a.sqts.PackageId,
                                            NomTrackingId = a.sqts.NomTrackingId,
                                            Username = a.shipper.FirstName + " " + a.shipper.LastName,
                                            ReductionReasonDetail = a.sqts.ReductionReasonDescription
                                        }).OrderByDescending(a=>a.StatementDate).ToList();

                        summaryDTOList = sqtsList;
                        
                    }
                    else if (pathType == "R")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && sqts.ModelType == "N"
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).Select(a => new SummaryDTO
                                        {
                                            DelPointQty = a.sqts.DeliveryQuantity,
                                            RecPointQty = a.sqts.ReceiptQuantity,
                                            nomStartDate = a.sqts.BeginingDateTime,
                                            nomEndDate = a.sqts.EndingDateTime,
                                            DelLoc = a.sqts.DeliveryLocation,
                                            RecLoc = a.sqts.ReceiptLocation,
                                            ContractSVC = a.sqts.ServiceRequestorContract,
                                            DwnIdentifier = a.sqts.DownstreamID,
                                            UpIdentifier = a.sqts.UpstreamId,
                                            StatementDate = a.sqts.StatementDate,
                                            ReductionReason = a.sqts.ReductionReason,
                                            Cycle = a.sqts.CycleIndicator,
                                            PkgId = a.sqts.PackageId,
                                            NomTrackingId = a.sqts.NomTrackingId,
                                            Username = a.shipper.FirstName + " " + a.shipper.LastName,
                                            ReductionReasonDetail = a.sqts.ReductionReasonDescription
                                        }).OrderByDescending(a => a.StatementDate).ToList();
                        summaryDTOList = sqtsList;
                    }
                    else if (pathType == "D")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && sqts.ModelType == "N"
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).Select(a => new SummaryDTO
                                        {
                                            DelPointQty = a.sqts.DeliveryQuantity,
                                            RecPointQty = a.sqts.ReceiptQuantity,
                                            nomStartDate = a.sqts.BeginingDateTime,
                                            nomEndDate = a.sqts.EndingDateTime,
                                            DelLoc = a.sqts.DeliveryLocation,
                                            RecLoc = a.sqts.ReceiptLocation,
                                            ContractSVC = a.sqts.ServiceRequestorContract,
                                            DwnIdentifier = a.sqts.DownstreamID,
                                            UpIdentifier = a.sqts.UpstreamId,
                                            StatementDate = a.sqts.StatementDate,
                                            ReductionReason = a.sqts.ReductionReason,
                                            Cycle = a.sqts.CycleIndicator,
                                            PkgId = a.sqts.PackageId,
                                            NomTrackingId = a.sqts.NomTrackingId,
                                            Username = a.shipper.FirstName + " " + a.shipper.LastName,
                                            ReductionReasonDetail = a.sqts.ReductionReasonDescription
                                        }).OrderByDescending(a => a.StatementDate).ToList();
                        summaryDTOList = sqtsList;
                    }
                }
                else
                {
                    if (pathType == "P" || pathType == "T")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && batch.CreatedBy == UserId
                                        && sqts.ModelType == pathType.Trim()
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).Select(a => new SummaryDTO
                                        {
                                            DelPointQty = a.sqts.DeliveryQuantity,
                                            RecPointQty = a.sqts.ReceiptQuantity,
                                            nomStartDate = a.sqts.BeginingDateTime,
                                            nomEndDate = a.sqts.EndingDateTime,
                                            DelLoc = a.sqts.DeliveryLocation,
                                            RecLoc = a.sqts.ReceiptLocation,
                                            ContractSVC = a.sqts.ServiceRequestorContract,
                                            DwnIdentifier = a.sqts.DownstreamID,
                                            UpIdentifier = a.sqts.UpstreamId,
                                            StatementDate = a.sqts.StatementDate,
                                            ReductionReason = a.sqts.ReductionReason,
                                            Cycle = a.sqts.CycleIndicator,
                                            PkgId = a.sqts.PackageId,
                                            NomTrackingId = a.sqts.NomTrackingId,
                                            Username = a.shipper.FirstName + " " + a.shipper.LastName,
                                            ReductionReasonDetail = a.sqts.ReductionReasonDescription
                                        }).OrderByDescending(a => a.StatementDate).ToList();
                        summaryDTOList = sqtsList;
                    }
                    else if (pathType == "R")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && batch.CreatedBy == UserId
                                        && sqts.ModelType == "N"
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).Select(a => new SummaryDTO
                                        {
                                            DelPointQty = a.sqts.DeliveryQuantity,
                                            RecPointQty = a.sqts.ReceiptQuantity,
                                            nomStartDate = a.sqts.BeginingDateTime,
                                            nomEndDate = a.sqts.EndingDateTime,
                                            DelLoc = a.sqts.DeliveryLocation,
                                            RecLoc = a.sqts.ReceiptLocation,
                                            ContractSVC = a.sqts.ServiceRequestorContract,
                                            DwnIdentifier = a.sqts.DownstreamID,
                                            UpIdentifier = a.sqts.UpstreamId,
                                            StatementDate = a.sqts.StatementDate,
                                            ReductionReason = a.sqts.ReductionReason,
                                            Cycle = a.sqts.CycleIndicator,
                                            PkgId = a.sqts.PackageId,
                                            NomTrackingId = a.sqts.NomTrackingId,
                                            Username = a.shipper.FirstName + " " + a.shipper.LastName,
                                            ReductionReasonDetail = a.sqts.ReductionReasonDescription
                                        }).OrderByDescending(a => a.StatementDate).ToList();
                        summaryDTOList = sqtsList;
                    }
                    else if (pathType == "D")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && batch.CreatedBy == UserId
                                        && sqts.ModelType == "N"
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).Select(a => new SummaryDTO
                                        {
                                            DelPointQty = a.sqts.DeliveryQuantity,
                                            RecPointQty = a.sqts.ReceiptQuantity,
                                            nomStartDate = a.sqts.BeginingDateTime,
                                            nomEndDate = a.sqts.EndingDateTime,
                                            DelLoc = a.sqts.DeliveryLocation,
                                            RecLoc = a.sqts.ReceiptLocation,
                                            ContractSVC = a.sqts.ServiceRequestorContract,
                                            DwnIdentifier = a.sqts.DownstreamID,
                                            UpIdentifier = a.sqts.UpstreamId,
                                            StatementDate = a.sqts.StatementDate,
                                            ReductionReason = a.sqts.ReductionReason,
                                            Cycle = a.sqts.CycleIndicator,
                                            PkgId = a.sqts.PackageId,
                                            NomTrackingId = a.sqts.NomTrackingId,
                                            Username = a.shipper.FirstName + " " + a.shipper.LastName,
                                            ReductionReasonDetail = a.sqts.ReductionReasonDescription
                                        }).OrderByDescending(a => a.StatementDate).ToList();
                        summaryDTOList = sqtsList;
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(UserId))
                {
                    if (pathType == "P" || pathType == "T")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && sqts.ModelType == pathType.Trim()
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && (sqts.ReceiptQuantity != 0 || sqts.DeliveryQuantity != 0)
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).Select(a => new SummaryDTO
                                        {
                                            DelPointQty = a.sqts.DeliveryQuantity,
                                            RecPointQty = a.sqts.ReceiptQuantity,
                                            nomStartDate = a.sqts.BeginingDateTime,
                                            nomEndDate = a.sqts.EndingDateTime,
                                            DelLoc = a.sqts.DeliveryLocation,
                                            RecLoc = a.sqts.ReceiptLocation,
                                            ContractSVC = a.sqts.ServiceRequestorContract,
                                            DwnIdentifier = a.sqts.DownstreamID,
                                            UpIdentifier = a.sqts.UpstreamId,
                                            StatementDate = a.sqts.StatementDate,
                                            ReductionReason = a.sqts.ReductionReason,
                                            Cycle = a.sqts.CycleIndicator,
                                            PkgId = a.sqts.PackageId,
                                            NomTrackingId = a.sqts.NomTrackingId,
                                            Username = a.shipper.FirstName + " " + a.shipper.LastName,
                                            ReductionReasonDetail = a.sqts.ReductionReasonDescription
                                        }).OrderByDescending(a => a.StatementDate).ToList();
                        summaryDTOList = sqtsList;
                    }
                    else if (pathType == "R")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && sqts.ModelType == "N"
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && sqts.BeginingDateTime.Month == month
                                        && (sqts.ReceiptQuantity != 0 || sqts.DeliveryQuantity != 0)
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).Select(a => new SummaryDTO
                                        {
                                            DelPointQty = a.sqts.DeliveryQuantity,
                                            RecPointQty = a.sqts.ReceiptQuantity,
                                            nomStartDate = a.sqts.BeginingDateTime,
                                            nomEndDate = a.sqts.EndingDateTime,
                                            DelLoc = a.sqts.DeliveryLocation,
                                            RecLoc = a.sqts.ReceiptLocation,
                                            ContractSVC = a.sqts.ServiceRequestorContract,
                                            DwnIdentifier = a.sqts.DownstreamID,
                                            UpIdentifier = a.sqts.UpstreamId,
                                            StatementDate = a.sqts.StatementDate,
                                            ReductionReason = a.sqts.ReductionReason,
                                            Cycle = a.sqts.CycleIndicator,
                                            PkgId = a.sqts.PackageId,
                                            NomTrackingId = a.sqts.NomTrackingId,
                                            Username = a.shipper.FirstName + " " + a.shipper.LastName,
                                            ReductionReasonDetail = a.sqts.ReductionReasonDescription
                                        }).OrderByDescending(a => a.StatementDate).ToList();
                        summaryDTOList = sqtsList;
                    }
                    else if (pathType == "D")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && sqts.ModelType == "N"
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && sqts.BeginingDateTime.Month == month
                                        && (sqts.ReceiptQuantity != 0 || sqts.DeliveryQuantity != 0)
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).Select(a => new SummaryDTO
                                        {
                                            DelPointQty = a.sqts.DeliveryQuantity,
                                            RecPointQty = a.sqts.ReceiptQuantity,
                                            nomStartDate = a.sqts.BeginingDateTime,
                                            nomEndDate = a.sqts.EndingDateTime,
                                            DelLoc = a.sqts.DeliveryLocation,
                                            RecLoc = a.sqts.ReceiptLocation,
                                            ContractSVC = a.sqts.ServiceRequestorContract,
                                            DwnIdentifier = a.sqts.DownstreamID,
                                            UpIdentifier = a.sqts.UpstreamId,
                                            StatementDate = a.sqts.StatementDate,
                                            ReductionReason = a.sqts.ReductionReason,
                                            Cycle = a.sqts.CycleIndicator,
                                            PkgId = a.sqts.PackageId,
                                            NomTrackingId = a.sqts.NomTrackingId,
                                            Username = a.shipper.FirstName + " " + a.shipper.LastName,
                                            ReductionReasonDetail = a.sqts.ReductionReasonDescription
                                        }).OrderByDescending(a => a.StatementDate).ToList();
                        summaryDTOList = sqtsList;
                    }
                }
                else
                {
                    if (pathType == "P" || pathType == "T")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && batch.CreatedBy == UserId
                                        && sqts.ModelType == pathType.Trim()
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && (sqts.ReceiptQuantity != 0 || sqts.DeliveryQuantity != 0)
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).Select(a => new SummaryDTO
                                        {
                                            DelPointQty = a.sqts.DeliveryQuantity,
                                            RecPointQty = a.sqts.ReceiptQuantity,
                                            nomStartDate = a.sqts.BeginingDateTime,
                                            nomEndDate = a.sqts.EndingDateTime,
                                            DelLoc = a.sqts.DeliveryLocation,
                                            RecLoc = a.sqts.ReceiptLocation,
                                            ContractSVC = a.sqts.ServiceRequestorContract,
                                            DwnIdentifier = a.sqts.DownstreamID,
                                            UpIdentifier = a.sqts.UpstreamId,
                                            StatementDate = a.sqts.StatementDate,
                                            ReductionReason = a.sqts.ReductionReason,
                                            Cycle = a.sqts.CycleIndicator,
                                            PkgId = a.sqts.PackageId,
                                            NomTrackingId = a.sqts.NomTrackingId,
                                            Username = a.shipper.FirstName + " " + a.shipper.LastName,
                                            ReductionReasonDetail = a.sqts.ReductionReasonDescription
                                        }).OrderByDescending(a => a.StatementDate).ToList();
                        summaryDTOList = sqtsList;
                    }
                    else if (pathType == "R")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && !string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && batch.CreatedBy == UserId
                                        && sqts.ModelType == "N"
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && (sqts.ReceiptQuantity != 0 || sqts.DeliveryQuantity != 0)
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).Select(a => new SummaryDTO
                                        {
                                            DelPointQty = a.sqts.DeliveryQuantity,
                                            RecPointQty = a.sqts.ReceiptQuantity,
                                            nomStartDate = a.sqts.BeginingDateTime,
                                            nomEndDate = a.sqts.EndingDateTime,
                                            DelLoc = a.sqts.DeliveryLocation,
                                            RecLoc = a.sqts.ReceiptLocation,
                                            ContractSVC = a.sqts.ServiceRequestorContract,
                                            DwnIdentifier = a.sqts.DownstreamID,
                                            UpIdentifier = a.sqts.UpstreamId,
                                            StatementDate = a.sqts.StatementDate,
                                            ReductionReason = a.sqts.ReductionReason,
                                            Cycle = a.sqts.CycleIndicator,
                                            PkgId = a.sqts.PackageId,
                                            NomTrackingId = a.sqts.NomTrackingId,
                                            Username = a.shipper.FirstName + " " + a.shipper.LastName,
                                            ReductionReasonDetail = a.sqts.ReductionReasonDescription
                                        }).OrderByDescending(a => a.StatementDate).ToList();
                        summaryDTOList = sqtsList;

                    }
                    else if (pathType == "D")
                    {
                        var sqtsList = (from sqts in DbContext.SQTSPerTransaction
                                        join nom in DbContext.V4_Nomination on sqts.NomTrackingId equals nom.NominatorTrackingId
                                        join batch in DbContext.V4_Batch on nom.TransactionID equals batch.TransactionID
                                        join shipper in DbContext.Shipper on batch.CreatedBy equals shipper.UserId
                                        where !string.IsNullOrEmpty(sqts.DeliveryLocation)
                                        && string.IsNullOrEmpty(sqts.ReceiptLocation)
                                        && !string.IsNullOrEmpty(sqts.ServiceRequestorContract)
                                        && sqts.ServiceRequestor == shipperCompanyDuns
                                        && batch.CreatedBy == UserId
                                        && sqts.ModelType == "N"
                                        && !sqts.NomTrackingId.Contains("N/A")
                                        && sqts.TSPCode == pipeDuns
                                        && (sqts.ReceiptQuantity != 0 || sqts.DeliveryQuantity != 0)
                                        && sqts.BeginingDateTime.Month == month
                                        && sqts.BeginingDateTime.Year == currentYear
                                        select new { sqts, shipper }).Select(a => new SummaryDTO
                                        {
                                            DelPointQty = a.sqts.DeliveryQuantity,
                                            RecPointQty = a.sqts.ReceiptQuantity,
                                            nomStartDate = a.sqts.BeginingDateTime,
                                            nomEndDate = a.sqts.EndingDateTime,
                                            DelLoc = a.sqts.DeliveryLocation,
                                            RecLoc = a.sqts.ReceiptLocation,
                                            ContractSVC = a.sqts.ServiceRequestorContract,
                                            DwnIdentifier = a.sqts.DownstreamID,
                                            UpIdentifier = a.sqts.UpstreamId,
                                            StatementDate = a.sqts.StatementDate,
                                            ReductionReason = a.sqts.ReductionReason,
                                            Cycle = a.sqts.CycleIndicator,
                                            PkgId = a.sqts.PackageId,
                                            NomTrackingId = a.sqts.NomTrackingId,
                                            Username = a.shipper.FirstName + " " + a.shipper.LastName,
                                            ReductionReasonDetail = a.sqts.ReductionReasonDescription
                                        }).OrderByDescending(a => a.StatementDate).ToList();
                        summaryDTOList = sqtsList;
                    }
                }
            }
            return summaryDTOList;
        }

        //public List<SQTSPerTransactionDTO> GetSqtsDataForExcel(string pipelineDuns, int month, string pathType, bool IsSqtsQty, string UserId, bool showZero)
        //{
        //    try
        //    {
        //        int currentYear = DateTime.Now.Year;
        //        List<SQTSPerTransactionDTO> list = new List<SQTSPerTransactionDTO>();
        //        if (IsSqtsQty)
        //        {
        //            if(pathType=="P" || pathType == "T")
        //            {
        //                var data = (from sqts in DbContext.SQTSPerTransaction
        //                            where sqts.BeginingDateTime.Month == month
        //                            && sqts.BeginingDateTime.Year == currentYear
        //                            && sqts.TSPCode == pipelineDuns
        //                            && sqts.ModelType == pathType
        //                            && !sqts.NomTrackingId.Contains("N/A")
        //                            select new SQTSPerTransactionDTO
        //                            {
        //                                //TranasactionId=sqts.TranasactionId,
        //                                AssociatedContract = sqts.AssociatedContract,
        //                                BeginingDateTime = sqts.BeginingDateTime,
        //                                ServiceProviderActivityCode = sqts.ServiceProviderActivityCode,
        //                                BidTransportationRate = sqts.BidTransportationRate,
        //                                CapacityTypeIndicator = sqts.CapacityTypeIndicator,
        //                                CycleIndicator = sqts.CycleIndicator,
        //                                DealType = sqts.DealType,
        //                                DeliveryLocation = sqts.DeliveryLocation,
        //                                DeliveryQuantity = sqts.DeliveryQuantity,
        //                                DeliveryRank = sqts.DeliveryRank,
        //                                DownstreamContractIdentifier = sqts.DownstreamContractIdentifier,
        //                                DownstreamID = sqts.DownstreamID,
        //                                DownstreamPackageId = sqts.DownstreamPackageId,
        //                                EndingDateTime = sqts.EndingDateTime,
        //                                ExportDecleration = sqts.ExportDecleration,
        //                                FuelQuantity = sqts.FuelQuantity,
        //                                ServiceRequestor = sqts.ServiceRequestor,
        //                                //Id=sqts.Id,
        //                                ModelType = sqts.ModelType,
        //                                NominationUserData1 = sqts.NominationUserData1,
        //                                NominationUserData2 = sqts.NominationUserData2,
        //                                NomSubsequentCycleIndicator = sqts.NomSubsequentCycleIndicator,
        //                                NomTrackingId = sqts.NomTrackingId,
        //                                PackageId = sqts.PackageId,
        //                                pipelineId = sqts.pipelineId,
        //                                ProcessingRightsIndicator = sqts.ProcessingRightsIndicator,
        //                                ReceiptLocation = sqts.ReceiptLocation,
        //                                ReceiptQuantity = sqts.ReceiptQuantity,
        //                                ReceiptRank = sqts.ReceiptRank,
        //                                ReductionReason = sqts.ReductionReason,
        //                                Route = sqts.Route,
        //                                ServiceRequestorContract = sqts.ServiceRequestorContract,
        //                                StatementDate = sqts.StatementDate,
        //                                TransactionType = sqts.TransactionType,
        //                                TSPCode = sqts.TSPCode,
        //                                UpstreamContractIdentifier = sqts.UpstreamContractIdentifier,
        //                                UpstreamId = sqts.UpstreamId,
        //                                UpstreamPackageId = sqts.UpstreamPackageId
        //                            }).ToList();
        //                list = data;
        //            }
        //            else if (pathType == "R")
        //            {
        //                var data = (from sqts in DbContext.SQTSPerTransaction
        //                            where sqts.BeginingDateTime.Month == month
        //                            && sqts.BeginingDateTime.Year == currentYear
        //                            && sqts.TSPCode == pipelineDuns
        //                            && sqts.ModelType == "N"
        //                            && !string.IsNullOrEmpty(sqts.ReceiptLocation)
        //                            && string.IsNullOrEmpty(sqts.DeliveryLocation)
        //                            && !sqts.NomTrackingId.Contains("N/A")
        //                            select new SQTSPerTransactionDTO
        //                            {
        //                                //TranasactionId=sqts.TranasactionId,
        //                                AssociatedContract = sqts.AssociatedContract,
        //                                BeginingDateTime = sqts.BeginingDateTime,
        //                                ServiceProviderActivityCode = sqts.ServiceProviderActivityCode,
        //                                BidTransportationRate = sqts.BidTransportationRate,
        //                                CapacityTypeIndicator = sqts.CapacityTypeIndicator,
        //                                CycleIndicator = sqts.CycleIndicator,
        //                                DealType = sqts.DealType,
        //                                DeliveryLocation = sqts.DeliveryLocation,
        //                                DeliveryQuantity = sqts.DeliveryQuantity,
        //                                DeliveryRank = sqts.DeliveryRank,
        //                                DownstreamContractIdentifier = sqts.DownstreamContractIdentifier,
        //                                DownstreamID = sqts.DownstreamID,
        //                                DownstreamPackageId = sqts.DownstreamPackageId,
        //                                EndingDateTime = sqts.EndingDateTime,
        //                                ExportDecleration = sqts.ExportDecleration,
        //                                FuelQuantity = sqts.FuelQuantity,
        //                                ServiceRequestor = sqts.ServiceRequestor,
        //                                //Id=sqts.Id,
        //                                ModelType = sqts.ModelType,
        //                                NominationUserData1 = sqts.NominationUserData1,
        //                                NominationUserData2 = sqts.NominationUserData2,
        //                                NomSubsequentCycleIndicator = sqts.NomSubsequentCycleIndicator,
        //                                NomTrackingId = sqts.NomTrackingId,
        //                                PackageId = sqts.PackageId,
        //                                pipelineId = sqts.pipelineId,
        //                                ProcessingRightsIndicator = sqts.ProcessingRightsIndicator,
        //                                ReceiptLocation = sqts.ReceiptLocation,
        //                                ReceiptQuantity = sqts.ReceiptQuantity,
        //                                ReceiptRank = sqts.ReceiptRank,
        //                                ReductionReason = sqts.ReductionReason,
        //                                Route = sqts.Route,
        //                                ServiceRequestorContract = sqts.ServiceRequestorContract,
        //                                StatementDate = sqts.StatementDate,
        //                                TransactionType = sqts.TransactionType,
        //                                TSPCode = sqts.TSPCode,
        //                                UpstreamContractIdentifier = sqts.UpstreamContractIdentifier,
        //                                UpstreamId = sqts.UpstreamId,
        //                                UpstreamPackageId = sqts.UpstreamPackageId
        //                            }).ToList();
        //                list = data;
        //            }
        //            else if (pathType == "D")
        //            {
        //                var data = (from sqts in DbContext.SQTSPerTransaction
        //                            where sqts.BeginingDateTime.Month == month
        //                            && sqts.BeginingDateTime.Year == currentYear
        //                            && sqts.TSPCode == pipelineDuns
        //                            && sqts.ModelType == "U"
        //                            && string.IsNullOrEmpty(sqts.ReceiptLocation)
        //                            && !string.IsNullOrEmpty(sqts.DeliveryLocation)
        //                            && !sqts.NomTrackingId.Contains("N/A")
        //                            select new SQTSPerTransactionDTO
        //                            {
        //                                //TranasactionId=sqts.TranasactionId,
        //                                AssociatedContract = sqts.AssociatedContract,
        //                                BeginingDateTime = sqts.BeginingDateTime,
        //                                ServiceProviderActivityCode = sqts.ServiceProviderActivityCode,
        //                                BidTransportationRate = sqts.BidTransportationRate,
        //                                CapacityTypeIndicator = sqts.CapacityTypeIndicator,
        //                                CycleIndicator = sqts.CycleIndicator,
        //                                DealType = sqts.DealType,
        //                                DeliveryLocation = sqts.DeliveryLocation,
        //                                DeliveryQuantity = sqts.DeliveryQuantity,
        //                                DeliveryRank = sqts.DeliveryRank,
        //                                DownstreamContractIdentifier = sqts.DownstreamContractIdentifier,
        //                                DownstreamID = sqts.DownstreamID,
        //                                DownstreamPackageId = sqts.DownstreamPackageId,
        //                                EndingDateTime = sqts.EndingDateTime,
        //                                ExportDecleration = sqts.ExportDecleration,
        //                                FuelQuantity = sqts.FuelQuantity,
        //                                ServiceRequestor = sqts.ServiceRequestor,
        //                                //Id=sqts.Id,
        //                                ModelType = sqts.ModelType,
        //                                NominationUserData1 = sqts.NominationUserData1,
        //                                NominationUserData2 = sqts.NominationUserData2,
        //                                NomSubsequentCycleIndicator = sqts.NomSubsequentCycleIndicator,
        //                                NomTrackingId = sqts.NomTrackingId,
        //                                PackageId = sqts.PackageId,
        //                                pipelineId = sqts.pipelineId,
        //                                ProcessingRightsIndicator = sqts.ProcessingRightsIndicator,
        //                                ReceiptLocation = sqts.ReceiptLocation,
        //                                ReceiptQuantity = sqts.ReceiptQuantity,
        //                                ReceiptRank = sqts.ReceiptRank,
        //                                ReductionReason = sqts.ReductionReason,
        //                                Route = sqts.Route,
        //                                ServiceRequestorContract = sqts.ServiceRequestorContract,
        //                                StatementDate = sqts.StatementDate,
        //                                TransactionType = sqts.TransactionType,
        //                                TSPCode = sqts.TSPCode,
        //                                UpstreamContractIdentifier = sqts.UpstreamContractIdentifier,
        //                                UpstreamId = sqts.UpstreamId,
        //                                UpstreamPackageId = sqts.UpstreamPackageId
        //                            }).ToList();
        //                list = data;

        //            }
        //        }
        //        else
        //        {
        //            if (pathType == "P" || pathType == "T")
        //            {
        //                var data = (from sqts in DbContext.SQTSPerTransaction
        //                            where sqts.BeginingDateTime.Month == month
        //                            && sqts.BeginingDateTime.Year == currentYear
        //                            && sqts.TSPCode == pipelineDuns
        //                            && sqts.ModelType == pathType
        //                            && sqts.NomTrackingId.Contains("N/A")
        //                            select new SQTSPerTransactionDTO
        //                            {
        //                                //TranasactionId=sqts.TranasactionId,
        //                                AssociatedContract = sqts.AssociatedContract,
        //                                BeginingDateTime = sqts.BeginingDateTime,
        //                                ServiceProviderActivityCode = sqts.ServiceProviderActivityCode,
        //                                BidTransportationRate = sqts.BidTransportationRate,
        //                                CapacityTypeIndicator = sqts.CapacityTypeIndicator,
        //                                CycleIndicator = sqts.CycleIndicator,
        //                                DealType = sqts.DealType,
        //                                DeliveryLocation = sqts.DeliveryLocation,
        //                                DeliveryQuantity = sqts.DeliveryQuantity,
        //                                DeliveryRank = sqts.DeliveryRank,
        //                                DownstreamContractIdentifier = sqts.DownstreamContractIdentifier,
        //                                DownstreamID = sqts.DownstreamID,
        //                                DownstreamPackageId = sqts.DownstreamPackageId,
        //                                EndingDateTime = sqts.EndingDateTime,
        //                                ExportDecleration = sqts.ExportDecleration,
        //                                FuelQuantity = sqts.FuelQuantity,
        //                                ServiceRequestor = sqts.ServiceRequestor,
        //                                //Id=sqts.Id,
        //                                ModelType = sqts.ModelType,
        //                                NominationUserData1 = sqts.NominationUserData1,
        //                                NominationUserData2 = sqts.NominationUserData2,
        //                                NomSubsequentCycleIndicator = sqts.NomSubsequentCycleIndicator,
        //                                NomTrackingId = sqts.NomTrackingId,
        //                                PackageId = sqts.PackageId,
        //                                pipelineId = sqts.pipelineId,
        //                                ProcessingRightsIndicator = sqts.ProcessingRightsIndicator,
        //                                ReceiptLocation = sqts.ReceiptLocation,
        //                                ReceiptQuantity = sqts.ReceiptQuantity,
        //                                ReceiptRank = sqts.ReceiptRank,
        //                                ReductionReason = sqts.ReductionReason,
        //                                Route = sqts.Route,
        //                                ServiceRequestorContract = sqts.ServiceRequestorContract,
        //                                StatementDate = sqts.StatementDate,
        //                                TransactionType = sqts.TransactionType,
        //                                TSPCode = sqts.TSPCode,
        //                                UpstreamContractIdentifier = sqts.UpstreamContractIdentifier,
        //                                UpstreamId = sqts.UpstreamId,
        //                                UpstreamPackageId = sqts.UpstreamPackageId
        //                            }).ToList();
        //                list = data;
        //            }
        //            else if (pathType == "R")
        //            {
        //                var data = (from sqts in DbContext.SQTSPerTransaction
        //                            where sqts.BeginingDateTime.Month == month
        //                            && sqts.BeginingDateTime.Year == currentYear
        //                            && sqts.TSPCode == pipelineDuns
        //                            && sqts.ModelType == "U"
        //                            && !string.IsNullOrEmpty(sqts.ReceiptLocation)
        //                            && string.IsNullOrEmpty(sqts.DeliveryLocation)
        //                            && sqts.NomTrackingId.Contains("N/A")
        //                            select new SQTSPerTransactionDTO
        //                            {
        //                                //TranasactionId=sqts.TranasactionId,
        //                                AssociatedContract = sqts.AssociatedContract,
        //                                BeginingDateTime = sqts.BeginingDateTime,
        //                                ServiceProviderActivityCode = sqts.ServiceProviderActivityCode,
        //                                BidTransportationRate = sqts.BidTransportationRate,
        //                                CapacityTypeIndicator = sqts.CapacityTypeIndicator,
        //                                CycleIndicator = sqts.CycleIndicator,
        //                                DealType = sqts.DealType,
        //                                DeliveryLocation = sqts.DeliveryLocation,
        //                                DeliveryQuantity = sqts.DeliveryQuantity,
        //                                DeliveryRank = sqts.DeliveryRank,
        //                                DownstreamContractIdentifier = sqts.DownstreamContractIdentifier,
        //                                DownstreamID = sqts.DownstreamID,
        //                                DownstreamPackageId = sqts.DownstreamPackageId,
        //                                EndingDateTime = sqts.EndingDateTime,
        //                                ExportDecleration = sqts.ExportDecleration,
        //                                FuelQuantity = sqts.FuelQuantity,
        //                                ServiceRequestor = sqts.ServiceRequestor,
        //                                //Id=sqts.Id,
        //                                ModelType = sqts.ModelType,
        //                                NominationUserData1 = sqts.NominationUserData1,
        //                                NominationUserData2 = sqts.NominationUserData2,
        //                                NomSubsequentCycleIndicator = sqts.NomSubsequentCycleIndicator,
        //                                NomTrackingId = sqts.NomTrackingId,
        //                                PackageId = sqts.PackageId,
        //                                pipelineId = sqts.pipelineId,
        //                                ProcessingRightsIndicator = sqts.ProcessingRightsIndicator,
        //                                ReceiptLocation = sqts.ReceiptLocation,
        //                                ReceiptQuantity = sqts.ReceiptQuantity,
        //                                ReceiptRank = sqts.ReceiptRank,
        //                                ReductionReason = sqts.ReductionReason,
        //                                Route = sqts.Route,
        //                                ServiceRequestorContract = sqts.ServiceRequestorContract,
        //                                StatementDate = sqts.StatementDate,
        //                                TransactionType = sqts.TransactionType,
        //                                TSPCode = sqts.TSPCode,
        //                                UpstreamContractIdentifier = sqts.UpstreamContractIdentifier,
        //                                UpstreamId = sqts.UpstreamId,
        //                                UpstreamPackageId = sqts.UpstreamPackageId
        //                            }).ToList();
        //                list = data;
        //            }
        //            else if (pathType == "D")
        //            {
        //                var data = (from sqts in DbContext.SQTSPerTransaction
        //                            where sqts.BeginingDateTime.Month == month
        //                            && sqts.BeginingDateTime.Year == currentYear
        //                            && sqts.TSPCode == pipelineDuns
        //                            && sqts.ModelType == "U"
        //                            && string.IsNullOrEmpty(sqts.ReceiptLocation)
        //                            && !string.IsNullOrEmpty(sqts.DeliveryLocation)
        //                            && sqts.NomTrackingId.Contains("N/A")
        //                            select new SQTSPerTransactionDTO
        //                            {
        //                                //TranasactionId=sqts.TranasactionId,
        //                                AssociatedContract = sqts.AssociatedContract,
        //                                BeginingDateTime = sqts.BeginingDateTime,
        //                                ServiceProviderActivityCode = sqts.ServiceProviderActivityCode,
        //                                BidTransportationRate = sqts.BidTransportationRate,
        //                                CapacityTypeIndicator = sqts.CapacityTypeIndicator,
        //                                CycleIndicator = sqts.CycleIndicator,
        //                                DealType = sqts.DealType,
        //                                DeliveryLocation = sqts.DeliveryLocation,
        //                                DeliveryQuantity = sqts.DeliveryQuantity,
        //                                DeliveryRank = sqts.DeliveryRank,
        //                                DownstreamContractIdentifier = sqts.DownstreamContractIdentifier,
        //                                DownstreamID = sqts.DownstreamID,
        //                                DownstreamPackageId = sqts.DownstreamPackageId,
        //                                EndingDateTime = sqts.EndingDateTime,
        //                                ExportDecleration = sqts.ExportDecleration,
        //                                FuelQuantity = sqts.FuelQuantity,
        //                                ServiceRequestor = sqts.ServiceRequestor,
        //                                //Id=sqts.Id,
        //                                ModelType = sqts.ModelType,
        //                                NominationUserData1 = sqts.NominationUserData1,
        //                                NominationUserData2 = sqts.NominationUserData2,
        //                                NomSubsequentCycleIndicator = sqts.NomSubsequentCycleIndicator,
        //                                NomTrackingId = sqts.NomTrackingId,
        //                                PackageId = sqts.PackageId,
        //                                pipelineId = sqts.pipelineId,
        //                                ProcessingRightsIndicator = sqts.ProcessingRightsIndicator,
        //                                ReceiptLocation = sqts.ReceiptLocation,
        //                                ReceiptQuantity = sqts.ReceiptQuantity,
        //                                ReceiptRank = sqts.ReceiptRank,
        //                                ReductionReason = sqts.ReductionReason,
        //                                Route = sqts.Route,
        //                                ServiceRequestorContract = sqts.ServiceRequestorContract,
        //                                StatementDate = sqts.StatementDate,
        //                                TransactionType = sqts.TransactionType,
        //                                TSPCode = sqts.TSPCode,
        //                                UpstreamContractIdentifier = sqts.UpstreamContractIdentifier,
        //                                UpstreamId = sqts.UpstreamId,
        //                                UpstreamPackageId = sqts.UpstreamPackageId
        //                            }).ToList();
        //                list = data;
        //            }

        //        }
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //}
    }
    #region ISQTSRepository
    public interface ISQTSRepository : IRepository<SQTSPerTransaction>
    {
        List<SqtsDTO> GetSQTSForNom(string NomTrackingID);
        List<SummaryDTO> GetSqtsOrphanData(int month, string pipeDuns, string pathType, string shipperCompanyDuns, bool showZero, int pageSize = 15, int pageNo = 1);
        List<SummaryDTO> GetSqtsData(int month, string pipeDuns, string UserId, string pathType, string shipperCompanyDuns, bool showZero, int pageSize = 15, int pageNo = 1);
        List<SummaryDTO> GetSqtsOrphanDataForExcel(int month, string pipeDuns, string pathType, string shipperCompanyDuns, bool showZero);
        List<SummaryDTO> GetSqtsDataForExcel(int month, string pipeDuns, string UserId, string pathType, string shipperCompanyDuns, bool showZero);
        //List<SQTSPerTransactionDTO> GetSqtsDataForExcel(string pipelineDuns, int month, string pathType, bool IsSqtsQty, string UserId, bool showZero);
    }
    #endregion

}
