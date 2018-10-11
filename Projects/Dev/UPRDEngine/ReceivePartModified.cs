using EDITranslation;
using EDITranslation.AdditionalStandards;
using EDITranslation.CapacityRelease;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UPRD.Data.Repositories;
using UPRD.Enums;
using UPRD.Infrastructure;
using UPRD.Model;
using UPRD.Service;
using UPRD.Service.Interface;

namespace UPRDEngine.EDIEngineSendAndReceive
{
    public class ReceivePartModified
    {
        #region common services
        IUprdSettingRepository _serviceSettings;
        IUprdTaskMgrJobsRepository _serviceTaskMgrJob;
        IUprdTaskMgrXmlRepository _serviceTaskMgrXml;
        IUprdJobWorkflowRepository _serviceJobWorkFlow;
        IUprdIncomingDataRepository _serviceIncomingData;
        IUprdFileSysIncomingDataRepository _serviceFileSysIncomingData;
        IUprdJobStackErrorLogRepository _serviceJobStackErrorLog;
        IUprdTransactionLogRepository _serviceTransactionLog;
        IUprdPipelineRepository _servicePipeline;
        IUprdmetadataDatasetRepository _serviceDataset;
        IUprdInboxRepository _serviceInbox;
        IUprdStatuRepository _serviceUPRDStatus;
        IUPRDApplicationLogRepository _applicationLogs;
        IUnitOfWork unitOfWork;
        IModalFactory modalFactory;
        IUprdShipperCompanyRepository _serviceShipperCompany;
        #endregion
        #region Oacy services
        IUprdOACYRepository _serviceOacyPerTransaction;
        #endregion
        #region Unsc services
        IUprdUNSCRepository _serviceUnscPerTransaction;
        #endregion
        #region SWNT Services
        IUprdSWNTPerTransactionRepository _serviceSwntPerTransaction;
        #endregion
        #region Email Services
        IUprdEmailTemplateRepository emailTempalteService;
        IUprdEmailQueueRepository emailQueueService;
        IUPRDApplicationLogRepository appLogService;
        #endregion
        public ReceivePartModified()
        {
            StandardKernel Kernal = new StandardKernel();
            Kernal.Load(Assembly.GetExecutingAssembly());
            _serviceSettings = Kernal.Get<UprdSettingRepository>();
            _serviceTaskMgrJob = Kernal.Get<UprdTaskMgrJobsRepository>();
            _serviceTaskMgrXml = Kernal.Get<UprdTaskMgrXmlRepository>();
            _serviceJobWorkFlow = Kernal.Get<UprdJobWorkflowRepository>();
            _serviceIncomingData = Kernal.Get<UprdIncomingDataRepository>();
            _serviceJobStackErrorLog = Kernal.Get<UprdJobStackErrorLogRepository>();
            _serviceTransactionLog = Kernal.Get<UprdTransactionLogRepository>();
            _serviceInbox = Kernal.Get<UprdInboxRepository>();
            _servicePipeline = Kernal.Get<UprdPipelineRepository>();
            _serviceDataset = Kernal.Get<UprdmetadataDatasetRepository>();
            _serviceUPRDStatus = Kernal.Get<UprdStatuRepository>();
            _applicationLogs = Kernal.Get<UPRDApplicationLogRepository>();
            unitOfWork = Kernal.Get<UnitOfWork>();
            modalFactory = Kernal.Get<ModalFactory>();
            _serviceShipperCompany = Kernal.Get<UprdShipperCompanyRepository>();
            _serviceFileSysIncomingData = Kernal.Get<UprdFileSysIncomingDataRepository>();
            #region get oacy services
            _serviceOacyPerTransaction = Kernal.Get<UprdOACYRepository>();
            #endregion
            #region get unsc services
            _serviceUnscPerTransaction = Kernal.Get<UprdUNSCRepository>();
            #endregion
            #region get swnt services
            _serviceSwntPerTransaction = Kernal.Get<UprdSWNTPerTransactionRepository>();
            #endregion
            #region get email service
            emailTempalteService = Kernal.Get<UprdEmailTemplateRepository>();
            emailQueueService = Kernal.Get<UprdEmailQueueRepository>();
            appLogService = Kernal.Get<UPRDApplicationLogRepository>();
            #endregion
        }
        public void ProcessFiles(bool TryAgain)
        {
            Console.Clear();
            Console.WriteLine("Processing files");

            try
            {
                var releaseToShipper = _serviceSettings.GetById((int)Settings.releaseToShipper).Value;
                string crashFilesPath = _serviceSettings.GetById((int)Settings.CrashFilesPath).Value;
                var AckFiles = _serviceSettings.GetById((int)Settings.AckFiles).Value;
                var UnknownFiles = _serviceSettings.GetById((int)Settings.UnknownFiles).Value;
                string EngineFilePath = string.Empty;
                if (TryAgain)
                    EngineFilePath = crashFilesPath + @"\TryAgain";// _serviceSettings.GetById((int)Settings.TryAgainFilePath).Value;
                else
                    EngineFilePath = _serviceSettings.GetById((int)Settings.EngineFilesPath).Value;

                var inFileList = Directory.GetFiles(EngineFilePath);
                if (inFileList != null && inFileList.Count() > 0)
                {
                    List<SwntPerTransaction> SwntList = new List<SwntPerTransaction>();
                    List<OACYPerTransaction> OacyList = new List<OACYPerTransaction>();
                    List<UnscPerTransaction> UnscList = new List<UnscPerTransaction>();
                    List<UPRDStatus> UprdStatusList = new List<UPRDStatus>();
                    List<FileSysIncomingData> incominDataList = new List<FileSysIncomingData>();
                    bool IsProcessed = false;
                    Parallel.ForEach(inFileList, (file) =>
                    {
                        string fileText = string.Empty;
                        try
                        {
                            FileInfo fileInfo = new FileInfo(file);
                            if (File.Exists(file) && !IsFileLocked(fileInfo))
                            {
                                var currentfile = new FileSysIncomingData();
                                currentfile.MessageId = Path.GetFileNameWithoutExtension(file);
                                fileText = File.ReadAllText(file);
                                File.Delete(file);
                                currentfile.DecryptedData = fileText;
                                StringBuilder decryptedEDI = null;
                                decryptedEDI = new StringBuilder(currentfile.DecryptedData);
                                int pos = decryptedEDI.ToString().IndexOf("ISA");
                                var data = decryptedEDI.Remove(0, pos);
                                string pipelineDUNS = "";
                                pipelineDUNS = data.ToString().Substring(35, 9);

                                currentfile.DecryptedData = decryptedEDI.ToString();
                                currentfile.PipeDuns = pipelineDUNS;

                                //Start Processing
                                char dataSeperator, segmentSeperator;
                                dataSeperator = Convert.ToChar(decryptedEDI.ToString()[(decryptedEDI.ToString().IndexOf("ISA") + "ISA".Length)]);
                                segmentSeperator = Convert.ToChar(decryptedEDI.ToString()[(decryptedEDI.ToString().IndexOf("GS") - 1)]);
                                
                                if (dataSeperator != '\0' || segmentSeperator != '\0')
                                {
                                    EDIWrapperBase ediWrapper = new EDIWrapperBase(decryptedEDI.ToString()
                                        , new char[] { segmentSeperator }
                                        , new char[] { dataSeperator });
                                    List<string[]> separateSTFiles = ediWrapper.EnvelopeBlocks(EDIEnvelopeNodes.ST, EDIEnvelopeNodes.SE);

                                    foreach (string[] separateFile in separateSTFiles)
                                    {
                                        string subEDIfile = "";
                                        foreach (string element in separateFile)
                                        {
                                            subEDIfile = subEDIfile + element + Convert.ToChar(segmentSeperator);
                                        }
                                        if (!string.IsNullOrEmpty(subEDIfile))
                                        {
                                            EDIWrapperBase subEdiWrapper = new EDIWrapperBase(subEDIfile
                                            , new char[] { segmentSeperator }//segement seperators
                                            , new char[] { dataSeperator });//Dataseperators

                                            EDIFileType fileType = subEdiWrapper.FileType;
                                            switch (fileType)
                                            {
                                                case EDIFileType.OACY:
                                                    currentfile.DatasetType = "OACY";
                                                    OacyList.AddRange(FillUpOACYData(subEDIfile, currentfile.MessageId, dataSeperator, segmentSeperator));
                                                    IsProcessed = true;
                                                    break;
                                                case EDIFileType.UNSC:
                                                    currentfile.DatasetType = "UNSC";
                                                    UnscList.AddRange(FillUpUNSCData(subEDIfile, currentfile.MessageId, dataSeperator, segmentSeperator));
                                                    IsProcessed = true;
                                                    break;
                                                case EDIFileType.RURD:
                                                    currentfile.DatasetType = "RURD";
                                                    UprdStatusList.AddRange(FillUpRURDData(subEDIfile, currentfile.MessageId, dataSeperator, segmentSeperator));
                                                    IsProcessed = true;
                                                    break;
                                                case EDIFileType.SWNT:
                                                    currentfile.DatasetType = "SWNT";
                                                    SwntList.AddRange(FillUpSWNTData(subEDIfile, currentfile.MessageId, dataSeperator, segmentSeperator));
                                                    IsProcessed = true;
                                                    break;
                                                case EDIFileType.Ack:
                                                    IsProcessed = false;
                                                    currentfile.DatasetType = "ACK";
                                                    if (!Directory.Exists(AckFiles))
                                                    {
                                                        Directory.CreateDirectory(AckFiles);
                                                    }
                                                    var AckFilePath = AckFiles + "/" + currentfile.MessageId + ".txt";
                                                    File.WriteAllText(AckFilePath, decryptedEDI.ToString());
                                                    break;
                                                case EDIFileType.Unknown:
                                                    IsProcessed = false;
                                                    currentfile.DatasetType = "UNK";
                                                    if (!Directory.Exists(UnknownFiles))
                                                    {
                                                        Directory.CreateDirectory(UnknownFiles);
                                                    }
                                                    var UnknownFilePath = UnknownFiles + "/" + currentfile.MessageId + ".txt";
                                                    File.WriteAllText(UnknownFilePath, decryptedEDI.ToString());
                                                    break;
                                            }
                                        }
                                    }
                                }
                                currentfile.ReceivedAt = DateTime.Now;
                                incominDataList.Add(currentfile);
                            }
                        }
                        catch (Exception ex)
                        {
                            string storeFilePath = string.Empty;
                            var name=Path.GetFileNameWithoutExtension(file);
                            string data = fileText;                            
                            if (File.Exists(file))
                            {
                                data = File.ReadAllText(file);
                            }
                            IsProcessed = false;
                            Console.WriteLine("crash");
                            
                            if (TryAgain)
                            {
                                storeFilePath = crashFilesPath + @"\CrashAgain\";
                                var EmailTo = new string[] { "jay.singh@enercross.com", "lakhwinder.singh@fifthnote.co", "gagandeep.singh@fifthnote.co" };
                                string subject = "Crashed File " + "(" + ConfigurationManager.AppSettings["Environment"] + ")";
                                string fromDisplay = "NatGasHub";
                                string Body = "Please find the attached crashed file.";
                                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(data));
                                Attachment attachments = new Attachment(stream, name + ".txt", System.Net.Mime.MediaTypeNames.Text.Plain);
                                EmailService.SendEmailAsync(EmailTo, Body, subject, fromDisplay, attachments);
                                stream.Dispose();
                            }
                            else
                            {
                                storeFilePath = crashFilesPath + @"\TryAgain\";
                            }

                            if (!Directory.Exists(storeFilePath))
                            {
                                Directory.CreateDirectory(storeFilePath);
                            }
                            storeFilePath = storeFilePath + name + ".txt";
                            File.WriteAllText(storeFilePath, data + Environment.NewLine + ex.Message);
                            
                        }
                    });
                    
                    bool IsDataEntered = false;

                    if (IsProcessed)
                    {
                        if (SwntList.Count > 0)
                        {
                            IsDataEntered = _serviceSwntPerTransaction.SaveSwntList(SwntList);
                        }
                        if (UnscList.Count > 0)
                        {
                            IsDataEntered = _serviceUnscPerTransaction.SaveUnscList(UnscList);
                        }
                        if (OacyList.Count > 0)
                        {
                            IsDataEntered = _serviceOacyPerTransaction.SaveOacyList(OacyList);
                        }
                        if (UprdStatusList.Count > 0)
                            IsDataEntered = ProcessRurdData(UprdStatusList);
                        if (incominDataList.Count > 0)
                            _serviceFileSysIncomingData.AddListOfFileSysIncomingData(incominDataList);
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogManager("ReceivePartFileInsertion", "Error", ex.ToString());
            }
        }
        #region get file is in use or not
        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }
        #endregion


        #region data manupulation from file
        private List<OACYPerTransaction> FillUpOACYData(string ediFile, string fileName, char dataSeperator, char segmentSeperator)
        {
            string pipelineDUNS = ediFile.Substring(35, 9);

            OACY_DS ediOacy = new OACY_DS(ediFile
                , new char[] { segmentSeperator }//segement seperators
                , new char[] { dataSeperator }
                , fileName);
            return modalFactory.Create(ediOacy.ReadOacyFile());
        }
        
        private List<UnscPerTransaction> FillUpUNSCData(string ediFile, string fileName, char dataSeperator, char segmentSeperator)
        {
            string pipelineDUNS = ediFile.Substring(35, 9);

            UNSC_DS ediUnsc = new UNSC_DS(ediFile
                , new char[] { segmentSeperator }//segement seperators
                , new char[] { dataSeperator }
                , fileName);
            return modalFactory.Create(ediUnsc.ReadUnscFile());
        }
        private List<SwntPerTransaction> FillUpSWNTData(string ediFile, string fileName, char dataSeperator, char segmentSeperator)
        {
            string pipelineDUNS = ediFile.Substring(35, 9);
            SWNT_DS ediSWNT = new SWNT_DS(ediFile
                , new char[] { segmentSeperator }//segement seperators
                , new char[] { dataSeperator }
                , fileName);
            return modalFactory.Create(ediSWNT.ReadSwntFile());
        }
        private List<UPRDStatus> FillUpRURDData(string ediFile, string fileName, char dataSeperator, char segmentSeperator)
        {
            RURD_DS ediRURD = new RURD_DS(ediFile
                , new char[] { segmentSeperator }//segement seperators
                , new char[] { dataSeperator }
                , fileName);
            return modalFactory.Create(ediRURD.ReadRurdFile());
        }
        #endregion
        #region Data insert into db from EDI file
        private bool ProcessRurdData(List<UPRDStatus> uprdStatusList)
        {
            try
            {
                foreach (var uprdSatus in uprdStatusList)
                {
                    var uprdReq = _serviceUPRDStatus.GetUprdStatus(uprdSatus.RequestID);
                    if (uprdReq != null)
                    {
                        uprdReq.RURD_ID = uprdSatus.RURD_ID;
                        uprdReq.DatasetSummary = uprdSatus.DatasetSummary;
                        uprdReq.IsRURDReceived = uprdSatus.IsRURDReceived;
                        uprdReq.IsDataSetAvailable = uprdSatus.IsDataSetAvailable;
                        _serviceUPRDStatus.Update(uprdReq);
                        _serviceUPRDStatus.Save();
                        return true;
                    }
                    else
                        return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:-  " + ex.Message);
                return false;
            }
        }
        #endregion
        private void AppLogManager(string source, string type, string errMsg)
        {
            ApplicationLog log = new ApplicationLog();
            log.Source = source.ToString();
            log.Type = type;
            log.Description = errMsg.ToString();
            log.CreatedDate = DateTime.Now;
            _applicationLogs.Add(log);
            _applicationLogs.Save();
        }
    }
}
