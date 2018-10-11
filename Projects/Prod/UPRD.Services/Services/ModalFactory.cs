using System;
using System.Collections.Generic;
using System.Linq;
using Nom.ViewModel;
using Nom1Done.DTO;
using System.Globalization;
using UPRD.Service.Interface;
using UPRD.Data.Repositories;
using UPRD.Model;

namespace UPRD.Service
{
    public class ModalFactory : IModalFactory
    {
        //private readonly IPipelineRepository pipelineRepository;
        //IPipelineEDISettingRepository pipelineEDISettingRepository;
        public ModalFactory(           
            //IPipelineRepository pipelineRepository,
            //IPipelineEDISettingRepository pipelineEDISettingRepository
            )
        {
            //this.pipelineRepository = pipelineRepository;
            //this.pipelineEDISettingRepository = pipelineEDISettingRepository;
        }
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
            viewItem.PipelineID = item.PipelineID;
            //var pipeline = pipelineRepository.GetById(item.PipelineID);
            //viewItem.PipelineNameDuns = pipeline.Name + " (" + pipeline.DUNSNo + ")";
            viewItem.PostingDate = item.PostingDateTime.Value;           
            viewItem.ReceiveFileID = item.ReceiveFileID;
            viewItem.TotalDesignCapacity = item.TotalDesignCapacity;//String.Format(CultureInfo.InvariantCulture, "{0:0,0}", item.TotalDesignCapacity );
            viewItem.TransactionID = item.TransactionID;
            viewItem.TransactionServiceProvider = item.TransactionServiceProvider;
            viewItem.TransactionServiceProviderPropCode = item.TransactionServiceProviderPropCode;
            viewItem.UnscID = item.UnscID;
            viewItem.UnsubscribeCapacity = item.UnsubscribeCapacity;//String.Format(CultureInfo.InvariantCulture, "{0:0,0}", item.UnsubscribeCapacity);         
            viewItem.ChangePercentage = Convert.ToInt32(item.ChangePercentage);
            return viewItem;
        }
        public OACYPerTransactionDTO Parse(OACYPerTransaction item)
        {
            OACYPerTransactionDTO viewItem = new OACYPerTransactionDTO();
            viewItem.AllQtyAvailableIndicator = item.AllQtyAvailableIndicator;
            viewItem.CreatedDate = item.CreatedDate;
            viewItem.DesignCapacity = item.DesignCapacity;//String.Format(CultureInfo.InvariantCulture, "{0:0,0}", item.DesignCapacity);
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
            viewItem.OperatingCapacity = item.OperatingCapacity;//String.Format(CultureInfo.InvariantCulture, "{0:0,0}", item.OperatingCapacity);
            viewItem.OperationallyAvailableQty = item.OperationallyAvailableQty;//String.Format(CultureInfo.InvariantCulture, "{0:0,0}", item.OperationallyAvailableQty);
            viewItem.PipelineID = item.PipelineID;
            //var pipeline = pipelineRepository.GetById(item.PipelineID.GetValueOrDefault());
            //viewItem.PipelineNameDuns = pipeline.Name+" ("+ pipeline.DUNSNo+")";
            //viewItem.PostingDate = item.PostingDateTime.GetValueOrDefault();           
            //viewItem.ReceiceFileID = item.ReceiceFileID;
            //viewItem.TotalScheduleQty= String.Format(CultureInfo.InvariantCulture, "{0:0,0}", item.TotalScheduleQty);
            //viewItem.CycleIndicator = item.CycleIndicator;
            //viewItem.TransactionID = item.TransactionID;
            //viewItem.TransactionServiceProvider = item.TransactionServiceProvider;
            //viewItem.TransactionServiceProviderPropCode = item.TransactionServiceProviderPropCode;
            //viewItem.AvailablePercentage = Convert.ToInt32(item.AvailablePercentage);
            return viewItem;
        }
        public SwntPerTransactionDTO Parse(SwntPerTransaction model)
        {
            return null;
            //var pipeline = pipelineRepository.GetById(model.PipelineId);

            //return new SwntPerTransactionDTO() {
            //    Id = model.Id,
            //    TransactionId = model.TransactionId,
            //    ReceiveFileId = model.ReceiveFileId,
            //    PipelineId = model.PipelineId,
            //    PipelineNameDuns = pipeline.Name + " (" + pipeline.DUNSNo + ")",
            //    TransactionIdentifierCode = model.TransactionIdentifierCode,
            //    TransactionControlNumber = model.TransactionControlNumber,
            //    TransactionSetPurposeCode = model.TransactionSetPurposeCode,
            //    Description = model.Description,
            //    NoticeEffectiveDateTime = model.NoticeEffectiveDateTime,
            //    NoticeEndDateTime = model.NoticeEndDateTime,
            //    PostingDateTime = model.PostingDateTime,
            //    ResponseDateTime = model.ResponseDateTime,
            //    TransportationserviceProvider = model.TransportationserviceProvider,
            //    TransportationServiceProviderPropCode = model.TransportationServiceProviderPropCode,
            //    CriticalNoticeIndicator = model.CriticalNoticeIndicator,
            //    FreeFormMessageText = model.FreeFormMessageText,
            //    CreatedDate = model.CreatedDate,
            //    IsActive = model.IsActive,
            //    Message = model.Message,
            //    NoticeId = model.NoticeId,
            //    NoticeTypeDesc1 = model.NoticeTypeDesc1,
            //    NoticeTypeDesc2 = model.NoticeTypeDesc2,
            //    ReqrdResp = model.ReqrdResp,
            //    Subject = GetSubjectUsingNoticeDetails(model.Subject, model.Message),
            //    PriorNotice = model.PriorNotice,
            //    NoticeStatusDesc = model.NoticeStatusDesc,

            //};
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
        public PipelineDTO Parse(Pipeline pipe)
        {
            PipelineDTO dto = new PipelineDTO();
            dto.CreatedBy = pipe.CreatedBy;
            dto.CreatedDate = pipe.CreatedDate;
            dto.DUNSNo = pipe.DUNSNo;
            dto.ID = pipe.ID;
            dto.IsActive = pipe.IsActive;
            dto.ModelTypeID = pipe.ModelTypeID;
            dto.ModifiedBy = pipe.ModifiedBy;
            dto.ModifiedDate = pipe.ModifiedDate;
            dto.Name = pipe.Name + " (" + pipe.DUNSNo + ")";
            dto.NameWithoutDuns = pipe.Name;
            dto.ToUseTSPDUNS = pipe.ToUseTSPDUNS;
            dto.TSPId = pipe.TSPId;
            dto.TempItem = pipe.ID + "-" + pipe.ModelTypeID;
            dto.IsUprdActive = pipe.IsUprdActive;
            return dto;
        }
        public SettingDTO Parse(Setting a)
        {
            SettingDTO s = new SettingDTO();
            s.ID = a.ID;
            s.Value = a.Value;
            s.IsActive = a.IsActive;
            s.CreatedBy = a.CreatedBy;
            s.CreatedDate = a.CreatedDate;
            s.ModifiedBy = a.ModifiedBy;
            s.ModifiedDate = a.ModifiedDate;
            s.Name = a.Name;
            return s;
        }
        public PipelineEDISettingDTO Parse(PipelineEDISetting pipeSetting)
        {
            if (pipeSetting != null)
                return new PipelineEDISettingDTO
                {
                    id = pipeSetting.id,
                    DataSeparator = pipeSetting.DataSeparator,
                    DatasetId = pipeSetting.DatasetId,
                    GS01_Segment = pipeSetting.GS01_Segment,
                    GS02_Segment = pipeSetting.GS02_Segment,
                    GS03_Segment = pipeSetting.GS03_Segment,
                    GS07_Segment = pipeSetting.GS07_Segment,
                    GS08_Segment = pipeSetting.GS08_Segment,
                    SegmentSeperator = pipeSetting.SegmentSeperator,
                    ShipperCompDuns = pipeSetting.ShipperCompDuns,
                    ST01_Segment = pipeSetting.ST01_Segment,
                    ISA06_Segment = pipeSetting.ISA06_Segment,
                    ISA08_segment = pipeSetting.ISA08_segment,
                    ISA11_Segment = pipeSetting.ISA11_Segment,
                    ISA12_Segment = pipeSetting.ISA12_Segment,
                    ISA16_Segment = pipeSetting.ISA16_Segment,
                    PipeDuns = pipeSetting.PipeDuns,
                    StartDate = pipeSetting.StartDate.HasValue ? pipeSetting.StartDate.Value : DateTime.MinValue,
                    EndDate = pipeSetting.EndDate.HasValue ? pipeSetting.EndDate.Value : DateTime.MinValue,
                    SendManually = pipeSetting.SendManually,
                    ForOacy = pipeSetting.ForOacy,
                    ForUnsc = pipeSetting.ForUnsc,
                    ForSwnt = pipeSetting.ForSwnt
                };
            else
                return null;
        }
        public PipelineEDISetting Create(PipelineEDISettingDTO a)
        {
            return new PipelineEDISetting
            {
                SendManually = a.SendManually,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                ForOacy = a.ForOacy,
                ForSwnt = a.ForSwnt,
                ForUnsc = a.ForUnsc,
                ShipperCompDuns=a.ShipperCompDuns
            };
        }
        public List<OACYPerTransaction> Create(List<EDIOacyWrapperDTO> oacyList)
        {
            List<OACYPerTransaction> oacylist = new List<OACYPerTransaction>();
            foreach(var a in oacyList)
            {
                oacylist.Add(Create(a));
            }
            return oacylist;
        }
        private OACYPerTransaction Create(EDIOacyWrapperDTO a)
        {
            OACYPerTransaction oacy = new OACYPerTransaction();
            oacy.AllQtyAvailableIndicator = a.AllQtyAvailableIndicator;
            oacy.AvailablePercentage = a.AvailablePercentage;
            oacy.CreatedDate = a.CreatedDate;
            oacy.CycleIndicator = a.CycleIndicator;
            oacy.DesignCapacity = a.DesignCapacity;
            oacy.EffectiveGasDayTime = a.EffectiveGasDayTime;
            oacy.FlowIndicator = a.FlowIndicator;
            oacy.ITIndicator = a.ITIndicator;
            oacy.Loc = a.Loc;
            oacy.LocName = a.LocName;
            oacy.LocPropDesc = a.LocPropDesc;
            oacy.LocQTIDesc = a.LocQTIDesc;
            oacy.LocZn = a.LocZn;
            oacy.MeasurementBasis = a.MeasurementBasis;
            oacy.OACYID = a.OACYID;
            oacy.OperatingCapacity = a.OperatingCapacity;
            oacy.OperationallyAvailableQty = a.OperationallyAvailableQty;
            oacy.PipelineID = a.PipelineID;
            oacy.PostingDateTime = a.PostingDateTime;
            oacy.ReceiceFileID = a.ReceiceFileID;
            oacy.TotalScheduleQty = a.TotalScheduleQty;
            oacy.TransactionID = a.TransactionID;
            oacy.TransactionServiceProvider = a.TransactionServiceProvider;
            oacy.TransactionServiceProviderPropCode = a.TransactionServiceProviderPropCode;
            return oacy;
        }
        public List<SwntPerTransaction> Create(List<SwntPerTransactionDTO> swntList)
        {
            List<SwntPerTransaction> swntlist = new List<SwntPerTransaction>();
            foreach(var a in swntList)
            {
                swntlist.Add(Create(a));
            }
            return swntlist;
        }
        private SwntPerTransaction Create(SwntPerTransactionDTO a)
        {
            SwntPerTransaction swnt = new SwntPerTransaction();
            swnt.CreatedDate = a.CreatedDate;
            swnt.CriticalNoticeIndicator = a.CriticalNoticeIndicator;
            swnt.Description = a.Description;
            swnt.FreeFormMessageText = a.FreeFormMessageText;
            swnt.Id = a.Id;
            swnt.IsActive = a.IsActive;
            swnt.Message = a.Message;
            swnt.NoticeEffectiveDateTime = a.NoticeEffectiveDateTime;
            swnt.NoticeEndDateTime = a.NoticeEndDateTime;
            swnt.NoticeId = a.NoticeId;
            swnt.NoticeStatusDesc = a.NoticeStatusDesc;
            swnt.NoticeTypeDesc1 = a.NoticeTypeDesc1;
            swnt.NoticeTypeDesc2 = a.NoticeTypeDesc2;
            swnt.PipelineId = a.PipelineId;
            swnt.PostingDateTime = a.PostingDateTime;
            swnt.PriorNotice = a.PriorNotice;
            swnt.ReceiveFileId = a.ReceiveFileId;
            swnt.ReqrdResp = a.ReqrdResp;
            swnt.ResponseDateTime = a.ResponseDateTime == DateTime.MinValue ? null : a.ResponseDateTime;
            swnt.Subject = a.Subject;
            swnt.TransactionControlNumber = a.TransactionControlNumber;
            swnt.TransactionId = a.TransactionId;
            swnt.TransactionIdentifierCode = a.TransactionIdentifierCode;
            swnt.TransactionSetPurposeCode = a.TransactionSetPurposeCode;
            swnt.TransportationserviceProvider = a.TransportationserviceProvider;
            swnt.TransportationServiceProviderPropCode = a.TransportationServiceProviderPropCode;
            return swnt;
        }
        public List<UnscPerTransaction> Create(List<EDIUnscWrapperDTO> unscList)
        {
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            foreach(var a in unscList)
            {
                unsclist.Add(Create(a));
            }
            return unsclist;
        }
        private UnscPerTransaction Create(EDIUnscWrapperDTO a)
        {
            UnscPerTransaction u = new UnscPerTransaction();            
            u.CreatedDate = a.CreatedDate;
            u.EffectiveGasDayTime = a.EffectiveGasDayTime;
            u.EndingEffectiveDay = a.EndingEffectiveDay;
            u.Loc = a.Loc;
            u.LocName = a.LocName;
            u.LocPurpDesc = a.LocPurpDesc;
            u.LocQTIDesc = a.LocQTIDesc;
            u.LocZn = a.LocZn;
            u.MeasBasisDesc = a.MeasBasisDesc;
            u.PipelineID = a.PipelineID;
            u.PostingDateTime = a.PostingDateTime;
            u.ReceiveFileID = a.ReceiveFileID;
            u.TotalDesignCapacity = a.TotalDesignCapacity;
            u.TransactionID = a.TransactionID;
            u.TransactionServiceProvider = a.TransactionServiceProvider;
            u.TransactionServiceProviderPropCode = a.TransactionServiceProviderPropCode;
            u.UnscID = a.UnscID;
            u.UnsubscribeCapacity = a.UnsubscribeCapacity;
            return u;
        }
        public List<UPRDStatus> Create(List<UPRDStatusDTO> uprdStatusList)
        {
            List<UPRDStatus> uprdstatuslist = new List<UPRDStatus>();
            foreach(var a in uprdStatusList)
            {
                uprdstatuslist.Add(Create(a));
            }
            return uprdstatuslist;
        }
        private UPRDStatus Create(UPRDStatusDTO a)
        {
            UPRDStatus u = new UPRDStatus();
            u.CreatedDate = a.CreatedDate;
            u.DatasetRequested = a.DatasetRequested;
            u.DatasetSummary = a.DatasetSummary;
            u.RequestID = a.RequestID;
            u.RURD_ID = a.RURD_ID;
            u.TransactionId = a.TransactionId;
            u.UPRD_ID = a.UPRD_ID;
            u.IsRURDReceived = a.IsRURDReceived;
            u.IsDataSetAvailable = a.IsDataSetAvailable;
            u.IsDatasetReceived = a.IsDatasetReceived;
            return u;
        }
        public List<UPRDStatusDTO> Parse(List<UPRDStatus> uprdStatusList)
        {
            List<UPRDStatusDTO> uprdDTOList=new List<UPRDStatusDTO>();
            foreach(var a in uprdStatusList)
            {
                uprdDTOList.Add(Parse(a));
            }
            return uprdDTOList;
        }
        private UPRDStatusDTO Parse(UPRDStatus a)
        {
            UPRDStatusDTO d = new UPRDStatusDTO();
            d.CreatedDate = a.CreatedDate;
            d.DatasetRequested = a.DatasetRequested;
            d.DatasetSummary = a.DatasetSummary;
            d.IsDataSetAvailable = a.IsDataSetAvailable;
            d.IsDatasetReceived = a.IsDatasetReceived;
            d.IsRURDReceived = a.IsRURDReceived;
            d.RequestID = a.RequestID;
            d.RURD_ID = a.RURD_ID;
            d.TransactionId = a.TransactionId;
            d.UPRD_ID = a.UPRD_ID;
            return d;
        }
    }
}
