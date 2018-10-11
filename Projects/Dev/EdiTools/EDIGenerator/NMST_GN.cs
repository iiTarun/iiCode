﻿using Nom.ViewModel;
using Nom1Done.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EdiTools.EDIGenerator
{
    public class NMST_GN
    {
        public EdiDocument GenerateEDIPNTNomination(BatchDTO batch, List<V4_NominationDTO> NomList, PipelineEDISettingDTO pipelineEdiSetting, bool IsTest)
        {
            string number = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);
            List<V4_NominationDTO> nomSupply = NomList.Where(a => a.PathType.Trim() == "S").ToList();
            List<V4_NominationDTO> nomMarket = NomList.Where(a => a.PathType.Trim() == "M").ToList();
            List<V4_NominationDTO> nomTransport = NomList.Where(a => a.PathType.Trim() == "T").ToList();
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            var ediDocument = new EdiDocument();
            var isa = new EdiSegment("ISA");
            isa[01] = "00";
            isa[02] = "".PadRight(10);
            isa[03] = "00";
            isa[04] = "".PadRight(10);
            isa[05] = "01";
            isa[06] = batch.ServiceRequester.Trim().PadRight(15);
            isa[07] = pipelineEdiSetting.ISA08_segment.Trim().All(char.IsDigit) == true ? "01" : "00";//"01";
            isa[08] = pipelineEdiSetting.ISA08_segment.Trim().PadRight(15);
            isa[09] = EdiValue.Date(6, DateTime.Now);
            isa[10] = EdiValue.Time(4, DateTime.Now);
            isa[11] = pipelineEdiSetting.ISA11_Segment.Trim();
            isa[12] = pipelineEdiSetting.ISA12_Segment.Trim();
            isa[13] = number;
            isa[14] = "0";
            isa[15] = IsTest ? "T" : "P";
            isa[16] = pipelineEdiSetting.ISA16_Segment.Trim();
            ediDocument.Segments.Add(isa);

            var gs = new EdiSegment("GS");
            gs[01] = pipelineEdiSetting.GS01_Segment.Trim();
            gs[02] = pipelineEdiSetting.GS02_Segment.Trim();
            gs[03] = pipelineEdiSetting.GS03_Segment.Trim();
            gs[04] = EdiValue.Date(6, DateTime.Now);
            gs[05] = EdiValue.Time(4, DateTime.Now);
            gs[06] = number.Substring(number.Length - 4, 4);
            gs[07] = pipelineEdiSetting.GS07_Segment.Trim();
            gs[08] = pipelineEdiSetting.GS08_Segment.Trim();
            ediDocument.Segments.Add(gs);

            var st = new EdiSegment("ST");
            st[01] = pipelineEdiSetting.ST01_Segment.Trim();
            st[02] = gs[06];
            ediDocument.Segments.Add(st);

            var bgn = new EdiSegment("BGN");
            bgn[01] = "14";
            bgn[02] = batch.ReferenceNumber.ToString();
            bgn[03] = EdiValue.Date(8, DateTime.Now);
            bgn[04] = "";
            bgn[05] = "";
            bgn[06] = "";
            bgn[07] = "G1";
            ediDocument.Segments.Add(bgn);


            var dtmIssue = new EdiSegment("DTM");
            dtmIssue[01] = "102";
            dtmIssue[02] = "";
            dtmIssue[03] = "";
            dtmIssue[04] = "";
            dtmIssue[05] = "DT";
            dtmIssue[06] = EdiValue.Date(8, DateTime.Now) + EdiValue.Time(4, DateTime.Now);
            ediDocument.Segments.Add(dtmIssue);

            var N1_header_ServiceProvider = new EdiSegment("N1");
            N1_header_ServiceProvider[01] = "N1";
            N1_header_ServiceProvider[02] = "";
            N1_header_ServiceProvider[03] = "1";
            N1_header_ServiceProvider[04] = batch.pipeDUNSNo.Trim();
            ediDocument.Segments.Add(N1_header_ServiceProvider);

            var N1_header_ServiceRequester = new EdiSegment("N1");
            N1_header_ServiceRequester[01] = "SJ";
            N1_header_ServiceRequester[02] = "";
            N1_header_ServiceRequester[03] = "1";
            N1_header_ServiceRequester[04] = batch.ServiceRequester.Trim();
            ediDocument.Segments.Add(N1_header_ServiceRequester);

            #region supply
            foreach (var item in nomSupply)
            {
                var dtmEffectiveRD8 = new EdiSegment("DTM");
                dtmEffectiveRD8[01] = "007";
                dtmEffectiveRD8[02] = "";
                dtmEffectiveRD8[03] = "";
                dtmEffectiveRD8[04] = "";
                dtmEffectiveRD8[05] = "RD8";
                dtmEffectiveRD8[06] = batch.DateBeg.ToString("yyyyMMdd") + "-" + batch.DateEnd.ToString("yyyyMMdd");
                ediDocument.Segments.Add(dtmEffectiveRD8);

                var cycleSegment = new EdiSegment("N9");
                cycleSegment[01] = "CYI";
                cycleSegment[02] = batch.CycleCode;
                ediDocument.Segments.Add(cycleSegment);
                #region 
                var sln_Segment = new EdiSegment("SLN");
                sln_Segment[01] = item.NominatorTrackingId.Trim();
                sln_Segment[02] = "";
                sln_Segment[03] = "I";
                ediDocument.Segments.Add(sln_Segment);
                #endregion

                #region
                if (!string.IsNullOrEmpty(item.QuantityTypeIndicator))
                {
                    var LQ_QtyTypeInd = new EdiSegment("LQ");
                    LQ_QtyTypeInd[01] = "QT";
                    LQ_QtyTypeInd[02] = item.QuantityTypeIndicator.Trim();
                    ediDocument.Segments.Add(LQ_QtyTypeInd);
                }
                
                
                if (!string.IsNullOrEmpty(item.CapacityTypeIndicator) && (batch.pipeDUNSNo != "006931794"))
                {
                    var LQ_CapTypeInd = new EdiSegment("LQ");
                    LQ_CapTypeInd[01] = "CQ";
                    LQ_CapTypeInd[02] = item.CapacityTypeIndicator.Trim();
                    ediDocument.Segments.Add(LQ_CapTypeInd);
                }
                
                if (!string.IsNullOrEmpty(batch.NomSubCycle) && (batch.pipeDUNSNo != "006931794"))
                {
                    var LQ_SubSeqCycleInd = new EdiSegment("LQ");
                    LQ_SubSeqCycleInd[01] = "SCI";
                    LQ_SubSeqCycleInd[02] = batch.NomSubCycle.Trim();
                    ediDocument.Segments.Add(LQ_SubSeqCycleInd);
                }
                
                if (!string.IsNullOrEmpty(item.ExportDecleration) && (batch.pipeDUNSNo != "006931794"))
                {
                    var LQ_QtyTypeInd = new EdiSegment("LQ");
                    LQ_QtyTypeInd[01] = "XD";
                    LQ_QtyTypeInd[02] = item.ExportDecleration.Trim();
                    ediDocument.Segments.Add(LQ_QtyTypeInd);
                }
                
                if (!string.IsNullOrEmpty(item.BidupIndicator) && (batch.pipeDUNSNo != "006931794"))
                {
                    var LQ_BidUpInd = new EdiSegment("LQ");
                    LQ_BidUpInd[01] = "BUI";
                    LQ_BidUpInd[02] = item.BidupIndicator.Trim();
                    ediDocument.Segments.Add(LQ_BidUpInd);
                }
                
                if (!string.IsNullOrEmpty(item.ProcessingRightIndicator) && (batch.pipeDUNSNo != "006931794"))
                {
                    var LQ_ProcessingRightIndicator = new EdiSegment("LQ");
                    LQ_ProcessingRightIndicator[01] = "PRI";
                    LQ_ProcessingRightIndicator[02] = item.ProcessingRightIndicator.Trim();
                    ediDocument.Segments.Add(LQ_ProcessingRightIndicator);
                }
                
                if (!string.IsNullOrEmpty(item.MaxRateIndicator) && (batch.pipeDUNSNo != "006931794"))
                {
                    var LQ_MaxRateIndicator = new EdiSegment("LQ");
                    LQ_MaxRateIndicator[01] = "MRI";
                    LQ_MaxRateIndicator[02] = item.MaxRateIndicator.Trim();
                    ediDocument.Segments.Add(LQ_MaxRateIndicator);
                }
                #endregion

                #region N9 Extended reference information
                
                if (!string.IsNullOrEmpty(item.Route))
                {
                    var N9_Route = new EdiSegment("N9");
                    N9_Route[01] = "RU";
                    N9_Route[02] = item.Route.Trim();
                    ediDocument.Segments.Add(N9_Route);
                }
                if (!string.IsNullOrEmpty(item.AssociatedContract))
                {
                    var N9_AssociatedContract = new EdiSegment("N9");
                    N9_AssociatedContract[01] = "KAS";
                    N9_AssociatedContract[02] = item.AssociatedContract.Trim();
                    ediDocument.Segments.Add(N9_AssociatedContract);
                }
                if (!string.IsNullOrEmpty(item.ServiceProviderActivityCode))
                {
                    var N9_ServiceProviderActivityCode = new EdiSegment("N9");
                    N9_ServiceProviderActivityCode[01] = "BE";
                    N9_ServiceProviderActivityCode[02] = item.ServiceProviderActivityCode.Trim();
                    ediDocument.Segments.Add(N9_ServiceProviderActivityCode);
                }
                if (!string.IsNullOrEmpty(item.DealType))
                {
                    var N9_DealType = new EdiSegment("N9");
                    N9_DealType[01] = "PD";
                    N9_DealType[02] = item.DealType.Trim();
                    ediDocument.Segments.Add(N9_DealType);
                }
                if (!string.IsNullOrEmpty(item.NominationUserData1))
                {
                    var N9_NominationUserData1 = new EdiSegment("N9");
                    N9_NominationUserData1[01] = "JD";
                    N9_NominationUserData1[02] = item.NominationUserData1.Trim();
                    ediDocument.Segments.Add(N9_NominationUserData1);
                }
                if (!string.IsNullOrEmpty(item.NominationUserData2))
                {
                    var N9_NominationUserData2 = new EdiSegment("N9");
                    N9_NominationUserData2[01] = "Y8";
                    N9_NominationUserData2[02] = item.NominationUserData2.Trim();
                    ediDocument.Segments.Add(N9_NominationUserData2);
                }
                if (!string.IsNullOrEmpty(item.PathRank) && (batch.pipeDUNSNo != "006931794"))
                {
                    var N9_PathRank = new EdiSegment("N9");
                    N9_PathRank[01] = "PRK";
                    N9_PathRank[02] = item.PathRank.Trim();
                    ediDocument.Segments.Add(N9_PathRank);
                }
                #endregion
                #region N1 party Identification Upstraem
                if (!string.IsNullOrEmpty(item.UpstreamIdentifier))
                {
                    var N1_PartyIdentUp = new EdiSegment("N1");
                    N1_PartyIdentUp[01] = "US";
                    N1_PartyIdentUp[02] = "";
                    N1_PartyIdentUp[03] = "1";
                    N1_PartyIdentUp[04] = item.UpstreamIdentifier.Trim();
                    ediDocument.Segments.Add(N1_PartyIdentUp);
                }
                #endregion
                #region LCD Place Location Description Receipt Location 
                if (!string.IsNullOrEmpty(item.ReceiptLocationIdentifier))
                {
                    var LCD_RecLoc = new EdiSegment("LCD");
                    LCD_RecLoc[01] = "1";
                    LCD_RecLoc[02] = "MQ";
                    LCD_RecLoc[03] = "";
                    LCD_RecLoc[04] = "";
                    LCD_RecLoc[05] = "SV"; //
                    LCD_RecLoc[06] = item.ReceiptLocationIdentifier.Trim();
                    ediDocument.Segments.Add(LCD_RecLoc);
                }
                #endregion
                #region
                if (!string.IsNullOrEmpty(item.UpstreamContractIdentifier))
                {
                    var N9_UpConIden = new EdiSegment("N9");
                    N9_UpConIden[01] = "UP";
                    N9_UpConIden[02] = item.UpstreamContractIdentifier.Trim();
                    ediDocument.Segments.Add(N9_UpConIden);
                }
                if (!string.IsNullOrEmpty(item.UpstreamPackageId))
                {
                    var N9_UpPakageID = new EdiSegment("N9");
                    N9_UpPakageID[01] = "PKU";
                    N9_UpPakageID[02] = item.UpstreamPackageId.Trim();
                    ediDocument.Segments.Add(N9_UpPakageID);
                }
                #endregion
                #region LQ Industry Code Identification Upstream
                if (!string.IsNullOrEmpty(item.ReceiptRank))
                {
                    var LQ_recRank = new EdiSegment("LQ");
                    LQ_recRank[01] = "R2";
                    LQ_recRank[02] = item.ReceiptRank.Trim();
                    ediDocument.Segments.Add(LQ_recRank);
                }
                if (!string.IsNullOrEmpty(item.UpstreamRank))
                {
                    var LQ_UpRank = new EdiSegment("LQ");
                    LQ_UpRank[01] = "R1";
                    LQ_UpRank[02] = item.UpstreamRank.Trim();
                    ediDocument.Segments.Add(LQ_UpRank);
                }
                #endregion
                #region QTY Quantity delivery
                if (!string.IsNullOrEmpty(item.Quantity.ToString()))
                {
                    var qty_delQty = new EdiSegment("QTY");
                    qty_delQty[01] = "38";
                    qty_delQty[02] = item.Quantity.ToString();
                    qty_delQty[03] = "BZ";
                    ediDocument.Segments.Add(qty_delQty);
                }
                #endregion
            }
            #endregion
            #region Transport
            foreach (var item in nomTransport)
            {
                var dtmEffective = new EdiSegment("DTM");
                dtmEffective[01] = "007";//effective day code
                dtmEffective[02] = "";
                dtmEffective[03] = "";
                dtmEffective[04] = "";
                dtmEffective[05] = "RDT";//Date Time Period format qualifier
                dtmEffective[06] = batch.DateBeg.ToString("yyyyMMddHHmm") + "-" + batch.DateEnd.ToString("yyyyMMddHHmm");
                ediDocument.Segments.Add(dtmEffective);
                if (item != null && !string.IsNullOrEmpty(item.ContractNumber))
                {
                    var contractSegment = new EdiSegment("CS");
                    contractSegment[01] = item.ContractNumber.Trim();
                    contractSegment[02] = "";
                    contractSegment[03] = "";
                    contractSegment[04] = "NMT";
                    contractSegment[05] = "T";
                    ediDocument.Segments.Add(contractSegment);
                }
                #region SLN segment sublineItem
                var sln_Segment = new EdiSegment("SLN");
                sln_Segment[01] = item.NominatorTrackingId.Trim();
                sln_Segment[02] = "";
                sln_Segment[03] = "I";
                ediDocument.Segments.Add(sln_Segment);
                
                #endregion
                #region LQ segment Industry Code
                if (!string.IsNullOrEmpty(item.QuantityTypeIndicator))
                {
                    var LQ_QtyTypeInd = new EdiSegment("LQ");
                    LQ_QtyTypeInd[01] = "QT";
                    LQ_QtyTypeInd[02] = item.QuantityTypeIndicator.Trim();
                    ediDocument.Segments.Add(LQ_QtyTypeInd);
                }
                if (!string.IsNullOrEmpty(item.CapacityTypeIndicator))
                {
                    var LQ_CapTypeInd = new EdiSegment("LQ");
                    LQ_CapTypeInd[01] = "CQ";
                    LQ_CapTypeInd[02] = item.CapacityTypeIndicator.Trim();
                    ediDocument.Segments.Add(LQ_CapTypeInd);
                }
                if (!string.IsNullOrEmpty(batch.NomSubCycle))
                {
                    var LQ_SubSeqCycleInd = new EdiSegment("LQ");
                    LQ_SubSeqCycleInd[01] = "SCI";
                    LQ_SubSeqCycleInd[02] = batch.NomSubCycle.Trim();
                    ediDocument.Segments.Add(LQ_SubSeqCycleInd);
                }
                if (!string.IsNullOrEmpty(item.ExportDecleration))
                {
                    var LQ_QtyTypeInd = new EdiSegment("LQ");
                    LQ_QtyTypeInd[01] = "XD";
                    LQ_QtyTypeInd[02] = item.ExportDecleration.Trim();
                    ediDocument.Segments.Add(LQ_QtyTypeInd);
                }
                if (!string.IsNullOrEmpty(item.BidupIndicator))
                {
                    var LQ_BidUpInd = new EdiSegment("LQ");
                    LQ_BidUpInd[01] = "BUI";
                    LQ_BidUpInd[02] = item.BidupIndicator.Trim();
                    ediDocument.Segments.Add(LQ_BidUpInd);
                }
                if (!string.IsNullOrEmpty(item.ProcessingRightIndicator))
                {
                    var LQ_ProcessingRightIndicator = new EdiSegment("LQ");
                    LQ_ProcessingRightIndicator[01] = "PRI";
                    LQ_ProcessingRightIndicator[02] = item.ProcessingRightIndicator.Trim();
                    ediDocument.Segments.Add(LQ_ProcessingRightIndicator);
                }
                if (!string.IsNullOrEmpty(item.MaxRateIndicator))
                {
                    var LQ_MaxRateIndicator = new EdiSegment("LQ");
                    LQ_MaxRateIndicator[01] = "MRI";
                    LQ_MaxRateIndicator[02] = item.MaxRateIndicator.Trim();
                    ediDocument.Segments.Add(LQ_MaxRateIndicator);
                }
                #endregion
                #region N9 Extended reference information
                if (!string.IsNullOrEmpty(item.Route))
                {
                    var N9_Route = new EdiSegment("N9");
                    N9_Route[01] = "RU";
                    N9_Route[02] = item.Route.Trim();
                    ediDocument.Segments.Add(N9_Route);
                }
                if (!string.IsNullOrEmpty(item.PackageId))
                {
                    var N9_PackageId = new EdiSegment("N9");
                    N9_PackageId[01] = "PKG";
                    N9_PackageId[02] = item.PackageId.Trim();
                    ediDocument.Segments.Add(N9_PackageId);
                }
                if (!string.IsNullOrEmpty(item.AssociatedContract))
                {
                    var N9_AssociatedContract = new EdiSegment("N9");
                    N9_AssociatedContract[01] = "KAS";
                    N9_AssociatedContract[02] = item.AssociatedContract.Trim();
                    ediDocument.Segments.Add(N9_AssociatedContract);
                }
                if (!string.IsNullOrEmpty(item.ServiceProviderActivityCode))
                {
                    var N9_ServiceProviderActivityCode = new EdiSegment("N9");
                    N9_ServiceProviderActivityCode[01] = "BE";
                    N9_ServiceProviderActivityCode[02] = item.ServiceProviderActivityCode.Trim();
                    ediDocument.Segments.Add(N9_ServiceProviderActivityCode);
                }
                if (!string.IsNullOrEmpty(item.DealType))
                {
                    var N9_DealType = new EdiSegment("N9");
                    N9_DealType[01] = "PD";
                    N9_DealType[02] = item.DealType.Trim();
                    ediDocument.Segments.Add(N9_DealType);
                }
                if (!string.IsNullOrEmpty(item.NominationUserData1))
                {
                    var N9_NominationUserData1 = new EdiSegment("N9");
                    N9_NominationUserData1[01] = "JD";
                    N9_NominationUserData1[02] = item.NominationUserData1.Trim();
                    ediDocument.Segments.Add(N9_NominationUserData1);
                }
                if (!string.IsNullOrEmpty(item.NominationUserData2))
                {
                    var N9_NominationUserData2 = new EdiSegment("N9");
                    N9_NominationUserData2[01] = "Y8";
                    N9_NominationUserData2[02] = item.NominationUserData2.Trim();
                    ediDocument.Segments.Add(N9_NominationUserData2);
                }
                if (!string.IsNullOrEmpty(item.PathRank) && (batch.pipeDUNSNo != "006931794"))
                {
                    var N9_PathRank = new EdiSegment("N9");
                    N9_PathRank[01] = "PRK";
                    N9_PathRank[02] = item.PathRank.Trim();
                    ediDocument.Segments.Add(N9_PathRank);
                }
                #endregion
                #region N1 party Identification Upstraem
                var N1_PartyIdentUp = new EdiSegment("N1");
                N1_PartyIdentUp[01] = "US"; 
                N1_PartyIdentUp[02] = "";
                N1_PartyIdentUp[03] = "ZZ";
                N1_PartyIdentUp[04] = "N/A";
                ediDocument.Segments.Add(N1_PartyIdentUp);
                #endregion
                #region LCD Place Location Description Receipt Location 
                if (!string.IsNullOrEmpty(item.ReceiptLocationIdentifier))
                {
                    var LCD_RecLoc = new EdiSegment("LCD");
                    LCD_RecLoc[01] = "1";
                    LCD_RecLoc[02] = "M2";
                    LCD_RecLoc[03] = "";
                    LCD_RecLoc[04] = "";
                    LCD_RecLoc[05] = "SV";
                    LCD_RecLoc[06] = item.ReceiptLocationIdentifier.Trim();
                    ediDocument.Segments.Add(LCD_RecLoc);
                }
                #endregion
                #region N9 extended reference information Upstream
                if (!string.IsNullOrEmpty(item.UpstreamContractIdentifier))
                {
                    var N9_UpConIden = new EdiSegment("N9");
                    N9_UpConIden[01] = "UP";
                    N9_UpConIden[02] = item.UpstreamContractIdentifier.Trim();
                    ediDocument.Segments.Add(N9_UpConIden);
                }
                if (!string.IsNullOrEmpty(item.UpstreamPackageId))
                {
                    var N9_UpPakageID = new EdiSegment("N9");
                    N9_UpPakageID[01] = "PKU";
                    N9_UpPakageID[02] = item.UpstreamPackageId.Trim();
                    ediDocument.Segments.Add(N9_UpPakageID);
                }
                #endregion
                
                #region QTY Quantity delivery
                if (!string.IsNullOrEmpty(item.Quantity.ToString()))
                {
                    var qty_delQty = new EdiSegment("QTY");
                    qty_delQty[01] = "QD";
                    qty_delQty[02] = item.Quantity.ToString();
                    qty_delQty[03] = "BZ";
                    ediDocument.Segments.Add(qty_delQty);
                }
                #endregion
                #region N1 Party Identification Downstream
                var N1_PartyIdentDwn = new EdiSegment("N1");
                N1_PartyIdentDwn[01] = "DW";
                N1_PartyIdentDwn[02] = "";
                N1_PartyIdentDwn[03] = "ZZ";
                N1_PartyIdentDwn[04] = "N/A";
                ediDocument.Segments.Add(N1_PartyIdentDwn);
                #endregion
                #region
                if (!string.IsNullOrEmpty(item.DeliveryLocationIdentifer))
                {
                    var LCD_DelLoc = new EdiSegment("LCD");
                    LCD_DelLoc[01] = "1";
                    LCD_DelLoc[02] = "MD";
                    LCD_DelLoc[03] = "";
                    LCD_DelLoc[04] = "";
                    LCD_DelLoc[05] = "SJ"; //
                    LCD_DelLoc[06] = item.DeliveryLocationIdentifer.Trim();
                    ediDocument.Segments.Add(LCD_DelLoc);
                }
                #endregion
                #region N9 extended reference information Dwnstream
                if (!string.IsNullOrEmpty(item.DownstreamContractIdentifier))
                {
                    var N9_DwnConIden = new EdiSegment("N9");
                    N9_DwnConIden[01] = "DT";
                    N9_DwnConIden[02] = item.DownstreamContractIdentifier.Trim();
                    ediDocument.Segments.Add(N9_DwnConIden);
                }
                
                if (!string.IsNullOrEmpty(item.DownstreamPackageId))
                {
                    var N9_DwnPakageID = new EdiSegment("N9");
                    N9_DwnPakageID[01] = "PGD";
                    N9_DwnPakageID[02] = item.DownstreamPackageId.Trim();
                    ediDocument.Segments.Add(N9_DwnPakageID);
                }

                #endregion
                
            }
            #endregion
            #region Market path
            foreach (var item in nomMarket)
            {
                
                var dtmEffective = new EdiSegment("DTM");
                dtmEffective[01] = "007";
                dtmEffective[02] = "";
                dtmEffective[03] = "";
                dtmEffective[04] = "";
                dtmEffective[05] = "RDT";
                dtmEffective[06] = batch.DateBeg.ToString("yyyyMMddHHmm") + "-" + batch.DateEnd.ToString("yyyyMMddHHmm");
                ediDocument.Segments.Add(dtmEffective);
                
                if (item != null && !string.IsNullOrEmpty(item.ContractNumber))
                {
                    var contractSegment = new EdiSegment("CS");
                    contractSegment[01] = item.ContractNumber.Trim();
                    contractSegment[02] = "";
                    contractSegment[03] = "";
                    contractSegment[04] = "NMT";
                    contractSegment[05] = "U";
                    ediDocument.Segments.Add(contractSegment);
                }
                #region SLN segment sublineItem
                var sln_Segment = new EdiSegment("SLN");
                sln_Segment[01] = item.NominatorTrackingId.Trim();
                sln_Segment[02] = "";
                sln_Segment[03] = "I";
                ediDocument.Segments.Add(sln_Segment);
                
                #endregion
                #region LQ segment Industry Code
                
                if (!string.IsNullOrEmpty(item.QuantityTypeIndicator))
                {
                    var LQ_QtyTypeInd = new EdiSegment("LQ");
                    LQ_QtyTypeInd[01] = "QT";
                    LQ_QtyTypeInd[02] = item.QuantityTypeIndicator.Trim();
                    ediDocument.Segments.Add(LQ_QtyTypeInd);
                }
                
                if (!string.IsNullOrEmpty(item.CapacityTypeIndicator) && (batch.pipeDUNSNo != "006931794"))
                {
                    var LQ_CapTypeInd = new EdiSegment("LQ");
                    LQ_CapTypeInd[01] = "CQ";
                    LQ_CapTypeInd[02] = item.CapacityTypeIndicator.Trim();
                    ediDocument.Segments.Add(LQ_CapTypeInd);
                }
                
                if (!string.IsNullOrEmpty(batch.NomSubCycle) && (batch.pipeDUNSNo != "006931794"))
                {
                    var LQ_SubSeqCycleInd = new EdiSegment("LQ");
                    LQ_SubSeqCycleInd[01] = "SCI";
                    LQ_SubSeqCycleInd[02] = batch.NomSubCycle.Trim();
                    ediDocument.Segments.Add(LQ_SubSeqCycleInd);
                }
                
                if (!string.IsNullOrEmpty(item.ExportDecleration) && (batch.pipeDUNSNo != "006931794"))
                {
                    var LQ_QtyTypeInd = new EdiSegment("LQ");
                    LQ_QtyTypeInd[01] = "XD";
                    LQ_QtyTypeInd[02] = item.ExportDecleration.Trim();
                    ediDocument.Segments.Add(LQ_QtyTypeInd);
                }
                
                if (!string.IsNullOrEmpty(item.BidupIndicator) && (batch.pipeDUNSNo != "006931794"))
                {
                    var LQ_BidUpInd = new EdiSegment("LQ");
                    LQ_BidUpInd[01] = "BUI";
                    LQ_BidUpInd[02] = item.BidupIndicator.Trim();
                    ediDocument.Segments.Add(LQ_BidUpInd);
                }
                
                if (!string.IsNullOrEmpty(item.ProcessingRightIndicator))
                {
                    var LQ_ProcessingRightIndicator = new EdiSegment("LQ");
                    LQ_ProcessingRightIndicator[01] = "PRI";
                    LQ_ProcessingRightIndicator[02] = item.ProcessingRightIndicator.Trim();
                    ediDocument.Segments.Add(LQ_ProcessingRightIndicator);
                }
                
                if (!string.IsNullOrEmpty(item.MaxRateIndicator) && (batch.pipeDUNSNo != "006931794"))
                {
                    var LQ_MaxRateIndicator = new EdiSegment("LQ");
                    LQ_MaxRateIndicator[01] = "MRI";
                    LQ_MaxRateIndicator[02] = item.MaxRateIndicator.Trim();
                    ediDocument.Segments.Add(LQ_MaxRateIndicator);
                }
                #endregion
                #region N9 Extended reference information
                
                if (!string.IsNullOrEmpty(item.Route))
                {
                    var N9_Route = new EdiSegment("N9");
                    N9_Route[01] = "RU";
                    N9_Route[02] = item.Route.Trim();
                    ediDocument.Segments.Add(N9_Route);
                }
                
                if (!string.IsNullOrEmpty(item.PackageId))
                {
                    var N9_PackageId = new EdiSegment("N9");
                    N9_PackageId[01] = "PKG";
                    N9_PackageId[02] = item.PackageId.Trim();
                    ediDocument.Segments.Add(N9_PackageId);
                }
                
                if (!string.IsNullOrEmpty(item.AssociatedContract))
                {
                    var N9_AssociatedContract = new EdiSegment("N9");
                    N9_AssociatedContract[01] = "KAS";
                    N9_AssociatedContract[02] = item.AssociatedContract.Trim();
                    ediDocument.Segments.Add(N9_AssociatedContract);
                }
                
                if (!string.IsNullOrEmpty(item.ServiceProviderActivityCode))
                {
                    var N9_ServiceProviderActivityCode = new EdiSegment("N9");
                    N9_ServiceProviderActivityCode[01] = "BE";
                    N9_ServiceProviderActivityCode[02] = item.ServiceProviderActivityCode.Trim();
                    ediDocument.Segments.Add(N9_ServiceProviderActivityCode);
                }
                
                if (!string.IsNullOrEmpty(item.DealType))
                {
                    var N9_DealType = new EdiSegment("N9");
                    N9_DealType[01] = "PD";
                    N9_DealType[02] = item.DealType.Trim();
                    ediDocument.Segments.Add(N9_DealType);
                }
                
                if (!string.IsNullOrEmpty(item.NominationUserData1))
                {
                    var N9_NominationUserData1 = new EdiSegment("N9");
                    N9_NominationUserData1[01] = "JD";
                    N9_NominationUserData1[02] = item.NominationUserData1.Trim();
                    ediDocument.Segments.Add(N9_NominationUserData1);
                }
                
                if (!string.IsNullOrEmpty(item.NominationUserData2))
                {
                    var N9_NominationUserData2 = new EdiSegment("N9");
                    N9_NominationUserData2[01] = "Y8";
                    N9_NominationUserData2[02] = item.NominationUserData2.Trim();
                    ediDocument.Segments.Add(N9_NominationUserData2);
                }
                
                if (!string.IsNullOrEmpty(item.PathRank) && (batch.pipeDUNSNo != "006931794"))
                {
                    var N9_PathRank = new EdiSegment("N9");
                    N9_PathRank[01] = "PRK";
                    N9_PathRank[02] = item.PathRank.Trim();
                    ediDocument.Segments.Add(N9_PathRank);
                }
                #endregion
                #region N1 Party Identification Downstream
                
                if (!string.IsNullOrEmpty(item.DownstreamIdentifier))
                {
                    var N1_PartyIdentDwn = new EdiSegment("N1");
                    N1_PartyIdentDwn[01] = "DW";
                    N1_PartyIdentDwn[02] = "";
                    N1_PartyIdentDwn[03] = "1";
                    N1_PartyIdentDwn[04] = item.DownstreamIdentifier.Trim();//Identification Code
                    ediDocument.Segments.Add(N1_PartyIdentDwn);
                }
                #endregion
                #region LCD Place Location Description Delivery Location
                
                if (!string.IsNullOrEmpty(item.DeliveryLocationIdentifer))
                {
                    var LCD_DelLoc = new EdiSegment("LCD");
                    LCD_DelLoc[01] = "1";
                    LCD_DelLoc[02] = "MQ";
                    LCD_DelLoc[03] = "";
                    LCD_DelLoc[04] = "";
                    LCD_DelLoc[05] = "SV";
                    LCD_DelLoc[06] = item.DeliveryLocationIdentifer.Trim();
                    ediDocument.Segments.Add(LCD_DelLoc);
                }
                #endregion
                #region N9 extended reference information Dwnstream
                
                if (!string.IsNullOrEmpty(item.DownstreamContractIdentifier))
                {
                    var N9_DwnConIden = new EdiSegment("N9");
                    N9_DwnConIden[01] = "DT";
                    N9_DwnConIden[02] = item.DownstreamContractIdentifier.Trim();
                    ediDocument.Segments.Add(N9_DwnConIden);
                }
                
                if (!string.IsNullOrEmpty(item.DownstreamPackageId))
                {
                    var N9_DwnPakageID = new EdiSegment("N9");
                    N9_DwnPakageID[01] = "PGD";
                    N9_DwnPakageID[02] = item.DownstreamPackageId.Trim();
                    ediDocument.Segments.Add(N9_DwnPakageID);
                }

                #endregion
                #region LQ Industry Code Identification Downstream
                
                if (!string.IsNullOrEmpty(item.DownstreamRank))
                {
                    var LQ_DwnRank = new EdiSegment("LQ");
                    LQ_DwnRank[01] = "R4";
                    LQ_DwnRank[02] = item.DownstreamRank.Trim();
                    ediDocument.Segments.Add(LQ_DwnRank);
                }
                #endregion
                #region QTY Quantity delivery
                if (!string.IsNullOrEmpty(item.Quantity.ToString()))
                {
                    var qty_delQty = new EdiSegment("QTY");
                    qty_delQty[01] = "38";
                    qty_delQty[02] = item.Quantity.ToString();
                    qty_delQty[03] = "BZ";
                    ediDocument.Segments.Add(qty_delQty);
                }
                #endregion
            }
            #endregion
            
            var se = new EdiSegment("SE");
            se[01] = (ediDocument.Segments.Count() - 1).ToString();
            se[02] = st[02];
            ediDocument.Segments.Add(se);

            var ge = new EdiSegment("GE");
            ge[01] = "1";
            ge[02] = gs[06];
            ediDocument.Segments.Add(ge);

            var iea = new EdiSegment("IEA");
            iea[01] = "1";
            iea[02] = number;
            ediDocument.Segments.Add(iea);

            // more segments...
            ediDocument.Options.SegmentTerminator = !string.IsNullOrEmpty(pipelineEdiSetting.SegmentSeperator) ? Convert.ToChar(pipelineEdiSetting.SegmentSeperator) : EdiOptions.DefaultSegmentTerminator;
            ediDocument.Options.ElementSeparator = Convert.ToChar(pipelineEdiSetting.DataSeparator);
            return ediDocument;
        }
        public EdiDocument GenerateEDIPathedNomination(BatchDTO batch, V4_NominationDTO Nom, PipelineEDISettingDTO pipelineEdiSetting, bool IsTest)
        {
            string number = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            //envelope header section
            var ediDocument = new EdiDocument();
            var isa = new EdiSegment("ISA");
            isa[01] = "00";
            isa[02] = "".PadRight(10);
            isa[03] = "00";
            isa[04] = "".PadRight(10);
            isa[05] = "01";
            isa[06] = batch.ServiceRequester.Trim().PadRight(15);
            isa[07] = "14";
            isa[08] = pipelineEdiSetting.ISA08_segment.Trim().PadRight(15);
            isa[09] = EdiValue.Date(6, DateTime.Now);
            isa[10] = EdiValue.Time(4, DateTime.Now);
            isa[11] = pipelineEdiSetting.ISA11_Segment.Trim();
            isa[12] = pipelineEdiSetting.ISA12_Segment.Trim();
            isa[13] = number;
            isa[14] = "0";
            isa[15] = IsTest ? "T" : "P";
            isa[16] = pipelineEdiSetting.ISA16_Segment.Trim();
            ediDocument.Segments.Add(isa);

            var gs = new EdiSegment("GS");
            gs[01] = pipelineEdiSetting.GS01_Segment.Trim();
            gs[02] = pipelineEdiSetting.GS02_Segment.Trim();
            gs[03] = pipelineEdiSetting.GS03_Segment.Trim();
            gs[04] = EdiValue.Date(6, DateTime.Now);
            gs[05] = EdiValue.Time(4, DateTime.Now);
            gs[06] = number.Substring(number.Length - 4, 4);
            gs[07] = pipelineEdiSetting.GS07_Segment.Trim();
            gs[08] = pipelineEdiSetting.GS08_Segment.Trim();
            ediDocument.Segments.Add(gs);

            
            var st = new EdiSegment("ST");
            st[01] = pipelineEdiSetting.ST01_Segment.Trim();
            st[02] = gs[06];
            ediDocument.Segments.Add(st);

            var bgn = new EdiSegment("BGN");
            bgn[01] = "00";
            bgn[02] = batch.ReferenceNumber.ToString();
            bgn[03] = EdiValue.Date(8, DateTime.Now);
            bgn[04] = "";
            bgn[05] = "";
            bgn[06] = "";
            bgn[07] = "G1";
            ediDocument.Segments.Add(bgn);

            var dtmIssue = new EdiSegment("DTM");
            dtmIssue[01] = "102";
            dtmIssue[02] = "";
            dtmIssue[03] = "";
            dtmIssue[04] = "";
            dtmIssue[05] = "DT";
            dtmIssue[06] = EdiValue.Date(8, DateTime.Now) + EdiValue.Time(4, DateTime.Now);
            ediDocument.Segments.Add(dtmIssue);

            var N1_header_ServiceProvider = new EdiSegment("N1");
            N1_header_ServiceProvider[01] = "SJ";
            N1_header_ServiceProvider[02] = "";
            N1_header_ServiceProvider[03] = "SV";
            N1_header_ServiceProvider[04] = batch.pipeDUNSNo.Trim();
            ediDocument.Segments.Add(N1_header_ServiceProvider);

            var N1_header_ServiceRequester = new EdiSegment("N1");
            N1_header_ServiceRequester[01] = "78";
            N1_header_ServiceRequester[02] = "";
            N1_header_ServiceRequester[03] = "SV";
            N1_header_ServiceRequester[04] = batch.ServiceRequester.Trim();
            ediDocument.Segments.Add(N1_header_ServiceRequester);

            var dtmEffectiveRD8 = new EdiSegment("DTM");
            dtmEffectiveRD8[01] = "007";
            dtmEffectiveRD8[02] = "";
            dtmEffectiveRD8[03] = "";
            dtmEffectiveRD8[04] = "";
            dtmEffectiveRD8[05] = "RD8";
            dtmEffectiveRD8[06] = batch.DateBeg.ToString("yyyyMMdd") + "-" + batch.DateEnd.ToString("yyyyMMdd");
            ediDocument.Segments.Add(dtmEffectiveRD8);
            
            var cycleSegment = new EdiSegment("N9");
            cycleSegment[01] = "CYI";
            cycleSegment[02] = batch.CycleCode;
            ediDocument.Segments.Add(cycleSegment);
            
            if (Nom != null && !string.IsNullOrEmpty(Nom.ContractNumber))
            {
                var contractSegment = new EdiSegment("CS");
                contractSegment[01] = Nom.ContractNumber.Trim();
                contractSegment[02] = "";
                contractSegment[03] = "";
                contractSegment[04] = "NMT";
                contractSegment[05] = "P";
                ediDocument.Segments.Add(contractSegment);
            }
            #region
            var sln_Segment = new EdiSegment("SLN");
            sln_Segment[01] = Nom.NominatorTrackingId.Trim();
            sln_Segment[02] = "";
            sln_Segment[03] = "I";
            ediDocument.Segments.Add(sln_Segment);
            #endregion
            #region LQ segment Industry Code
            if (!string.IsNullOrEmpty(Nom.QuantityTypeIndicator))
            {
                var LQ_QtyTypeInd = new EdiSegment("LQ");
                LQ_QtyTypeInd[01] = "QT";
                LQ_QtyTypeInd[02] = Nom.QuantityTypeIndicator.Trim();
                ediDocument.Segments.Add(LQ_QtyTypeInd);
            }
            if (!string.IsNullOrEmpty(Nom.CapacityTypeIndicator))
            {
                var LQ_CapTypeInd = new EdiSegment("LQ");
                LQ_CapTypeInd[01] = "CQ";
                LQ_CapTypeInd[02] = Nom.CapacityTypeIndicator.Trim();
                ediDocument.Segments.Add(LQ_CapTypeInd);
            }
            if (!string.IsNullOrEmpty(batch.NomSubCycle))
            {
                var LQ_SubSeqCycleInd = new EdiSegment("LQ");
                LQ_SubSeqCycleInd[01] = "SCI";
                LQ_SubSeqCycleInd[02] = batch.NomSubCycle.Trim();
                ediDocument.Segments.Add(LQ_SubSeqCycleInd);
            }
            if (!string.IsNullOrEmpty(Nom.ExportDecleration))
            {
                var LQ_QtyTypeInd = new EdiSegment("LQ");
                LQ_QtyTypeInd[01] = "XD";
                LQ_QtyTypeInd[02] = Nom.ExportDecleration.Trim();
                ediDocument.Segments.Add(LQ_QtyTypeInd);
            }
            if (!string.IsNullOrEmpty(Nom.BidupIndicator))
            {
                var LQ_BidUpInd = new EdiSegment("LQ");
                LQ_BidUpInd[01] = "BUI";
                LQ_BidUpInd[02] = Nom.BidupIndicator.Trim();
                ediDocument.Segments.Add(LQ_BidUpInd);
            }
            if (!string.IsNullOrEmpty(Nom.ProcessingRightIndicator))
            {
                var LQ_ProcessingRightIndicator = new EdiSegment("LQ");
                LQ_ProcessingRightIndicator[01] = "PRI";
                LQ_ProcessingRightIndicator[02] = Nom.ProcessingRightIndicator.Trim();
                ediDocument.Segments.Add(LQ_ProcessingRightIndicator);
            }
            if (!string.IsNullOrEmpty(Nom.MaxRateIndicator))
            {
                var LQ_MaxRateIndicator = new EdiSegment("LQ");
                LQ_MaxRateIndicator[01] = "MRI";
                LQ_MaxRateIndicator[02] = Nom.MaxRateIndicator.Trim();
                ediDocument.Segments.Add(LQ_MaxRateIndicator);
            }
            #endregion
            #region N9 Extended reference information
            if (!string.IsNullOrEmpty(Nom.Route))
            {
                var N9_Route = new EdiSegment("N9");
                N9_Route[01] = "RU";
                N9_Route[02] = Nom.Route.Trim();
                ediDocument.Segments.Add(N9_Route);
            }
            if (!string.IsNullOrEmpty(Nom.AssociatedContract))
            {
                var N9_AssociatedContract = new EdiSegment("N9");
                N9_AssociatedContract[01] = "KAS";
                N9_AssociatedContract[02] = Nom.AssociatedContract.Trim();
                ediDocument.Segments.Add(N9_AssociatedContract);
            }
            if (!string.IsNullOrEmpty(Nom.ServiceProviderActivityCode))
            {
                var N9_ServiceProviderActivityCode = new EdiSegment("N9");
                N9_ServiceProviderActivityCode[01] = "BE";
                N9_ServiceProviderActivityCode[02] = Nom.ServiceProviderActivityCode.Trim();
                ediDocument.Segments.Add(N9_ServiceProviderActivityCode);
            }
            if (!string.IsNullOrEmpty(Nom.DealType))
            {
                var N9_DealType = new EdiSegment("N9");
                N9_DealType[01] = "PD";
                N9_DealType[02] = Nom.DealType.Trim();
                ediDocument.Segments.Add(N9_DealType);
            }
            if (!string.IsNullOrEmpty(Nom.NominationUserData1))
            {
                var N9_NominationUserData1 = new EdiSegment("N9");
                N9_NominationUserData1[01] = "JD";
                N9_NominationUserData1[02] = Nom.NominationUserData1.Trim();
                ediDocument.Segments.Add(N9_NominationUserData1);
            }
            if (!string.IsNullOrEmpty(Nom.NominationUserData2))
            {
                var N9_NominationUserData2 = new EdiSegment("N9");
                N9_NominationUserData2[01] = "Y8";
                N9_NominationUserData2[02] = Nom.NominationUserData2.Trim();
                ediDocument.Segments.Add(N9_NominationUserData2);
            }
            if (!string.IsNullOrEmpty(Nom.PathRank))
            {
                var N9_PathRank = new EdiSegment("N9");
                N9_PathRank[01] = "PRK";
                N9_PathRank[02] = Nom.PathRank.Trim();
                ediDocument.Segments.Add(N9_PathRank);
            }
            #endregion
            #region N1 party Identification Upstraem
            if (!string.IsNullOrEmpty(Nom.UpstreamIdentifier))
            {
                var N1_PartyIdentUp = new EdiSegment("N1");
                N1_PartyIdentUp[01] = "US";
                N1_PartyIdentUp[02] = "";
                N1_PartyIdentUp[03] = "1";
                N1_PartyIdentUp[04] = Nom.UpstreamIdentifier.Trim();
                ediDocument.Segments.Add(N1_PartyIdentUp);
            }
            #endregion
            #region LCD Place Location Description Receipt Location 
            if (!string.IsNullOrEmpty(Nom.ReceiptLocationIdentifier))
            {
                var LCD_RecLoc = new EdiSegment("LCD");
                LCD_RecLoc[01] = "1";
                LCD_RecLoc[02] = "M2";
                LCD_RecLoc[03] = "";
                LCD_RecLoc[04] = "";
                LCD_RecLoc[05] = "SV";
                LCD_RecLoc[06] = Nom.ReceiptLocationIdentifier.Trim();
                ediDocument.Segments.Add(LCD_RecLoc);
            }
            #endregion
            #region N9 extended reference information Upstream
            if (!string.IsNullOrEmpty(Nom.UpstreamContractIdentifier))
            {
                var N9_UpConIden = new EdiSegment("N9");
                N9_UpConIden[01] = "UP";
                N9_UpConIden[02] = Nom.UpstreamContractIdentifier.Trim();
                ediDocument.Segments.Add(N9_UpConIden);
            }
            if (!string.IsNullOrEmpty(Nom.UpstreamPackageId))
            {
                var N9_UpPakageID = new EdiSegment("N9");
                N9_UpPakageID[01] = "PKU";
                N9_UpPakageID[02] = Nom.UpstreamPackageId.Trim();
                ediDocument.Segments.Add(N9_UpPakageID);
            }
            #endregion
            #region LQ Industry Code Identification Upstream
            if (!string.IsNullOrEmpty(Nom.UpstreamRank))
            {
                var LQ_UpRank = new EdiSegment("LQ");
                LQ_UpRank[01] = "R1";
                LQ_UpRank[02] = Nom.UpstreamRank.Trim();
                ediDocument.Segments.Add(LQ_UpRank);
            }
            if (!string.IsNullOrEmpty(Nom.ReceiptRank))
            {
                var LQ_recRank = new EdiSegment("LQ");
                LQ_recRank[01] = "R2";
                LQ_recRank[02] = Nom.ReceiptRank.Trim();
                ediDocument.Segments.Add(LQ_recRank);
            }
            #endregion
            #region QTY Quantity delivery
            if (!string.IsNullOrEmpty(Nom.Quantity.ToString()))
            {
                var qty_delQty = new EdiSegment("QTY");
                qty_delQty[01] = "38";
                qty_delQty[02] = Nom.Quantity.ToString();
                qty_delQty[03] = "BZ";
                ediDocument.Segments.Add(qty_delQty);
            }
            #endregion
            #region N1 Party Identification Downstream
            if (!string.IsNullOrEmpty(Nom.DownstreamIdentifier))
            {
                var N1_PartyIdentDwn = new EdiSegment("N1");
                N1_PartyIdentDwn[01] = "DW";
                N1_PartyIdentDwn[02] = "";
                N1_PartyIdentDwn[03] = "1";
                N1_PartyIdentDwn[04] = Nom.DownstreamIdentifier.Trim();
                ediDocument.Segments.Add(N1_PartyIdentDwn);
            }
            #endregion
            #region LCD Place Location Description Delivery Location
            if (!string.IsNullOrEmpty(Nom.DeliveryLocationIdentifer))
            {
                var LCD_DelLoc = new EdiSegment("LCD");
                LCD_DelLoc[01] = "1";
                LCD_DelLoc[02] = "MQ";
                LCD_DelLoc[03] = "";
                LCD_DelLoc[04] = "";
                LCD_DelLoc[05] = "SV";
                LCD_DelLoc[06] = Nom.DeliveryLocationIdentifer.Trim();
                ediDocument.Segments.Add(LCD_DelLoc);
            }
            #endregion
            #region N9 extended reference information Dwnstream
            if (!string.IsNullOrEmpty(Nom.DownstreamContractIdentifier))
            {
                var N9_DwnConIden = new EdiSegment("N9");
                N9_DwnConIden[01] = "DT";
                N9_DwnConIden[02] = Nom.DownstreamContractIdentifier.Trim();
                ediDocument.Segments.Add(N9_DwnConIden);
            }
            if (!string.IsNullOrEmpty(Nom.DownstreamPackageId))
            {
                var N9_DwnPakageID = new EdiSegment("N9");
                N9_DwnPakageID[01] = "PGD";
                N9_DwnPakageID[02] = Nom.DownstreamPackageId.Trim();
                ediDocument.Segments.Add(N9_DwnPakageID);
            }
            #endregion
            #region LQ Industry Code Identification Downstream
            if (!string.IsNullOrEmpty(Nom.DeliveryRank))
            {
                var LQ_delRank = new EdiSegment("LQ");
                LQ_delRank[01] = "R3";
                LQ_delRank[02] = Nom.DeliveryRank.Trim();
                ediDocument.Segments.Add(LQ_delRank);
            }
            if (!string.IsNullOrEmpty(Nom.DownstreamRank))
            {
                var LQ_DwnRank = new EdiSegment("LQ");
                LQ_DwnRank[01] = "R4";
                LQ_DwnRank[02] = Nom.DownstreamRank.Trim();
                ediDocument.Segments.Add(LQ_DwnRank);
            }
            #endregion
            #region QTY Quantity delivery
            if (!string.IsNullOrEmpty(Nom.Quantity.ToString()))
            {
                var qty_delQty = new EdiSegment("QTY");
                qty_delQty[01] = "38";
                qty_delQty[02] = Nom.Quantity.ToString();
                qty_delQty[03] = "BZ";
                ediDocument.Segments.Add(qty_delQty);
            }
            #endregion
            var se = new EdiSegment("SE");
            se[01] = (ediDocument.Segments.Count() - 1).ToString();
            se[02] = st[02];
            ediDocument.Segments.Add(se);

            var ge = new EdiSegment("GE");
            ge[01] = "1";
            ge[02] = gs[06];
            ediDocument.Segments.Add(ge);

            var iea = new EdiSegment("IEA");
            iea[01] = "1";
            iea[02] = number;
            ediDocument.Segments.Add(iea);

            // more segments...
            ediDocument.Options.SegmentTerminator = !string.IsNullOrEmpty(pipelineEdiSetting.SegmentSeperator) ? Convert.ToChar(pipelineEdiSetting.SegmentSeperator) : EdiOptions.DefaultSegmentTerminator;
            ediDocument.Options.ElementSeparator = Convert.ToChar(pipelineEdiSetting.DataSeparator);//'*';// data separator from edi setting
                                                                                                    //ediDocument.Save(filePath.ToString());
            return ediDocument;
        }
        public EdiDocument GenerateEDINonPathNomination(BatchDTO batch,List<V4_NominationDTO> NomList,PipelineEDISettingDTO pipelineEDISetting,bool IsTest)
        {
            string number = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            var ediDocument = new EdiDocument();
            var isa = new EdiSegment("ISA");
            isa[01] = "00";
            isa[02] = "".PadRight(10);
            isa[03] = "00";
            isa[04] = "".PadRight(10);
            isa[05] = "01";
            isa[06] = batch.ServiceRequester.Trim().PadRight(15);
            isa[07] = pipelineEDISetting.ISA08_segment.Trim().All(char.IsDigit) == true ? "01" : "14";
            isa[08] = pipelineEDISetting.ISA08_segment.Trim().PadRight(15);
            isa[09] = EdiValue.Date(6, DateTime.Now);
            isa[10] = EdiValue.Time(4, DateTime.Now);
            isa[11] = pipelineEDISetting.ISA11_Segment.Trim();
            isa[12] = pipelineEDISetting.ISA12_Segment.Trim();
            isa[13] = number;
            isa[14] = "0";
            isa[15] = IsTest ? "T" : "P";
            isa[16] = pipelineEDISetting.ISA16_Segment.Trim();
            ediDocument.Segments.Add(isa);

            var gs = new EdiSegment("GS");
            gs[01] = pipelineEDISetting.GS01_Segment.Trim();
            gs[02] = pipelineEDISetting.GS02_Segment.Trim();
            gs[03] = pipelineEDISetting.GS03_Segment.Trim();
            gs[04] = EdiValue.Date(8, DateTime.Now);
            gs[05] = EdiValue.Time(4, DateTime.Now);
            gs[06] = number.Substring(number.Length - 4, 4);
            gs[07] = pipelineEDISetting.GS07_Segment.Trim();
            gs[08] = pipelineEDISetting.GS08_Segment.Trim();
            ediDocument.Segments.Add(gs);
            
            var st = new EdiSegment("ST");
            st[01] = pipelineEDISetting.ST01_Segment.Trim();
            st[02] = gs[06];
            ediDocument.Segments.Add(st);

            var bgn = new EdiSegment("BGN");
            bgn[01] = "00";
            bgn[02] = batch.ReferenceNumber.ToString();
            bgn[03] = EdiValue.Date(8, DateTime.Now);
            bgn[04] = "";
            bgn[05] = "";
            bgn[06] = "";
            bgn[07] = "G1";
            ediDocument.Segments.Add(bgn);

            var dtmIssue = new EdiSegment("DTM");
            dtmIssue[01] = "102";
            dtmIssue[02] = "";
            dtmIssue[03] = "";
            dtmIssue[04] = "";
            dtmIssue[05] = "DT";
            dtmIssue[06] = EdiValue.Date(8, DateTime.Now) + EdiValue.Time(4, DateTime.Now);
            ediDocument.Segments.Add(dtmIssue);

            var N1_header_ServiceProvider = new EdiSegment("N1");
            N1_header_ServiceProvider[01] = "78";
            N1_header_ServiceProvider[02] = "";
            N1_header_ServiceProvider[03] = "1";
            N1_header_ServiceProvider[04] = batch != null ? batch.pipeDUNSNo.Trim() : string.Empty;
            ediDocument.Segments.Add(N1_header_ServiceProvider);

            var N1_header_ServiceRequester = new EdiSegment("N1");
            N1_header_ServiceRequester[01] = "SJ";
            N1_header_ServiceRequester[02] = "";
            N1_header_ServiceRequester[03] = "1";
            N1_header_ServiceRequester[04] = batch.ServiceRequester.Trim();
            ediDocument.Segments.Add(N1_header_ServiceRequester);

            var dtmEffectiveRD8 = new EdiSegment("DTM");
            dtmEffectiveRD8[01] = "007";
            dtmEffectiveRD8[02] = "";
            dtmEffectiveRD8[03] = "";
            dtmEffectiveRD8[04] = "";
            dtmEffectiveRD8[05] = "RD8";
            dtmEffectiveRD8[06] = batch.DateBeg.ToString("yyyyMMdd") + "-" + batch.DateEnd.ToString("yyyyMMdd");
            ediDocument.Segments.Add(dtmEffectiveRD8);

            if (batch.CycleId >= 1)
            {
                var cycleSegment = new EdiSegment("N9");
                cycleSegment[01] = "CYI";
                cycleSegment[02] = batch.CycleCode;
                ediDocument.Segments.Add(cycleSegment);
            }
            foreach(var Nom in NomList)
            {
                #region SLN segment sublineItem
                var sln_Segment = new EdiSegment("SLN");
                sln_Segment[01] = Nom.NominatorTrackingId.Trim();
                sln_Segment[02] = "";
                sln_Segment[03] = "I";
                ediDocument.Segments.Add(sln_Segment);
                #endregion
                #region LQ segment Industry Code
                
                if (!string.IsNullOrEmpty(Nom.QuantityTypeIndicator))
                {
                    var LQ_QtyTypeInd = new EdiSegment("LQ");
                    LQ_QtyTypeInd[01] = "QT";
                    LQ_QtyTypeInd[02] = Nom.QuantityTypeIndicator.Trim();
                    ediDocument.Segments.Add(LQ_QtyTypeInd);
                }
                
                if (!string.IsNullOrEmpty(Nom.TransactionType))
                {
                    var LQ_TranType = new EdiSegment("LQ");
                    LQ_TranType[01] = "TT";
                    LQ_TranType[02] = Nom.TransactionType.Trim();
                    ediDocument.Segments.Add(LQ_TranType);
                }
                
                if (!string.IsNullOrEmpty(Nom.CapacityTypeIndicator))
                {
                    var LQ_CapTypeInd = new EdiSegment("LQ");
                    LQ_CapTypeInd[01] = "CQ";
                    LQ_CapTypeInd[02] = Nom.CapacityTypeIndicator.Trim();
                    ediDocument.Segments.Add(LQ_CapTypeInd);
                }
                
                if (!string.IsNullOrEmpty(batch.NomSubCycle))
                {
                    var LQ_SubSeqCycleInd = new EdiSegment("LQ");
                    LQ_SubSeqCycleInd[01] = "SCI";
                    LQ_SubSeqCycleInd[02] = batch.NomSubCycle.Trim();
                    ediDocument.Segments.Add(LQ_SubSeqCycleInd);
                }
                
                if (!string.IsNullOrEmpty(Nom.ExportDecleration))
                {
                    var LQ_QtyTypeInd = new EdiSegment("LQ");
                    LQ_QtyTypeInd[01] = "XD";
                    LQ_QtyTypeInd[02] = Nom.ExportDecleration.Trim();
                    ediDocument.Segments.Add(LQ_QtyTypeInd);
                }
                
                if (!string.IsNullOrEmpty(Nom.BidupIndicator))
                {
                    var LQ_BidUpInd = new EdiSegment("LQ");
                    LQ_BidUpInd[01] = "BUI";
                    LQ_BidUpInd[02] = Nom.BidupIndicator.Trim();
                    ediDocument.Segments.Add(LQ_BidUpInd);
                }
                
                if (!string.IsNullOrEmpty(Nom.ProcessingRightIndicator))
                {
                    var LQ_ProcessingRightIndicator = new EdiSegment("LQ");
                    LQ_ProcessingRightIndicator[01] = "PRI";
                    LQ_ProcessingRightIndicator[02] = Nom.ProcessingRightIndicator.Trim();
                    ediDocument.Segments.Add(LQ_ProcessingRightIndicator);
                }
                
                if (!string.IsNullOrEmpty(Nom.MaxRateIndicator))
                {
                    var LQ_MaxRateIndicator = new EdiSegment("LQ");
                    LQ_MaxRateIndicator[01] = "MRI";
                    LQ_MaxRateIndicator[02] = Nom.MaxRateIndicator.Trim();
                    ediDocument.Segments.Add(LQ_MaxRateIndicator);
                }
                #endregion
                #region N9 Extended reference information
                #endregion
                #region N1 party Identification Upstraem
                if (!string.IsNullOrEmpty(Nom.UpstreamIdentifier))
                {
                    var N1_PartyIdentUp = new EdiSegment("N1");
                    N1_PartyIdentUp[01] = "US";
                    N1_PartyIdentUp[02] = "";
                    N1_PartyIdentUp[03] = "1";
                    N1_PartyIdentUp[04] = Nom.UpstreamIdentifier.Trim();
                    ediDocument.Segments.Add(N1_PartyIdentUp);
                }
                #endregion
                #region LCD Place Location Description Receipt Location 
                if (!string.IsNullOrEmpty(Nom.ReceiptLocationIdentifier))
                {
                    var LCD_RecLoc = new EdiSegment("LCD");
                    LCD_RecLoc[01] = "1";                    
                    LCD_RecLoc[02] = "M2";
                    LCD_RecLoc[03] = "";
                    LCD_RecLoc[04] = "";
                    LCD_RecLoc[05] = "SV";
                    LCD_RecLoc[06] = Nom.ReceiptLocationIdentifier.Trim();
                    ediDocument.Segments.Add(LCD_RecLoc);
                }
                #endregion
                #region N9 extended reference information Upstream
                if (!string.IsNullOrEmpty(Nom.UpstreamContractIdentifier))
                {
                    var N9_UpConIden = new EdiSegment("N9");
                    N9_UpConIden[01] = "UP";
                    N9_UpConIden[02] = Nom.UpstreamContractIdentifier.Trim();
                    ediDocument.Segments.Add(N9_UpConIden);
                }
                if (!string.IsNullOrEmpty(Nom.UpstreamPackageId))
                {
                    var N9_UpPakageID = new EdiSegment("N9");
                    N9_UpPakageID[01] = "PKU";
                    N9_UpPakageID[02] = Nom.UpstreamPackageId.Trim();
                    ediDocument.Segments.Add(N9_UpPakageID);
                }
                #endregion
                #region LQ Industry Code Identification Upstream
                if (!string.IsNullOrEmpty(Nom.UpstreamRank))
                {
                    var LQ_UpRank = new EdiSegment("LQ");
                    LQ_UpRank[01] = "R1";
                    LQ_UpRank[02] = Nom.UpstreamRank.Trim();
                    ediDocument.Segments.Add(LQ_UpRank);
                }
                if (!string.IsNullOrEmpty(Nom.ReceiptRank))
                {
                    var LQ_recRank = new EdiSegment("LQ");
                    LQ_recRank[01] = "R2";
                    LQ_recRank[02] = Nom.ReceiptRank.Trim();
                    ediDocument.Segments.Add(LQ_recRank);
                }
                #endregion
                #region QTY Quantity delivery
                if (Nom.PathType.Trim() == "NPR")
                {
                    if (!string.IsNullOrEmpty(Nom.Quantity.ToString()))
                    {
                        var qty_delQty = new EdiSegment("QTY");
                        qty_delQty[01] = "QD";
                        qty_delQty[02] = Nom.Quantity.ToString();
                        qty_delQty[03] = "BZ";
                        ediDocument.Segments.Add(qty_delQty);
                    }
                }
                #endregion
                #region N1 Party Identification Downstream
                if (!string.IsNullOrEmpty(Nom.DownstreamIdentifier))
                {
                    var N1_PartyIdentDwn = new EdiSegment("N1");
                    N1_PartyIdentDwn[01] = "DW";
                    N1_PartyIdentDwn[02] = "";
                    N1_PartyIdentDwn[03] = "1";
                    N1_PartyIdentDwn[04] = Nom.DownstreamIdentifier.Trim();
                    ediDocument.Segments.Add(N1_PartyIdentDwn);
                }
                #endregion
                #region LCD Place Location Description Delivery Location
                if (!string.IsNullOrEmpty(Nom.DeliveryLocationIdentifer))
                {
                    var LCD_DelLoc = new EdiSegment("LCD");
                    LCD_DelLoc[01] = "1";
                    LCD_DelLoc[02] = "MQ";
                    LCD_DelLoc[03] = "";
                    LCD_DelLoc[04] = "";
                    LCD_DelLoc[05] = "SV";
                    LCD_DelLoc[06] = Nom.DeliveryLocationIdentifer.Trim();
                    ediDocument.Segments.Add(LCD_DelLoc);
                }
                #endregion
                #region N9 extended reference information Dwnstream
                if (!string.IsNullOrEmpty(Nom.DownstreamContractIdentifier))
                {
                    var N9_DwnConIden = new EdiSegment("N9");
                    N9_DwnConIden[01] = "DT";
                    N9_DwnConIden[02] = Nom.DownstreamContractIdentifier.Trim();
                    ediDocument.Segments.Add(N9_DwnConIden);
                }
                if (!string.IsNullOrEmpty(Nom.DownstreamPackageId))
                {
                    var N9_DwnPakageID = new EdiSegment("N9");
                    N9_DwnPakageID[01] = "PGD";
                    N9_DwnPakageID[02] = Nom.DownstreamPackageId.Trim();
                    ediDocument.Segments.Add(N9_DwnPakageID);
                }
                #endregion
                #region LQ Industry Code Identification Downstream
                if (!string.IsNullOrEmpty(Nom.DeliveryRank))
                {
                    var LQ_delRank = new EdiSegment("LQ");
                    LQ_delRank[01] = "R3";
                    LQ_delRank[02] = Nom.DeliveryRank.Trim();
                    ediDocument.Segments.Add(LQ_delRank);
                }
                if (!string.IsNullOrEmpty(Nom.DownstreamRank))
                {
                    var LQ_DwnRank = new EdiSegment("LQ");
                    LQ_DwnRank[01] = "R4";
                    LQ_DwnRank[02] = Nom.DownstreamRank.Trim();
                    ediDocument.Segments.Add(LQ_DwnRank);
                }
                #endregion
                #region QTY DelQuantity delivery
                if (Nom.PathType.Trim() == "NPD")
                {
                    if (!string.IsNullOrEmpty(Nom.DelQuantity.ToString()))
                    {
                        var qty_delQty = new EdiSegment("QTY");
                        qty_delQty[01] = "38";
                        qty_delQty[02] = Nom.DelQuantity.ToString();
                        qty_delQty[03] = "BZ";
                        ediDocument.Segments.Add(qty_delQty);
                    }
                }
                #endregion
            }
            
            var se = new EdiSegment("SE");
            se[01] = (ediDocument.Segments.Count() - 1).ToString();
            se[02] = st[02];
            ediDocument.Segments.Add(se);

            var ge = new EdiSegment("GE");
            ge[01] = "1";
            ge[02] = gs[06];
            ediDocument.Segments.Add(ge);

            var iea = new EdiSegment("IEA");
            iea[01] = "1";
            iea[02] = number;
            ediDocument.Segments.Add(iea);
            

            ediDocument.Options.SegmentTerminator = !string.IsNullOrEmpty(pipelineEDISetting.SegmentSeperator) ? Convert.ToChar(pipelineEDISetting.SegmentSeperator) : EdiOptions.DefaultSegmentTerminator;
            ediDocument.Options.ElementSeparator = Convert.ToChar(pipelineEDISetting.DataSeparator);

            return ediDocument;
        }
    }
}
