using EdiTools;
using EDITranslation;
using EDITranslation.AdditionalStandards;
using Nom1Done.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nom1Done.ReceiveUI.Controllers
{
    public class ManualEdiController : Controller
    {
        // GET: ManualEdi
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase postedFile)
        {
            try
            {
                string filePath = string.Empty;
                if (postedFile != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    //Read the contents of CSV file.

                    string ediFile = System.IO.File.ReadAllText(filePath);
                    var dataSeperator = Convert.ToChar(ediFile[(ediFile.IndexOf("ISA") + "ISA".Length)]);
                    var segmentSeperator = Convert.ToChar(ediFile[(ediFile.IndexOf("GS") - 1)]);



                    OACY_DS ediOacy = new OACY_DS(ediFile
                , new char[] { segmentSeperator }//segement seperators
                , new char[] { dataSeperator }
                , "test");
                    var fg = ediOacy.ReadOacyFile();




                    EDIWrapperBase ediWrapper = new EDIWrapperBase(ediFile
                            , new char[] { segmentSeperator }//segement seperators
                            , new char[] { dataSeperator });//Dataseperators
                    List<string[]> separateSTFiles = ediWrapper.EnvelopeBlocks(EDIEnvelopeNodes.ST, EDIEnvelopeNodes.SE);

                    foreach (string[] separateFile in separateSTFiles)
                    {
                        string subEDIfile = "";
                        foreach (string element in separateFile)
                        {
                            subEDIfile = subEDIfile + element + Convert.ToChar(segmentSeperator);
                        }

                        EDIWrapperBase subEdiWrapper = new EDIWrapperBase(subEDIfile
                            , new char[] { segmentSeperator }//segement seperators
                            , new char[] { dataSeperator });//Dataseperators

                        EDIFileType fileType = subEdiWrapper.FileType;
                        switch (fileType)
                        {
                            case EDIFileType.NMQR:
                                //inbox.DatasetID = (int)DTO.DataSet.Nomination_Quick_Response;
                                //pendingJob.DatasetId = inbox.DatasetID;
                                //_serviceInbox.Update(inbox);
                                //_serviceInbox.Save();
                                //_serviceTaskMgrJob.Update(pendingJob);
                                //_serviceTaskMgrJob.Save();
                                //isProcessed = ProcessNmqrData(subEDIfile, pendingJob.TransactionId, inbox.PipelineID, dataSeperator, segmentSeperator);
                                break;
                            case EDIFileType.OACY:
                                //inbox.DatasetID = (int)DTO.DataSet.Operational_Capacity;
                                //pendingJob.DatasetId = inbox.DatasetID;
                                //_serviceInbox.Update(inbox);
                                //_serviceInbox.Save();
                                //_serviceTaskMgrJob.Update(pendingJob);
                                //_serviceTaskMgrJob.Save();
                                //isProcessed = ProcessOacyData(subEDIfile, pendingJob.TransactionId, inbox.PipelineID, dataSeperator, segmentSeperator);
                                break;
                            case EDIFileType.UNSC:
                                //inbox.DatasetID = (int)DTO.DataSet.Unsubscribed_Capacity;
                                //pendingJob.DatasetId = inbox.DatasetID;
                                //_serviceInbox.Update(inbox);
                                //_serviceInbox.Save();
                                //_serviceTaskMgrJob.Update(pendingJob);
                                //_serviceTaskMgrJob.Save();
                                //isProcessed = ProcessUnscData(subEDIfile, pendingJob.TransactionId, inbox.PipelineID, dataSeperator, segmentSeperator);
                                break;
                            case EDIFileType.RURD:
                                //inbox.DatasetID = (int)DTO.DataSet.Response_to_Upload_of_Request_for_Download;
                                //pendingJob.DatasetId = inbox.DatasetID;
                                //_serviceInbox.Update(inbox);
                                //_serviceInbox.Save();
                                //_serviceTaskMgrJob.Update(pendingJob);
                                //_serviceTaskMgrJob.Save();
                                //isProcessed = ProcessRurdData(subEDIfile, pendingJob.TransactionId, inbox.PipelineID, dataSeperator, segmentSeperator);
                                break;
                            case EDIFileType.SWNT:
                                //inbox.DatasetID = (int)DTO.DataSet.System_Wide_Notices;
                                //pendingJob.DatasetId = inbox.DatasetID;
                                //_serviceInbox.Update(inbox);
                                //_serviceInbox.Save();
                                //_serviceTaskMgrJob.Update(pendingJob);
                                //_serviceTaskMgrJob.Save();
                                //isProcessed = ProcessSwntData(subEDIfile, pendingJob.TransactionId, inbox.PipelineID, dataSeperator, segmentSeperator);
                                break;
                            case EDIFileType.SQTS:
                                //inbox.DatasetID = (int)DTO.DataSet.Scheduled_Quantity;
                                //pendingJob.DatasetId = inbox.DatasetID;
                                //_serviceInbox.Update(inbox);
                                //_serviceInbox.Save();
                                //_serviceTaskMgrJob.Update(pendingJob);
                                //_serviceTaskMgrJob.Save();
                                //isProcessed = ProcessSqtsData(subEDIfile, pendingJob.TransactionId, inbox.PipelineID, dataSeperator, segmentSeperator);
                                break;
                            case EDIFileType.Ack:
                                //inbox.DatasetID = (int)DTO.DataSet.Ack_997;
                                //pendingJob.DatasetId = inbox.DatasetID;
                                //_serviceInbox.Update(inbox);
                                //_serviceInbox.Save();
                                //_serviceTaskMgrJob.Update(pendingJob);
                                //_serviceTaskMgrJob.Save();
                                break;
                            case EDIFileType.Unknown:
                                //inbox.DatasetID = 99;
                                //pendingJob.DatasetId = inbox.DatasetID;
                                //_serviceInbox.Update(inbox);
                                //_serviceInbox.Save();
                                //_serviceTaskMgrJob.Update(pendingJob);
                                //_serviceTaskMgrJob.Save();
                                break;
                        }
                    }




                    //EdiOptions options = new EdiOptions();
                    //options.ComponentSeparator = '>';
                    //options.ElementSeparator = dataSeperator;
                    //options.SegmentTerminator = segmentSeperator;

                    //EdiDocument ediDocument = new EdiDocument(ediFile,options);


                    //var startNodeIndexes = Enumerable.Range(0, ediDocument.Segments.Count())
                    // .Where(i => ediDocument.Segments[i].Id.StartsWith("ST"))
                    // .ToList();

                    //var endingNodeIdexes = Enumerable.Range(0, ediDocument.Segments.Count())
                    //    .Where(i => ediDocument.Segments[i].Id.StartsWith("SE"))
                    //    .ToList();

                    //if (startNodeIndexes.Count != endingNodeIdexes.Count)
                    //{
                    //    //ST and SE don't match 
                    //}
                    //else
                    //{
                    //    List<OACYPerTransaction> list = new List<OACYPerTransaction>();
                    //    for (int i = 0; i < startNodeIndexes.Count; i++)
                    //    {
                    //        int countMatch = (endingNodeIdexes[i] - startNodeIndexes[i]) + 1;
                    //        var singleExtractedFile = ediDocument.Segments.ToList().GetRange(startNodeIndexes[i], countMatch);
                    //        var fileType = CheckEDIFileType(singleExtractedFile);
                    //        switch (fileType)
                    //        {
                    //            case EDIFileType.OACY:
                    //                ProcessOacyFiles(singleExtractedFile);
                    //                break;
                    //            case EDIFileType.UNSC:
                    //                ProcessUNSCFiles(singleExtractedFile);
                    //                break;
                    //            case EDIFileType.RURD:
                    //                ProcessRURDFiles(singleExtractedFile);
                    //                break;
                    //            case EDIFileType.SWNT:
                    //                ProcessSWNTFiles(singleExtractedFile);
                    //                break;
                    //            case EDIFileType.NMQR:
                    //                ProcessNMQRFiles(singleExtractedFile);
                    //                break;
                    //            case EDIFileType.SQTS:
                    //                ProcessSQTSFiles(singleExtractedFile);
                    //                break;
                    //            case EDIFileType.Ack:
                    //                break;
                    //            case EDIFileType.Unknown:
                    //                break;
                    //            default:
                    //                break;
                    //        }
                    //    }
                    //}


                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "File upload failed!!";
            }
            return View();
        }

        private EDIFileType CheckEDIFileType(List<EdiSegment> ediFile)
        {
            EDIFileType fileType = EDIFileType.Unknown;
            string fileCode = ediFile[0][1]; // ST 1
            switch (fileCode)
            {
                case "846":
                    fileType = EDIFileType.RURD;
                    break;
                case "873":
                    string capFileCode = ediFile[1][7]; // BGN 7
                    switch (capFileCode)
                    {
                        case "US":
                            fileType = EDIFileType.UNSC;
                            break;
                        case "OA":
                            fileType = EDIFileType.OACY;
                            break;
                        case "Q1":
                            fileType = EDIFileType.SQTS;
                            break;
                    }
                    break;
                case "864":
                    fileType = EDIFileType.SWNT;
                    break;
                case "997":
                    fileType = EDIFileType.Ack;
                    break;
                case "874":
                    fileType = EDIFileType.NMQR;
                    break;
            }
            return fileType;
        }


        public void ProcessOacyFiles(List<EdiSegment> ediFile)
        {
            OACYPerTransaction model = new OACYPerTransaction();
            foreach (var item in ediFile)
            {
                
            }
        }

        public void ProcessUNSCFiles(List<EdiSegment> ediFile)
        {
            
            foreach (var item in ediFile)
            {
                UnscPerTransaction model = new UnscPerTransaction();

            }
        }

        public void ProcessRURDFiles(List<EdiSegment> ediFile)
        {
            foreach (var item in ediFile)
            {
                
            }
        }

        public void ProcessSWNTFiles(List<EdiSegment> ediFile)
        {
            SwntPerTransaction model = new SwntPerTransaction();
            foreach (var item in ediFile)
            {
               // model.TransactionIdentifierCode = item.Elements item.Elements[0].ToString();
                model.TransactionControlNumber = item.Elements[1].ToString();
                //model.PipelineId=item.Elements[]
            }
        }

        public void ProcessNMQRFiles(List<EdiSegment> ediFile)
        {
            foreach (var item in ediFile)
            {

            }
        }

        public void ProcessSQTSFiles(List<EdiSegment> ediFile)
        {
            foreach (var item in ediFile)
            {

            }
        }
    }

}