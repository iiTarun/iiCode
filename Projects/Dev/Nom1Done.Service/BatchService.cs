using Nom1Done.Service.Interface;
using System;
using System.Linq;
using Nom.ViewModel;
using Nom1Done.Data.Repositories;
using Nom1Done.DTO;

namespace Nom1Done.Service
{
    public class BatchService : IBatchService
    {
        IBatchRepository batchRepository;
        IModalFactory modelFactory;
        PNTNominationService servicePNTNomnation;
        public BatchService(IBatchRepository batchRepository, IModalFactory modelFactory, PNTNominationService servicePNTNomnation)
        {
            this.batchRepository = batchRepository;
            this.modelFactory = modelFactory;
            this.servicePNTNomnation = servicePNTNomnation;
        }
        public BatchDTO GetBatch(Guid transactionId)
        {
            return modelFactory.Parse(batchRepository.GetByTransactionID(transactionId));
        }

        public bool ValidateNomination(Guid transactioId, string pipelineDuns)
        {
            var SONAT = "006900518";
            bool reqFields = true;
            bool pathComplete = true;
            BatchDetailDTO batchDetail = servicePNTNomnation.GetNomDetail(transactioId, pipelineDuns);//GetNominationDetailByBatchID(transactioId);
            if ((batchDetail.StatusId==11) && (batchDetail.MarketList != null && batchDetail.MarketList.Count > 0) && (batchDetail.SupplyList != null && batchDetail.SupplyList.Count > 0))
            {
                int marketRows, supplyRows, transportRows, transportPathRows;
                marketRows = batchDetail.MarketList.Count;
                supplyRows = batchDetail.SupplyList.Count;
                transportRows = batchDetail.Contract.Count;
                transportPathRows = batchDetail.ContractPath.Count;
                #region Mandatory fields validation
                #region Market
                do
                {
                    if (string.IsNullOrEmpty(batchDetail.MarketList[marketRows - 1].Location))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.MarketList[marketRows - 1].LocationProp))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.MarketList[marketRows - 1].DownstreamID))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.MarketList[marketRows - 1].DownstreamIDProp))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.MarketList[marketRows - 1].ServiceRequestNo))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.MarketList[marketRows - 1].TransactionType))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.MarketList[marketRows - 1].DnstreamRank))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.MarketList[marketRows - 1].ReceiptQuantityGross + ""))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.MarketList[marketRows - 1].DeliveryQuantityNet + ""))
                    {
                        reqFields = false;
                        break;
                    }
                    marketRows--;
                } while (marketRows > 0);
                #endregion
                #region Supply
                do
                {
                    if (string.IsNullOrEmpty(batchDetail.SupplyList[supplyRows - 1].Location))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.SupplyList[supplyRows - 1].LocationProp))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.SupplyList[supplyRows - 1].UpstreamID))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.SupplyList[supplyRows - 1].UpstreamIDProp))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.SupplyList[supplyRows - 1].ServiceRequestNo))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.SupplyList[supplyRows - 1].TransactionType))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.SupplyList[supplyRows - 1].UpstreamRank))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.SupplyList[supplyRows - 1].ReceiptQuantityGross + ""))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.SupplyList[supplyRows - 1].DeliveryQuantityNet + ""))
                    {
                        reqFields = false;
                        break;
                    }
                    supplyRows--;
                } while (supplyRows > 0);
                #endregion
                #region contract 
                if((batchDetail.ContractPath != null && batchDetail.ContractPath.Count > 0) && (batchDetail.Contract != null && batchDetail.Contract.Count > 0))
                {
                    #region Transport
                    do
                    {
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].RecLocation))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].RecLocationProp))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].DelLocation))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].DelLocationProp))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].ServiceRequestNo))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].TransactionType))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].RecRank))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].DelRank))
                        {
                            reqFields = false;
                            break;
                        }
                        //if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].PackageID))
                        //{
                        //    reqFields = false;
                        //    break;
                        //}
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].PathRank))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].ReceiptDth))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].DeliveryDth))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].Route) && batchDetail.Duns==SONAT)
                        {
                            reqFields = false;
                            break;
                        }
                        transportRows--;
                    } while (transportRows > 0);
                    #endregion
                    #region TransportPath
                    do
                    {
                        if (string.IsNullOrEmpty(batchDetail.ContractPath[transportPathRows - 1].ServiceRequestNo))
                        {
                            reqFields = false;
                            break;
                        }
                        //if (string.IsNullOrEmpty(batchDetail.ContractPath[transportPathRows - 1].FuelPercentage))
                        //{
                        //    reqFields = false;
                        //    break;
                        //}
                        transportPathRows--;
                    } while (transportPathRows > 0);
                    #endregion
                }
                #endregion
                #endregion
                #region Nom Matrix Path validation
                if (reqFields)
                {
                    if(batchDetail.Contract!=null && batchDetail.Contract.Count>0)
                        foreach (var item in batchDetail.Contract)
                        {
                            if (!batchDetail.SupplyList.Any(a => a.Location == item.RecLocation))
                            {
                                pathComplete = false;
                                break;
                            }
                            if (!batchDetail.MarketList.Any(a => a.Location == item.DelLocation))
                            {
                                pathComplete = false;
                                break;
                            }
                            if (!batchDetail.ContractPath.Any(a => a.ServiceRequestNo == item.ServiceRequestNo))
                            {
                                pathComplete = false;
                                break;
                            }
                        }
                    else
                        foreach(var supply in batchDetail.SupplyList)
                        {
                            if (!batchDetail.MarketList.Any(a => a.LocationProp == supply.LocationProp))
                            {
                                pathComplete = false;
                                break;
                            }
                        }
                }
                else
                    return false;
                #endregion
            }
            else
                return false;

            if (reqFields && pathComplete)
                return true;
            else
                return false;
        }
    }
}
