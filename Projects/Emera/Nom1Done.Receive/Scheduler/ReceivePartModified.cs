using EDITranslation;
using EDITranslation.AdditionalStandards;
using EDITranslation.CapacityRelease;
using EDITranslation.Nominations;
using Ninject;
using Nom1Done.Data.Repositories;
using Nom1Done.DTO;
using Nom1Done.Infrastructure;
using Nom1Done.Model;
using Nom1Done.Service;
using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Xml;

namespace Nom1Done.Receive.Scheduler
{
    public class ReceivePartModified
    {
        #region common services
        ISettingRepository _serviceSettings;
        ITaskMgrJobsRepository _serviceTaskMgrJob;
        ITaskMgrXmlRepository _serviceTaskMgrXml;
        IJobWorkflowRepository _serviceJobWorkFlow;
        IIncomingDataRepository _serviceIncomingData;
        IFileSysIncomingDataRepository _serviceFileSysIncomingData;
        IJobStackErrorLogRepository _serviceJobStackErrorLog;
        ITransactionLogRepository _serviceTransactionLog;
        IPipelineRepository _servicePipeline;
        ImetadataDatasetRepository _serviceDataset;
        IInboxRepository _serviceInbox;
        INominationStatusRepository _serviceNomStatus;
        IApplicationLogRepository _applicationLogs;
        IBatchRepository _serviceBatch;
        INominationsRepository _serviceNomination;
        ImetadataCycleRepository _serviceMetadatCycle;
        IUnitOfWork unitOfWork;
        IModalFactory modalFactory;
        IShipperCompanyRepository _serviceShipperCompany;
        #endregion
        #region NMQR Services
        INMQRPerTransactionRepository _serviceNmqrPerTransaction;
        #endregion
        #region SQTS Services
        ISQTSTrackOnNomRepository _serviceSqtsTrackOnNom;
        ISQTSFileRepository _serviceSqtsFile;
        ISQTSPerTransactionRepository _serviceSqtsPerTransaction;
        ISQTSOPPerTransactionRepository _serviceSqtsOpPerTransaction;
        #endregion
        #region Email Services
        IEmailTemplateRepository emailTempalteService;
        IEmailQueueRepository emailQueueService;
        IApplicationLogRepository appLogService;
        #endregion
        #region Sqts Reduction Reason lookup
        //XmlNode redResonNode = null;
        IEnumerable<XmlNode> redReasonList = null;
        #endregion
        public ReceivePartModified()
        {
            StandardKernel Kernal = new StandardKernel();
            Kernal.Load(Assembly.GetExecutingAssembly());
            _serviceSettings = Kernal.Get<SettingRepository>();
            _serviceTaskMgrJob = Kernal.Get<TaskMgrJobsRepository>();
            _serviceTaskMgrXml = Kernal.Get<TaskMgrXmlRepository>();
            _serviceJobWorkFlow = Kernal.Get<JobWorkflowRepository>();
            _serviceIncomingData = Kernal.Get<IncomingDataRepository>();
            _serviceJobStackErrorLog = Kernal.Get<JobStackErrorLogRepository>();
            _serviceTransactionLog = Kernal.Get<TransactionLogRepository>();
            _serviceInbox = Kernal.Get<InboxRepository>();
            _servicePipeline = Kernal.Get<PipelineRepository>();
            _serviceDataset = Kernal.Get<metadataDatasetRepository>();
            _serviceNomStatus = Kernal.Get<NominationStatusRepository>();
            _applicationLogs = Kernal.Get<ApplicationLogRepository>();
            _serviceBatch = Kernal.Get<BatchRepository>();
            _serviceNomination = Kernal.Get<NominationsRepository>();
            _serviceMetadatCycle = Kernal.Get<metadataCycleRepository>();
            unitOfWork = Kernal.Get<UnitOfWork>();
            modalFactory = Kernal.Get<ModalFactory>();
            _serviceShipperCompany = Kernal.Get<ShipperCompanyRepository>();
            _serviceFileSysIncomingData = Kernal.Get<FileSysIncomingDataRepository>();
            _serviceSqtsOpPerTransaction = Kernal.Get<SQTSOPPerTransactionRepository>();
            #region get nmqr services
            _serviceNmqrPerTransaction = Kernal.Get<NMQRPerTransactionRepository>();
            #endregion
            #region get sqts service
            _serviceSqtsTrackOnNom = Kernal.Get<SQTSTrackOnNomRepository>();
            _serviceSqtsFile = Kernal.Get<SQTSFileRepository>();
            _serviceSqtsPerTransaction = Kernal.Get<SQTSPerTransactionRepository>();
            #endregion
            #region get email service
            emailTempalteService = Kernal.Get<EmailTemplateRepository>();
            emailQueueService = Kernal.Get<EmailQueueRepository>();
            appLogService = Kernal.Get<ApplicationLogRepository>();
            #endregion

            var path = Path.Combine(HostingEnvironment.MapPath("~/App_Data"), "SQTSReductionReasons.xml");
            FileInfo fileInfo = new FileInfo(path);
            XmlDocument xmlRedReason = new XmlDocument();
            
            if (System.IO.File.Exists(path) && !IsFileLocked(fileInfo))
            {
                xmlRedReason.Load(path);
                var redResonNode = xmlRedReason.GetElementsByTagName("Reason");
                if (redResonNode != null)
                {
                    redReasonList = redResonNode.Cast<XmlNode>();
                }
            }
        }
        public void ProcessFiles()
        {
            //AppLogManager("Schedular calling time", "info", DateTime.Now.ToLongTimeString());
            try
            {
                var releaseToShipper = _serviceSettings.GetById((int)Settings.releaseToShipper).Value;
                var crashFilesPath = _serviceSettings.GetById((int)Settings.WebCrashFilesPath).Value;
                var AckFiles = _serviceSettings.GetById((int)Settings.AckFiles).Value;
                var UnknownFiles = _serviceSettings.GetById((int)Settings.UnknownFiles).Value;
                var EngineFilePath= _serviceSettings.GetById((int)Settings.WebRecSchedFilePath).Value;
                
                var inFileList = Directory.GetFiles(EngineFilePath);
                if (inFileList != null && inFileList.Count() > 0)
                {
                    List<NMQRPerTransaction> NmqrList = new List<NMQRPerTransaction>();
                    List<SQTSPerTransaction> SqtsList = new List<SQTSPerTransaction>();
                    List<FileSysIncomingData> incominDataList = new List<FileSysIncomingData>();
                    List<SQTSOPPerTransaction> SqtsopList = new List<SQTSOPPerTransaction>();
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
                                                case EDIFileType.NMQR:
                                                    currentfile.DatasetType = "Nmqr";
                                                    NmqrList.AddRange(FillUpNMQRData(subEDIfile, currentfile.MessageId, dataSeperator, segmentSeperator));
                                                    IsProcessed = true;
                                                    break;
                                                case EDIFileType.SQTS:
                                                    currentfile.DatasetType = "SQTS";
                                                    SqtsList.AddRange(FillUpSQTSData(subEDIfile, currentfile.MessageId, dataSeperator, segmentSeperator, redReasonList));
                                                    IsProcessed = true;
                                                    break;
                                                case EDIFileType.SQTSOP:
                                                    currentfile.DatasetType = "SQTSOP";
                                                    SqtsopList.AddRange(FillUpSQTSOPData(subEDIfile, currentfile.MessageId, dataSeperator, segmentSeperator));
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
                            AppLogManager("web receive schedular", "error", ex.ToString());
                            var name=Path.GetFileNameWithoutExtension(file);
                            string data = fileText;
                            if (File.Exists(file))
                            {
                                data = File.ReadAllText(file);
                            }
                            IsProcessed = false;
                            if (!Directory.Exists(crashFilesPath))
                            {
                                Directory.CreateDirectory(crashFilesPath);
                            }
                            var crashFilePath = crashFilesPath + "/" + name + ".txt";
                            File.WriteAllText(crashFilePath, data + Environment.NewLine + ex.Message);
                            var EmailTo = new string[] { "jay.singh@enercross.com","lakhwinder.singh@fifthnote.co", "gagandeep.singh@fifthnote.co","mahesh.prajapati@fifthnote.co" };
                            string subject = "Crashed File";
                            string fromDisplay = "NatGasHub";
                            string Body = "Please find the attached crashed file.";
                            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(data));
                            Attachment attachments = new Attachment(stream, name + ".txt", System.Net.Mime.MediaTypeNames.Text.Plain);
                            EmailService.SendEmailAsync(EmailTo, Body, subject, fromDisplay, attachments);
                            stream.Dispose();
                        }
                    });
                    bool IsDataEntered = false;

                    if (IsProcessed)
                    {
                        if (NmqrList.Count > 0)
                            IsDataEntered = ProcessNmqrData(NmqrList);
                        if (SqtsList.Count > 0)
                        {
                            IsDataEntered = ProcessSqtsData(SqtsList);
                        }
                        if (SqtsopList.Count > 0)
                        {
                            IsDataEntered = ProcessSqtqOpData(SqtsopList);
                        }
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
        
        private List<NMQRPerTransaction> FillUpNMQRData(string ediFile, string fileName, char dataSeperator, char segmentSeperator)
        {
            string pipelineDUNS = ediFile.Substring(35, 9);

            NMQR_DS ediNmqr = new NMQR_DS(ediFile
                , new char[] { segmentSeperator }//segement seperators
                , new char[] { dataSeperator }
                , fileName);
            return modalFactory.Create(ediNmqr.ReadNMQRFileData());
        }
        
        
        
        private List<SQTSPerTransaction> FillUpSQTSData(string ediFile, string filename, char dataSeperator, char segmentSeperator, IEnumerable<XmlNode> redReasonList)
        {
            string pipelineDUNS = ediFile.Substring(35, 9);
            SQTS_DS ediSqts = new SQTS_DS(ediFile
                , new char[] { segmentSeperator }//segement seperators
                , new char[] { dataSeperator }
                , filename
                , redReasonList);
            return modalFactory.Create(ediSqts.ReadSQTSFile());
        }
        private List<SQTSOPPerTransaction> FillUpSQTSOPData(string ediFile, string filename, char dataSeperator, char segmentSeperator)
        {
            SQTSOP_DS ediSqtsOp = new SQTSOP_DS(ediFile
                , new char[] { segmentSeperator }
                , new char[] { dataSeperator }
                , filename);
            return modalFactory.Create(ediSqtsOp.ReadSqtsopFile());
        }
        #endregion
        #region Data insert into db from EDI file
        private bool ProcessNmqrData(List<NMQRPerTransaction> nmqrList)
        {
            try
            {
                NominationStatu nomStatus = null;
                V4_Batch batch = null;
                foreach (var nmqr in nmqrList)
                {
                    _serviceNmqrPerTransaction.Add(nmqr);
                    _serviceNmqrPerTransaction.Save();
                }
                if (nmqrList.Count > 0)
                {
                    string referenceNumber = nmqrList.FirstOrDefault().ReferenceNumber;
                    string statusCode = nmqrList.FirstOrDefault().StatusCode;
                    if (!string.IsNullOrEmpty(referenceNumber))
                        nomStatus = _serviceNomStatus.GetNomStatusOnReferenceNumber(referenceNumber);
                    if (nomStatus != null)
                    {
                        nomStatus.NMQR_ID = nmqrList.FirstOrDefault().Transactionid.ToString();
                        nomStatus.StatusDetail = statusCode;
                        if (nomStatus.StatusDetail == "EZ")
                            nomStatus.StatusID = (int)statusBatch.Failure_Gisb;
                        if (nomStatus.StatusDetail == "RZ")
                            nomStatus.StatusID = (int)statusBatch.Failure_NMQR;
                        if (nomStatus.StatusDetail == "WQ")
                            nomStatus.StatusID = (int)statusBatch.Success_NMQR;
                        _serviceNomStatus.Update(nomStatus);
                        _serviceNomStatus.Save();
                        batch = _serviceBatch.GetByTransactionID(nomStatus.NOM_ID);
                        if (batch != null)
                        {
                            batch.StatusID = nomStatus.StatusID;
                            _serviceBatch.Update(batch);
                            _serviceBatch.SaveChages();
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool ProcessSqtsData(List<SQTSPerTransaction> sqtsList)
        {
            try
            {
                if (sqtsList.Count() > 0)
                {
                    _serviceSqtsPerTransaction.SaveSqtsList(sqtsList);
                    _serviceSqtsPerTransaction.Save();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool ProcessSqtqOpData(List<SQTSOPPerTransaction> sqtsOpList)
        {
            try
            {
                return  _serviceSqtsOpPerTransaction.SaveSqtsList(sqtsOpList);
                
            }catch(Exception ex)
            {
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
