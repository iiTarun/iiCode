
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CentralisedUprd.Api.Models;
using CentralisedUprd.Api.UPRD.DTO;
using Nom1Done.Data.Repositories;

namespace CentralisedUprd.Api.Repositories
{
    public class ModalFactory
    {
        PipelineRepository pipelineRepository = new PipelineRepository();
        public UnscPerTransactionDTO Parse(UnscPerTransaction item)
        {
            UnscPerTransactionDTO viewItem = new UnscPerTransactionDTO();
             viewItem.CreatedDate = item.CreatedDate;
             viewItem.EffectiveGasDay = item.EffectiveGasDayTime.GetValueOrDefault();
             viewItem.EndingEffectiveDay = item.EndingEffectiveDay.GetValueOrDefault();
             viewItem.Loc = item.Loc;
             viewItem.LocName = item.LocName;
             viewItem.LocPurpDesc = item.LocPurpDesc;
             viewItem.LocQTIDesc = item.LocQTIDesc;
             viewItem.LocZn = item.LocZn;
             viewItem.MeasBasisDesc = item.MeasBasisDesc;
             switch (item.MeasBasisDesc)
             {
                 case "BZ":
                     viewItem.MeasBasisDesc = "MMBtu";
                     break;
                 case "G8":
                     viewItem.MeasBasisDesc = "Gigacalories";
                     break;
                 case "GV":
                     viewItem.MeasBasisDesc = "Gigajoules";
                     break;
                 case "TZ":
                     viewItem.MeasBasisDesc = "MCF";
                     break;
             }
            
             var pipeline = pipelineRepository.GetPipelineByDuns(item.TransactionServiceProvider);
             viewItem.PipelineID = pipeline.ID;
             viewItem.PipelineNameDuns = pipeline.Name + " (" + pipeline.DUNSNo + ")";
             viewItem.PostingDate = item.PostingDateTime.GetValueOrDefault();
             viewItem.ReceiveFileID = item.ReceiveFileID;
            viewItem.TotalDesignCapacity = item.TotalDesignCapacity; 
            viewItem.TransactionID = item.TransactionID;
             viewItem.TransactionServiceProvider = item.TransactionServiceProvider;
             viewItem.TransactionServiceProviderPropCode = item.TransactionServiceProviderPropCode;
             viewItem.UnscID = item.UnscID;
             viewItem.UnsubscribeCapacity = item.UnsubscribeCapacity;
             viewItem.ChangePercentage = Convert.ToInt32(item.ChangePercentage);
            return viewItem;
           
            
        }
        public UnscPerTransactionDTO ParseDTO(UnscPerTransactionDTO item)
        {
            UnscPerTransactionDTO viewItem = new UnscPerTransactionDTO();
            viewItem.CreatedDate = item.CreatedDate;
            viewItem.EffectiveGasDay = item.EffectiveGasDay.GetValueOrDefault();
            viewItem.EndingEffectiveDay = item.EndingEffectiveDay.GetValueOrDefault();
            viewItem.Loc = item.Loc;
            viewItem.LocName = item.LocName;
            viewItem.LocPurpDesc = item.LocPurpDesc;
            viewItem.LocQTIDesc = item.LocQTIDesc;
            viewItem.LocZn = item.LocZn;
            viewItem.MeasBasisDesc = item.MeasBasisDesc;
            switch (item.MeasBasisDesc)
            {
                case "BZ":
                    viewItem.MeasBasisDesc = "MMBtu";
                    break;
                case "G8":
                    viewItem.MeasBasisDesc = "Gigacalories";
                    break;
                case "GV":
                    viewItem.MeasBasisDesc = "Gigajoules";
                    break;
                case "TZ":
                    viewItem.MeasBasisDesc = "MCF";
                    break;
            }

            var pipeline = pipelineRepository.GetPipelineByDuns(item.TransactionServiceProvider);
            viewItem.PipelineID = pipeline.ID;
            viewItem.PipelineNameDuns = pipeline.Name + " (" + pipeline.DUNSNo + ")";
            viewItem.PostingDate = item.PostingDate.GetValueOrDefault();
            viewItem.ReceiveFileID = item.ReceiveFileID;
            viewItem.TotalDesignCapacity = item.TotalDesignCapacity;
            viewItem.TransactionID = item.TransactionID;
            viewItem.TransactionServiceProvider = item.TransactionServiceProvider;
            viewItem.TransactionServiceProviderPropCode = item.TransactionServiceProviderPropCode;
            viewItem.UnscID = item.UnscID;
            viewItem.UnsubscribeCapacity = item.UnsubscribeCapacity;
            viewItem.ChangePercentage = Convert.ToInt32(item.ChangePercentage);
            return viewItem;


        }
        public OACYPerTransactionDTO Parse(OACYPerTransaction item)
        {
            OACYPerTransactionDTO viewItem = new OACYPerTransactionDTO();
             viewItem.AllQtyAvailableIndicator = item.AllQtyAvailableIndicator;
             viewItem.CreatedDate = item.CreatedDate;
             viewItem.DesignCapacity = item.DesignCapacity;
             viewItem.EffectiveGasDay = item.EffectiveGasDayTime.GetValueOrDefault();
             viewItem.FlowIndicator = item.FlowIndicator;
             viewItem.ITIndicator = item.ITIndicator;
             viewItem.Loc = item.Loc;
             viewItem.LocName = item.LocName;
             viewItem.LocPropDesc = item.LocPropDesc;
             viewItem.LocQTIDesc = item.LocQTIDesc;
             viewItem.LocZn = item.LocZn;
             viewItem.MeasurementBasis = item.MeasurementBasis;
             switch (item.MeasurementBasis)
             {
                 case "BZ":
                     viewItem.MeasurementBasis = "MMBtu";
                     break;
                 case "G8":
                     viewItem.MeasurementBasis = "Gigacalories";
                     break;
                 case "GV":
                     viewItem.MeasurementBasis = "Gigajoules";
                     break;
                 case "TZ":
                     viewItem.MeasurementBasis = "MCF";
                     break;
             }
            
             viewItem.OACYID = item.OACYID;
            viewItem.OperatingCapacity = item.OperatingCapacity; //String.Format(CultureInfo.InvariantCulture, "{0:0,0}", item.OperatingCapacity);
            viewItem.OperationallyAvailableQty = item.OperationallyAvailableQty;


             var pipeline = pipelineRepository.GetPipelineByDuns(item.TransactionServiceProvider);
             viewItem.PipelineID = pipeline.ID;
             viewItem.PipelineNameDuns = pipeline.Name + " (" + pipeline.DUNSNo + ")";
             viewItem.PostingDate = item.PostingDateTime.GetValueOrDefault();
             viewItem.ReceiceFileID = item.ReceiceFileID;
             viewItem.TotalScheduleQty = item.TotalScheduleQty;
             viewItem.CycleIndicator = item.CycleIndicator;
             viewItem.TransactionID = item.TransactionID;
             viewItem.TransactionServiceProvider = item.TransactionServiceProvider;
             viewItem.TransactionServiceProviderPropCode = item.TransactionServiceProviderPropCode;
             viewItem.AvailablePercentage = Convert.ToInt32(item.AvailablePercentage);
             return viewItem;
        }

        public OACYPerTransactionDTO ParseDTO(OACYPerTransactionDTO item)
        {
            OACYPerTransactionDTO viewItem = new OACYPerTransactionDTO();
            viewItem.AllQtyAvailableIndicator = item.AllQtyAvailableIndicator;
            viewItem.CreatedDate = item.CreatedDate;
            viewItem.DesignCapacity = item.DesignCapacity;
            viewItem.EffectiveGasDay = item.EffectiveGasDay.GetValueOrDefault();
            viewItem.FlowIndicator = item.FlowIndicator;
            viewItem.ITIndicator = item.ITIndicator;
            viewItem.Loc = item.Loc;
            viewItem.LocName = item.LocName;
            viewItem.LocPropDesc = item.LocPropDesc;
            viewItem.LocQTIDesc = item.LocQTIDesc;
            viewItem.LocZn = item.LocZn;
            viewItem.MeasurementBasis = item.MeasurementBasis;
            switch (item.MeasurementBasis)
            {
                case "BZ":
                    viewItem.MeasurementBasis = "MMBtu";
                    break;
                case "G8":
                    viewItem.MeasurementBasis = "Gigacalories";
                    break;
                case "GV":
                    viewItem.MeasurementBasis = "Gigajoules";
                    break;
                case "TZ":
                    viewItem.MeasurementBasis = "MCF";
                    break;
            }

            viewItem.OACYID = item.OACYID;
            viewItem.OperatingCapacity = item.OperatingCapacity; //String.Format(CultureInfo.InvariantCulture, "{0:0,0}", item.OperatingCapacity);
            viewItem.OperationallyAvailableQty = item.OperationallyAvailableQty;


            var pipeline = pipelineRepository.GetPipelineByDuns(item.TransactionServiceProvider);
            viewItem.PipelineID = pipeline.ID;
            viewItem.PipelineNameDuns = pipeline.Name + " (" + pipeline.DUNSNo + ")";
            viewItem.PostingDate = item.PostingDate.GetValueOrDefault();
            viewItem.ReceiceFileID = item.ReceiceFileID;
            viewItem.TotalScheduleQty = item.TotalScheduleQty;
            viewItem.CycleIndicator = item.CycleIndicator;
            viewItem.TransactionID = item.TransactionID;
            viewItem.TransactionServiceProvider = item.TransactionServiceProvider;
            viewItem.TransactionServiceProviderPropCode = item.TransactionServiceProviderPropCode;
            viewItem.AvailablePercentage = Convert.ToInt32(item.AvailablePercentage);
            return viewItem;
        }


        public SwntPerTransactionDTO Parse(SwntPerTransaction model)
        {
            var pipeline = pipelineRepository.GetPipelineByDuns(model.TransportationserviceProvider);
            
            return new SwntPerTransactionDTO()
            {
                Id = model.Id,
                TransactionId = model.TransactionId,
                ReceiveFileId = model.ReceiveFileId,
                PipelineId = pipeline.ID,
                PipelineNameDuns = pipeline.Name + " (" + pipeline.DUNSNo + ")",
                TransactionIdentifierCode = model.TransactionIdentifierCode,
                TransactionControlNumber = model.TransactionControlNumber,
                TransactionSetPurposeCode = model.TransactionSetPurposeCode,
                Description = model.Description,
                NoticeEffectiveDateTime = model.NoticeEffectiveDateTime,
                NoticeEndDateTime = model.NoticeEndDateTime,
                PostingDateTime = model.PostingDateTime,
                ResponseDateTime = model.ResponseDateTime,
                TransportationserviceProvider = model.TransportationserviceProvider,
                TransportationServiceProviderPropCode = model.TransportationServiceProviderPropCode,
                CriticalNoticeIndicator = model.CriticalNoticeIndicator,
                FreeFormMessageText = model.FreeFormMessageText,
                CreatedDate = model.CreatedDate,
                IsActive = model.IsActive,
                Message = model.Message,
                NoticeId = model.NoticeId,
                NoticeTypeDesc1 = model.NoticeTypeDesc1,
                NoticeTypeDesc2 = model.NoticeTypeDesc2,
                ReqrdResp = model.ReqrdResp,
                Subject = GetSubjectUsingNoticeDetails(model.Subject, model.Message),
                PriorNotice = model.PriorNotice,
                NoticeStatusDesc = model.NoticeStatusDesc,
            
            };


        }

        public CounterPartiesDTO Parse(CounterParty item)
        {
            CounterPartiesDTO itemObj = new CounterPartiesDTO();
            itemObj.ID = item.ID;
            itemObj.Name = item.Name;
            itemObj.Identifier = item.Identifier;
            itemObj.PropCode = item.PropCode;
            itemObj.PipeDuns = item.PipeDuns;
            itemObj.IsActive = item.IsActive;
            itemObj.CreatedBy = item.CreatedBy;
            itemObj.CreatedDate = item.CreatedDate;
            itemObj.ModifiedBy = item.ModifiedBy;
            itemObj.ModifiedDate = item.ModifiedDate;
            return itemObj;
        }

        public LocationsDTO Parse(Location item)
        {
            LocationsDTO result = new LocationsDTO();
            result.CreatedBy = item.CreatedBy;
            result.ID = item.ID;
            result.CreatedDate = item.CreatedDate;
            result.Identifier = item.Identifier;
            result.IsActive = item.IsActive;
            result.ModifiedBy = item.ModifiedBy;
            result.ModifiedDate = item.ModifiedDate;
            result.Name = item.Name;
            result.PipelineID = item.PipelineID;
            result.PropCode = item.PropCode;
            result.RDUsageID = item.RDUsageID;

            if (result.RDUsageID == 1)
                result.RDB = "R";
            else if (result.RDUsageID == 2)
                result.RDB = "D";
            else
                result.RDB = "B";

            return result;
        }
        public string GetSubjectUsingNoticeDetails(string subject, string details)
        {
            string newSubject = string.Empty;
            if (string.IsNullOrEmpty(subject) || string.IsNullOrWhiteSpace(subject))
            {
                newSubject = new string(details.Take(25).ToArray());
                if (details.Length > 25) { newSubject += "..."; }
            }
            else
            {
                return subject;
            }
            return newSubject;
        }

    }
}
