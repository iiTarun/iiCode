using System;
using System.Text;
using System.Web;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using DidiSoft.Pgp;
using Nom1Done.Model;
using Nom1Done.Data.Repositories;
using System.Linq;
using Nom1Done.DTO;
using Nom1Done.Service.Interface;
using EDITranslation;
using UPRD.Data.Repositories;
using EDITranslation.Nominations;
using EDITranslation.AdditionalStandards;

namespace Nom1Done.Common
{
    public class ManageIncomingRequests : IManageIncomingRequestService
    {
        public Inbox _inbox;
        IUprdInboxRepository _uprdInboxRepo;
        IInboxRepository _serviceInbox;
        IUprdJobWorkflowRepository _uprdJobWorkFlowRepo;
        IJobWorkflowRepository _serviceJobworkFlow;
        IUprdSettingRepository _uprdSettingRepo;
        ISettingRepository _serviceSetting;
        IUprdPipelineRepository _uprdPipelineRepo;
        IPipelineRepository _pipe;
        IUprdIncomingDataRepository _uprdIncomingData;
        IIncomingDataRepository _serviceIncomingData;
        IUprdTaskMgrJobsRepository _uprdTaskMgrJobRepo;
        ITaskMgrJobsRepository _serviceTaskMgrJobs;
        IUprdGISBInboxRepository _uprdGISBInboxRepo;
        IGISBInboxRepository _serviceGisbInbox;
        IApplicationLogRepository _applicationLogs;
        IUprdmetadataErrorCodeRepository _uprdmetadataErrorCodeRepo;
        ImetadataErrorCodeRepository _servicemetadataErrorCode;
        IUprdEmailTemplateRepository _uprdEmailTemplate;
        IEmailTemplateRepository serviceEmailTemplate;
        IUprdEmailQueueRepository _uprdEmailQueueRepo;
        IEmailQueueRepository serviceEmailQueue;
        IApplicationLogRepository serviceAppLog;
        IUPRDApplicationLogRepository uprdAppLogRepo;
        IUprdShipperCompanyRepository uprdShipperCompany;
        IShipperCompanyRepository _serviceShipperCompany;
        INMQRPerTransactionRepository _serviceNmqrPerTransaction;
        INominationStatusRepository _serviceNomStatus;
        IModalFactory modalFactory;
        IBatchRepository _serviceBatch;
        ISQTSPerTransactionRepository _serviceSqtsPerTransaction;
        public ManageIncomingRequests(IUprdInboxRepository uprdInboxRepo,
        IInboxRepository serviceInbox,
        IUprdJobWorkflowRepository uprdJobWorkFlowRepo,
        IJobWorkflowRepository serviceJobworkFlow,
        IUprdSettingRepository uprdSettingRepo,
        ISettingRepository serviceSetting,
        IUprdPipelineRepository uprdPipelineRepo,
        IPipelineRepository pipe,
        IUprdIncomingDataRepository uprdIncomingData,
        IIncomingDataRepository serviceIncomingData,
        IUprdTaskMgrJobsRepository uprdTaskMgrJobRepo,
        ITaskMgrJobsRepository serviceTaskMgrJobs,
        IUprdGISBInboxRepository uprdGISBInboxRepo,
        IGISBInboxRepository serviceGisbInbox,
        IApplicationLogRepository applicationLogs,
        IUprdmetadataErrorCodeRepository uprdmetadataErrorCodeRepo,
        ImetadataErrorCodeRepository servicemetadataErrorCode,
        IUprdEmailTemplateRepository uprdEmailTemplate,
        IEmailTemplateRepository serviceEmailTemplate,
        IUprdEmailQueueRepository uprdEmailQueueRepo,
        IEmailQueueRepository serviceEmailQueue,
        IUPRDApplicationLogRepository uprdErrorLog,
        IApplicationLogRepository serviceAppLog,
        IUprdShipperCompanyRepository uprdShipperCompany,
        IModalFactory modalFactory,
        INominationStatusRepository serviceNomStatus,
        IBatchRepository serviceBatch,
        INMQRPerTransactionRepository serviceNmqrPerTransaction,
        ISQTSPerTransactionRepository serviceSqtsPerTransaction,
        IShipperCompanyRepository serviceShipperCompany)
        {
            this._serviceInbox = serviceInbox;
            this._uprdInboxRepo = uprdInboxRepo;
            this._uprdJobWorkFlowRepo = uprdJobWorkFlowRepo;
            this._serviceJobworkFlow = serviceJobworkFlow;
            this._uprdSettingRepo = uprdSettingRepo;
            this._serviceSetting = serviceSetting;
            this._uprdPipelineRepo = uprdPipelineRepo;
            this._pipe = pipe;
            this._uprdIncomingData = uprdIncomingData;
            this._serviceIncomingData = serviceIncomingData;
            this._uprdTaskMgrJobRepo = uprdTaskMgrJobRepo;
            this._serviceTaskMgrJobs = serviceTaskMgrJobs;
            this._uprdGISBInboxRepo = uprdGISBInboxRepo;
            this._serviceGisbInbox = serviceGisbInbox;
            this._applicationLogs = applicationLogs;
            this._uprdmetadataErrorCodeRepo = uprdmetadataErrorCodeRepo;
            this._servicemetadataErrorCode = servicemetadataErrorCode;
            this._uprdEmailTemplate = uprdEmailTemplate;
            this.serviceEmailTemplate = serviceEmailTemplate;
            this._uprdEmailQueueRepo = uprdEmailQueueRepo;
            this.serviceEmailQueue = serviceEmailQueue;
            this.serviceAppLog = serviceAppLog;
            this.uprdAppLogRepo = uprdErrorLog;
            this._serviceShipperCompany = serviceShipperCompany;
            this.uprdShipperCompany = uprdShipperCompany;
            this.modalFactory = modalFactory;
            this._serviceNmqrPerTransaction = serviceNmqrPerTransaction;
            this._serviceNomStatus = serviceNomStatus;
            this._serviceBatch = serviceBatch;

            this._serviceSqtsPerTransaction = serviceSqtsPerTransaction;
        }
        public string ProcessRequest(HttpRequestBase request, bool isTest, bool separateFiles)
        {
            int dataset = 0;
            bool ForNom = true;
            string newGisb = "false";
            try
            {
                int code = 0;
                string _senderDuns = string.Empty;
                string shipperDuns = _serviceSetting.GetById((int)Settings.releaseToShipper).Value;
                string message = string.Empty;
                var transactionId = Guid.NewGuid();
                StringBuilder decFile = new StringBuilder();
                #region Extract file from the request
                if (request.Form.Count <= 0)
                    return newGisb;
                foreach (string key in request.Files.Keys)
                {
                    HttpPostedFileBase file = request.Files.Get(key);
                    Stream receivedFileStream = file.InputStream;
                    StreamReader sr = new StreamReader(receivedFileStream);
                    message = sr.ReadToEnd();
                    _senderDuns = request.Form["from"].ToString();
                    AppLogManager("Senders Duns", "Info", _senderDuns, ForNom);
                }
                #endregion

                try
                {

                    if (!string.IsNullOrEmpty(message))
                        decFile = DecryptFile(new StringBuilder(message), shipperDuns, _senderDuns);
                    else
                        GenerateAndSendErrorMail("Received file is found empty", "received file is empty ", isTest, ForNom);

                }
                catch (Exception ex)
                {
                    AppLogManager("Receiving File Encryption in web.", "Error", ex.Message + " InnerException:- " + ex.InnerException, ForNom);
                    return "Wrong Encryption key.";
                }

                #region if any issue then uncommented this code / comment dataset Identification and process
                if (!string.IsNullOrEmpty(decFile.ToString()))
                    dataset = IdentifyFileReceived(decFile.ToString(), shipperDuns);
                else
                    GenerateAndSendErrorMail("Received file is not decript ", "received file is not decrypted ", isTest, ForNom);
                switch (dataset)
                {
                    case (int)Dataset.NMQR:
                        try
                        {
                            List<NMQRPerTransaction> NmqrList = null;
                            bool IsDataEntered = false;
                            char dataSeperator, segmentSeperator;
                            dataSeperator = Convert.ToChar(decFile.ToString()[(decFile.ToString().IndexOf("ISA") + "ISA".Length)]);
                            segmentSeperator = Convert.ToChar(decFile.ToString()[(decFile.ToString().IndexOf("GS") - 1)]);
                            int pos = decFile.ToString().IndexOf("ISA");
                            var data = decFile.Remove(0, pos);
                            var pipelineDUNS = data.ToString().Substring(35, 9);
                            if (dataSeperator != '\0' && segmentSeperator != '\0')
                            {
                                EDIWrapperBase ediWrapper = new EDIWrapperBase(decFile.ToString()
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
                                    EDIWrapperBase subEdiWrapper = new EDIWrapperBase(subEDIfile
                                    , new char[] { segmentSeperator }//segement seperators
                                    , new char[] { dataSeperator });//Dataseperators

                                    EDIFileType fileType = subEdiWrapper.FileType;
                                    switch (fileType)
                                    {
                                        case EDIFileType.NMQR:
                                            NmqrList = new List<NMQRPerTransaction>();
                                            NmqrList.AddRange(FillUpNMQRData(subEDIfile, transactionId.ToString(), dataSeperator, segmentSeperator));
                                            if (NmqrList.Count > 0)
                                                IsDataEntered = ProcessNmqrData(NmqrList, isTest, ForNom);
                                            else
                                                IsDataEntered = false;
                                            break;
                                    }
                                }
                                newGisb = StoreNomEntry(message, decFile.ToString(), isTest, _senderDuns, dataset, transactionId, IsDataEntered);
                            }
                        }
                        catch (Exception ex)
                        {
                            code = 0;
                            if (string.IsNullOrEmpty(_senderDuns))
                            {
                                code = (int)ErrorTypes.Missing_from_Common_Code_Identifier_code;
                            }
                            newGisb = SendGisbAndStoreFileAndGisbInFileSys(decFile.ToString(), transactionId, code, isTest, shipperDuns, true);
                            AppLogManager("Receiving File in web.", "Error", ex.Message + " InnerException:- " + ex.InnerException, ForNom);
                            GenerateAndSendErrorMail("Receiving File in web.", ex.Message, isTest, ForNom);
                        }
                        break;
                    case (int)Dataset.SQTS:
                    case (int)Dataset.SQOP:
                        code = 0;
                        if (string.IsNullOrEmpty(_senderDuns))
                        {
                            code = (int)ErrorTypes.Missing_from_Common_Code_Identifier_code;
                        }
                        newGisb = SendGisbAndStoreFileAndGisbInFileSys(decFile.ToString(), transactionId, code, isTest, shipperDuns, true);
                        break;
                    case 0:
                        code = 0;
                        if (string.IsNullOrEmpty(_senderDuns))
                        {
                            code = (int)ErrorTypes.Missing_from_Common_Code_Identifier_code;
                        }
                        newGisb = SendGisbAndStoreFileAndGisbInFileSys(decFile.ToString(), transactionId, code, isTest, shipperDuns, false);
                        break;
                }
                return newGisb;
                #endregion
            }
            catch (Exception ex)
            {
                AppLogManager("Receiving File in web.", "Error", ex.Message + " InnerException:- " + ex.InnerException, ForNom);
                GenerateAndSendErrorMail("Receiving File in web.", ex.Message, isTest, ForNom);
                return newGisb;
            }
        }
        private bool ProcessNmqrData(List<NMQRPerTransaction> nmqrList, bool IsTest, bool ForNom)
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
                AppLogManager("Receiving File in web.", "Error", ex.Message + " InnerException:- " + ex.InnerException, ForNom);
                GenerateAndSendErrorMail("Receiving File in web.", ex.Message, IsTest, ForNom);
                return false;
            }
        }
        private string SendGisbAndStoreFileAndGisbInFileSys(string decFile, Guid transactionId, int code, bool IsTest, string releaseToShipper, bool IsWeb)
        {
            string newGisb = "false";
            string CrashFilesPath = "";
            string UnknownFilePath = "";
            string EngSchedFilePath = "";
            try
            {
                if (IsWeb)
                {
                    CrashFilesPath = _serviceSetting.GetById((int)Settings.WebCrashFilesPath).Value;
                    UnknownFilePath = _serviceSetting.GetById((int)Settings.UnknownFiles).Value;
                    EngSchedFilePath = _serviceSetting.GetById((int)Settings.WebRecSchedFilePath).Value;
                }
                else
                {
                    CrashFilesPath = _serviceSetting.GetById((int)Settings.CrashFilesPath).Value;
                    UnknownFilePath = _serviceSetting.GetById((int)Settings.UnknownFiles).Value;
                    EngSchedFilePath = _serviceSetting.GetById((int)Settings.EngineFilesPath).Value;
                }

                newGisb = GenerateGisb(code, IsTest, releaseToShipper);
                //store file into file sys
                if (!Directory.Exists(EngSchedFilePath))
                {
                    Directory.CreateDirectory(EngSchedFilePath);
                }
                var engineFilePath = EngSchedFilePath + "/" + transactionId + ".txt";
                File.WriteAllText(engineFilePath, decFile);
                //store gisb into file sys
                if (!Directory.Exists(UnknownFilePath))
                {
                    Directory.CreateDirectory(UnknownFilePath);
                }
                var unknownFilePath = UnknownFilePath + "/" + transactionId + ".txt";
                File.WriteAllText(unknownFilePath, newGisb);

                return newGisb;
            }
            catch (Exception ex)
            {
                if (!Directory.Exists(CrashFilesPath))
                {
                    Directory.CreateDirectory(CrashFilesPath);
                }
                var CrashFilePath = CrashFilesPath + "/" + "NMQR_" + transactionId + ".txt";
                File.WriteAllText(CrashFilePath, decFile);
                return newGisb;
            }
        }
        private string StoreNomEntry(string encEdi, string decEdi, bool isTest, string _senderDuns, int dataset, Guid TransactionId, bool IsDataEntered)
        {
            string newGisb = "false";
            try
            {
                bool isValid = true;
                int Code = 0;
                string message = string.Empty;
                //string filePath = string.Empty;

                Inbox inMessage = new Inbox();
                inMessage.MessageID = TransactionId;
                inMessage.DatasetID = 0;
                inMessage.StatusID = (int)statusBatch.Encrypted;
                inMessage.GISB = "";
                inMessage.IsTest = false;
                inMessage.IsRead = false;
                inMessage.PipelineID = 0;
                inMessage.RecipientCompanyID = 0;
                inMessage.CreatedDate = DateTime.Now;
                inMessage.IsActive = true;
                #region Get file in request or getting form data (Initial Receiving Stage 1)
                try
                {
                    //****generate TransactionID in Inbox
                    inMessage.IsTest = isTest;
                    inMessage.StatusID = (int)statusBatch.Encrypted;
                    _serviceInbox.Add(inMessage);
                    _serviceInbox.Save();
                }
                catch (Exception ex)
                {
                    AppLogManager("Receive web Initial stage", "Error", ex.Message, true);
                    GenerateAndSendErrorMail("Receive web Initial stage", ex.Message, isTest, true);
                }
                #endregion
                #region Store Encrypted file in DB (Enc_file ToDB Receiving Stage 2)
                try
                {
                    //****checking if Form exist and sender(TSP) in request and store file in filesystem and DB (IncomingData)
                    Setting setting = _serviceSetting.GetById((int)Settings.From_Key);
                    if (setting != null)
                    {
                        #region Update Pipeline in inbox  (get duns from request)
                        //****Get senders DUNS from the request and update in Inbox (pipelineID)
                        int pos = decEdi.ToString().IndexOf("ISA");
                        var data = decEdi.Remove(0, pos);
                        var pipelineDUNS = data.ToString().Substring(35, 9);
                        if (string.IsNullOrEmpty(_senderDuns))
                        {
                            isValid = false;
                            Code = (int)ErrorTypes.Missing_from_Common_Code_Identifier_code;
                            inMessage.StatusID = (int)statusBatch.Failure_Gisb;
                            AppLogManager("Receive web Store file to DB", "Error", "_sendersDuns not in the request", true);
                        }
                        else
                        {
                            Pipeline pipe = _pipe.GetPipelineByDuns(pipelineDUNS);
                            if (pipe != null)
                                inMessage.PipelineID = pipe.ID;
                            else
                                AppLogManager("Receive web Store file to DB", "Error", "_senderDuns not get pipe", true);
                        }
                        #endregion
                        //****Store file in file System and DB
                        if (isValid)
                        {
                            #region Store file in DB
                            //****store file into DB
                            IncomingData incomingDate = new IncomingData();
                            incomingDate.Id = Guid.NewGuid().ToString();
                            incomingDate.MessageId = inMessage.MessageID.ToString();
                            incomingDate.ReceivedAt = DateTime.Now;
                            incomingDate.DataReceived = encEdi;
                            incomingDate.DecryptedData = decEdi;
                            incomingDate.PipeDuns = pipelineDUNS;
                            incomingDate.IsProcessed = IsDataEntered;
                            if (dataset == (int)Dataset.NMQR)
                            {
                                incomingDate.DatasetType = "NMQR";
                            }
                            else if (dataset == (int)Dataset.SQTS)
                            {
                                incomingDate.DatasetType = "SQTS";
                            }
                            else if (dataset == (int)Dataset.SQOP)
                            {
                                incomingDate.DatasetType = "SQOP";
                            }
                            else
                            {
                                incomingDate.IsProcessed = false;
                            }
                            _serviceIncomingData.Add(incomingDate);
                            _serviceIncomingData.Save();
                            #endregion
                        }
                        _serviceInbox.Update(inMessage);
                        _serviceInbox.Save();
                    }
                }
                catch (Exception ex)
                {
                    AppLogManager("Receive web Store file to DB (2nd stage)", "Error", ex.Message, true);
                    GenerateAndSendErrorMail("Receive web Store file to DB (2nd stage)", ex.Message, isTest, true);
                }

                #endregion
                #region  Generate Gisb (Send GISB Receiving Stage 3)
                try
                {
                    newGisb = GenerateGISB(inMessage.MessageID, Code, isTest, true);
                    if (string.IsNullOrEmpty(newGisb))
                    {
                        AppLogManager("Receive web GISB generation.", "Error", "GISB not generated.", true);
                        inMessage.StatusID = (int)statusBatch.Failure_Gisb;
                    }
                    else
                    {
                        inMessage.StatusID = (int)statusBatch.Success_Gisb;
                    }
                    inMessage.GISB = newGisb;
                    _serviceInbox.Update(inMessage);
                    _serviceInbox.Save();
                }
                catch (Exception ex)
                {
                    AppLogManager("Receive web generate GISB", "Error", ex.Message, true);
                    GenerateAndSendErrorMail("Receive web generate GISB", ex.Message, isTest, true);
                }
                #endregion
                return newGisb;
            }
            catch (Exception ex)
            {
                return newGisb;
            }
        }
        private string GenerateGisb(int code, bool isTest, string releaseToShipper)
        {
            string newGisb = "false";
            string privateKey = "", privateKeyPass = "";
            if (releaseToShipper == "078711334")//Enercross duns
            {
                privateKey = _serviceSetting.GetById((int)Settings.EnecrossPrivateKey).Value;
                privateKeyPass = _serviceSetting.GetById((int)Settings.Passphrase).Value;
            }
            else if (releaseToShipper == "101069263")//Emera duns
            {
                privateKey = _serviceSetting.GetById((int)Settings.EmeraPrivateKey).Value;
                privateKeyPass = _serviceSetting.GetById((int)Settings.EmeraPassphrase).Value;
            }
            else
            {
                privateKey = _serviceSetting.GetById((int)Settings.EnecrossPrivateKey).Value;
                privateKeyPass = _serviceSetting.GetById((int)Settings.Passphrase).Value;
            }

            Random r = new Random();
            int transid = r.Next(10000000, 90000000);
            string currentDate = Format.GisbDate();
            string status = "ok";

            if (code != 0)
            {
                metadataErrorCode codeStatus = _servicemetadataErrorCode.GetById(code);//.GetAll().Where(a=>a.ID==code).FirstOrDefault();
                if (codeStatus != null)
                {
                    status = codeStatus.Code + ": " + codeStatus.Description.Replace("'", "");
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("content-type: multipart/signed; micalg=pgp-md5; protocol=");
            sb.Append(@"""application/pgp-signature""");
            sb.Append("; boundary=");
            sb.Append(@"""--boundary2--" + currentDate);
            sb.Append(@"""");
            sb.Append("\r\n\r\n");

            sb.Append("----boundary2--" + currentDate + "\r\n");
            sb.Append("content-type: multipart/report; report-type=");
            sb.Append(@"""gisb-acknowledgement-receipt""");
            sb.Append("; boundary=");
            sb.Append(@"""NAESB7867""");

            sb.Append("\r\n\r\n--NAESB7867\r\n");
            sb.Append("content-type:text/html\r\n");

            var SignedDoc = SignedGISB(privateKey, privateKeyPass, currentDate, status, transid, isTest, true);

            sb.Append("<HTML><HEAD><TITLE>Acknowledgement Receipt Success</TITLE></HEAD><BODY><P>");
            sb.Append("time-c=" + currentDate + "*</br>");
            sb.Append("request-status=" + status + "*</br>");
            if (releaseToShipper == "078711334")
            {
                if (isTest)
                    sb.Append("server-id=test.enercross.com*</br>");
                else
                    sb.Append("server-id=prod.enercross.com*</br>");
            }
            else if (releaseToShipper == "101069263")
            {
                if (isTest)
                    sb.Append("server-id=emeratest.enercross.com*</br>");
                else
                    sb.Append("server-id=emeraprod.enercross.com*</br>");
            }
            else
            {
                if (isTest)
                    sb.Append("server-id=kochtest.enercross.com*</br>");
                else
                    sb.Append("server-id=kochprod.enercross.com*</br>");
            }
            sb.Append("trans-id=" + transid + "*</br>");
            sb.Append("</P></BODY></HTML>\r\n\r\n");
            sb.Append("--NAESB7867\r\n");
            sb.Append("Content-type: text/plain\r\n");
            sb.Append("time-c=" + currentDate + "*\r\n");
            sb.Append("request-status=" + status + "*\r\n");
            if (releaseToShipper == "078711334")
            {
                if (isTest)
                    sb.Append("server-id=test.enercross.com*</br>");
                else
                    sb.Append("server-id=prod.enercross.com*</br>");
            }
            else if (releaseToShipper == "101069263")
            {
                if (isTest)
                    sb.Append("server-id=emeratest.enercross.com*</br>");
                else
                    sb.Append("server-id=emeraprod.enercross.com*</br>");
            }
            else
            {
                if (isTest)
                    sb.Append("server-id=kochtest.enercross.com*</br>");
                else
                    sb.Append("server-id=kochprod.enercross.com*</br>");
            }
            sb.Append("trans-id=" + transid + "*\r\n");
            sb.Append("--NAESB7867--\r\n");
            sb.Append("----boundary2--" + currentDate + "\r\n");
            sb.Append("content-type: application/pgp-signature \r\n\r\n");
            sb.Append(SignedDoc.ToString());
            sb.Append("\r\n----boundary2--" + currentDate + "--");

            newGisb = sb.ToString();
            return newGisb;
        }
        private string GenerateGISB(Guid messageId, int code, bool isTest, bool ForNom)
        {
            string Gisb = "", privateKey = "", privateKeyPass = "", releaseToShipper = "";
            if (ForNom)
            {
                List<Setting> _settings = null;
                _settings = _serviceSetting.GetAll().ToList();
                releaseToShipper = _settings.Find(a => a.ID == (int)Settings.releaseToShipper).Value;
                ShipperCompany shipComp = null;
                if (!string.IsNullOrEmpty(releaseToShipper))
                    shipComp = _serviceShipperCompany.GetShipperCompanyByDuns(releaseToShipper);
                if (shipComp.ID == (int)ShipperComps.Enercross)
                {
                    privateKey = _settings.Find(a => a.ID == (int)Settings.EnecrossPrivateKey).Value;
                    privateKeyPass = _settings.Find(a => a.ID == (int)Settings.Passphrase).Value;
                }
                else if (shipComp.ID == (int)ShipperComps.Emera)
                {
                    privateKey = _settings.Find(a => a.ID == (int)Settings.EmeraPrivateKey).Value;
                    privateKeyPass = _settings.Find(a => a.ID == (int)Settings.EmeraPassphrase).Value;
                }
                else // Koch
                {
                    privateKey = _settings.Find(a => a.ID == (int)Settings.EnecrossPrivateKey).Value;
                    privateKeyPass = _settings.Find(a => a.ID == (int)Settings.Passphrase).Value;
                }
            }
            else
            {
                List<UPRD.Model.Setting> _settings = null;
                _settings = _uprdSettingRepo.GetAll().ToList();
                releaseToShipper = _settings.Find(a => a.ID == (int)Settings.releaseToShipper).Value;
                UPRD.Model.ShipperCompany shipComp = null;
                if (!string.IsNullOrEmpty(releaseToShipper))
                    shipComp = uprdShipperCompany.GetShipperCompanyByDuns(releaseToShipper);
                if (shipComp.ID == (int)ShipperComps.Enercross)
                {
                    privateKey = _settings.Find(a => a.ID == (int)Settings.EnecrossPrivateKey).Value;
                    privateKeyPass = _settings.Find(a => a.ID == (int)Settings.Passphrase).Value;
                }
                else if (shipComp.ID == (int)ShipperComps.Emera)
                {
                    privateKey = _settings.Find(a => a.ID == (int)Settings.EmeraPrivateKey).Value;
                    privateKeyPass = _settings.Find(a => a.ID == (int)Settings.EmeraPassphrase).Value;
                }
                else //koch
                {
                    privateKey = _settings.Find(a => a.ID == (int)Settings.EnecrossPrivateKey).Value;
                    privateKeyPass = _settings.Find(a => a.ID == (int)Settings.Passphrase).Value;
                }
            }

            try
            {
                Random r = new Random();
                int transid = r.Next(10000000, 90000000);
                string currentDate = Format.GisbDate();
                string status = "ok";
                if (code != 0)
                {
                    if (ForNom)
                    {
                        metadataErrorCode codeStatus = _servicemetadataErrorCode.GetById(code);//.GetAll().Where(a=>a.ID==code).FirstOrDefault();
                        if (codeStatus != null)
                        {
                            status = codeStatus.Code + ": " + codeStatus.Description.Replace("'", "");
                        }
                    }
                    else
                    {
                        UPRD.Model.metadataErrorCode codeStatus = _uprdmetadataErrorCodeRepo.GetById(code);//.GetAll().Where(a=>a.ID==code).FirstOrDefault();
                        if (codeStatus != null)
                        {
                            status = codeStatus.Code + ": " + codeStatus.Description.Replace("'", "");
                        }
                    }

                }

                StringBuilder sb = new StringBuilder();
                sb.Append("content-type: multipart/signed; micalg=pgp-md5; protocol=");
                sb.Append(@"""application/pgp-signature""");
                sb.Append("; boundary=");
                sb.Append(@"""--boundary2--" + currentDate);
                sb.Append(@"""");
                sb.Append("\r\n\r\n");

                sb.Append("----boundary2--" + currentDate + "\r\n");
                sb.Append("content-type: multipart/report; report-type=");
                sb.Append(@"""gisb-acknowledgement-receipt""");
                sb.Append("; boundary=");
                sb.Append(@"""NAESB7867""");

                sb.Append("\r\n\r\n--NAESB7867\r\n");
                sb.Append("content-type:text/html\r\n");

                var SignedDoc = SignedGISB(privateKey, privateKeyPass, currentDate, status, transid, isTest, ForNom);

                sb.Append("<HTML><HEAD><TITLE>Acknowledgement Receipt Success</TITLE></HEAD><BODY><P>");
                sb.Append("time-c=" + currentDate + "*</br>");
                sb.Append("request-status=" + status + "*</br>");
                if (releaseToShipper == "078711334")
                {
                    if (isTest)
                        sb.Append("server-id=test.enercross.com*</br>");
                    else
                        sb.Append("server-id=prod.enercross.com*</br>");
                }
                else if (releaseToShipper == "101069263")
                {
                    if (isTest)
                        sb.Append("server-id=emeratest.enercross.com*</br>");
                    else
                        sb.Append("server-id=emeraprod.enercross.com*</br>");
                }
                else if (releaseToShipper == "832703297")
                {
                    if (isTest)
                        sb.Append("server-id=kochtest.enercross.com*</br>");
                    else
                        sb.Append("server-id=kochprod.enercross.com*</br>");
                }

                sb.Append("trans-id=" + transid + "*</br>");
                sb.Append("</P></BODY></HTML>\r\n\r\n");
                sb.Append("--NAESB7867\r\n");
                sb.Append("Content-type: text/plain\r\n");
                sb.Append("time-c=" + currentDate + "*\r\n");
                sb.Append("request-status=" + status + "*\r\n");
                if (releaseToShipper == "078711334")
                {
                    if (isTest)
                        sb.Append("server-id=test.enercross.com*</br>");
                    else
                        sb.Append("server-id=prod.enercross.com*</br>");
                }
                else if (releaseToShipper == "101069263")
                {
                    if (isTest)
                        sb.Append("server-id=emeratest.enercross.com*</br>");
                    else
                        sb.Append("server-id=emeraprod.enercross.com*</br>");
                }
                else if (releaseToShipper == "832703297")
                {
                    if (isTest)
                        sb.Append("server-id=test.enercross.com*</br>");
                    else
                        sb.Append("server-id=prod.enercross.com*</br>");
                }
                sb.Append("trans-id=" + transid + "*\r\n");
                sb.Append("--NAESB7867--\r\n");
                sb.Append("----boundary2--" + currentDate + "\r\n");
                sb.Append("content-type: application/pgp-signature \r\n\r\n");
                sb.Append(SignedDoc.ToString());
                sb.Append("\r\n----boundary2--" + currentDate + "--");

                Gisb = sb.ToString();
                if (ForNom)
                {
                    GISBInbox gisbInbox = new GISBInbox();
                    gisbInbox.MessageID = messageId.ToString();
                    gisbInbox.Gisb = Gisb;
                    gisbInbox.ErrorCode = code;
                    gisbInbox.ErrorDescription = status;
                    gisbInbox.CreatedDate = DateTime.Now;
                    _serviceGisbInbox.Add(gisbInbox);
                    _serviceGisbInbox.Save();
                }
                else
                {
                    UPRD.Model.GISBInbox gisbInbox = new UPRD.Model.GISBInbox();
                    gisbInbox.MessageID = messageId.ToString();
                    gisbInbox.Gisb = Gisb;
                    gisbInbox.ErrorCode = code;
                    gisbInbox.ErrorDescription = status;
                    gisbInbox.CreatedDate = DateTime.Now;
                    _uprdGISBInboxRepo.Add(gisbInbox);
                    _uprdGISBInboxRepo.Save();
                }

            }
            catch (Exception ex)
            {
                AppLogManager("GISB Generation in Receiving", "Error", ex.Message, ForNom);
                Gisb = "";
            }
            return Gisb;
        }
        private string SignedGISB(string privateKey, string privatekeyPass, string currentDate, string status, int transid, bool isTest, bool ForNom)
        {
            try
            {
                StringBuilder gisbToSign = new StringBuilder();
                gisbToSign.Append("\r\n\r\n--NAESB7867\r\n");
                gisbToSign.Append("content-type:text/html\r\n");
                gisbToSign.Append("time-c=" + currentDate + "*</br>");
                gisbToSign.Append("request-status=" + status + "*</br>");
                if (isTest)
                    gisbToSign.Append("server-id=test.enercross.com*</br>");
                else
                    gisbToSign.Append("server-id=prod.enercross.com*</br>");
                gisbToSign.Append("trans-id=" + transid + "*</br>");
                gisbToSign.Append("--NAESB7867\r\n");
                gisbToSign.Append("Content-type: text/plain\r\n");
                gisbToSign.Append("time-c=" + currentDate + "* ");
                gisbToSign.Append("request-status=" + status + "* ");
                gisbToSign.Append("server-id=naesb.enercross.com* ");
                gisbToSign.Append("trans-id=" + transid + "* \r\n");
                gisbToSign.Append("--NAESB7867--\r\n");

                //string privateKey = File.OpenRead(privateKeyPath);

                return CryptoService.SignGISB(gisbToSign.ToString(), privateKey, privatekeyPass);
            }
            catch (Exception ex)
            {
                GenerateAndSendErrorMail("Receive web in signing GISB.", ex.Message, isTest, ForNom);
                return string.Empty;
            }
        }
        private void GenerateAndSendErrorMail(string source, string message, bool isTest, bool ForNom)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("%Exception", message);
                if (isTest)
                {
                    string sendMailTo = ConfigurationManager.AppSettings["SendMail"];
                    if (isTest)
                        param.Add("%Source", source + " " + "On test server.");
                    else
                        param.Add("%Source", source + " " + "On prod server.");

                    Email email = new Email(serviceEmailTemplate, serviceEmailQueue, serviceAppLog);
                    email.SendEmail(new string[] { sendMailTo, "lakhwinder.singh@invertedi.com" }, null, null, (int)TemplateType.ErrorNotification, param, "");
                }
            }
            catch (Exception ex)
            {
                AppLogManager("Receiving Web sending mail", "Error", ex.Message, ForNom);
            }
        }
        #region POC for seperate files
        private int IdentifyFileReceived(string decFile, string shipperDuns)
        {
            int DatasetRec = 0;
            try
            {
                if (!String.IsNullOrEmpty(decFile))
                {
                    EDIFileType fileType = ReturnEDIFileType(decFile);
                    switch (fileType)
                    {
                        //case EDIFileType.Ack:
                        //    break;
                        case EDIFileType.NMQR:
                            DatasetRec = (int)Dataset.NMQR;
                            break;
                        case EDIFileType.SQTS:
                            DatasetRec = (int)Dataset.SQTS;
                            break;
                        case EDIFileType.SQTSOP:
                            DatasetRec = (int)Dataset.SQOP;
                            break;
                            //case EDIFileType.OACY:
                            //    break;
                            //case EDIFileType.RURD:
                            //    break;
                            //case EDIFileType.SWNT:
                            //    break;
                            //case EDIFileType.Unknown:
                            //    break;
                            //case EDIFileType.UNSC:
                            //    break;
                            //default:
                            //    break;
                    }
                }
                return DatasetRec;
            }
            catch (Exception ex)
            {
                return DatasetRec;
            }

        }
        public StringBuilder DecryptFile(StringBuilder decFile, string shipperDuns, string _senderDuns)
        {
            //Pipeline pipe = null;
            string passPhrase = "", privateKey = "";
            PGPLib pgp = new PGPLib();
            string end = "-----END PGP MESSAGE-----";
            string Start = "-----BEGIN PGP MESSAGE-----";
            int pos1 = decFile.ToString().IndexOf(Start);
            int pos2 = decFile.ToString().IndexOf(end) + end.Length;
            StringBuilder dataRmvContent = new StringBuilder(decFile.ToString().Substring(pos1, pos2 - pos1));
            StringBuilder recEDI = null;
            if (shipperDuns == "078711334")
            {
                passPhrase = _serviceSetting.GetById((int)Settings.Passphrase).Value;
                privateKey = _serviceSetting.GetById((int)Settings.EnecrossPrivateKey).Value;
            }
            else if (shipperDuns == "101069263")
            {
                if (_senderDuns == "116025180")
                {
                    passPhrase = _serviceSetting.GetById((int)Settings.EmeraPassphrase).Value;
                    privateKey = _serviceSetting.GetById((int)Settings.Emera2048_PvtKey).Value;
                }
                else
                {
                    passPhrase = _serviceSetting.GetById((int)Settings.EmeraPassphrase).Value;
                    privateKey = _serviceSetting.GetById((int)Settings.EmeraPrivateKey).Value;
                }

            }
            else if (shipperDuns == "832703297")
            {
                passPhrase = _serviceSetting.GetById((int)Settings.Passphrase).Value;
                privateKey = _serviceSetting.GetById((int)Settings.EnecrossPrivateKey).Value;
            }
            recEDI = new StringBuilder(pgp.DecryptString(dataRmvContent.ToString(), privateKey, passPhrase));
            if (recEDI != null)
            {
                int pos = recEDI.ToString().IndexOf("ISA");
                decFile = recEDI.Remove(0, pos);
            }
            return decFile;
        }
        public EDIFileType ReturnEDIFileType(string _ediFile)
        {
            char dataSeperator = Convert.ToChar(_ediFile[(_ediFile.IndexOf("ISA") + "ISA".Length)]);
            char segmentSeperator = Convert.ToChar(_ediFile[(_ediFile.IndexOf("GS") - 1)]);
            string[] _segments = _ediFile.Trim().Split(segmentSeperator);

            EDIFileType fileType = EDIFileType.Unknown;
            var stQuery = from item in _segments
                          where item.StartsWith("ST")
                          select item;

            string[] stLine = stQuery.FirstOrDefault().Split(dataSeperator);
            string fileCode = stLine[1];
            switch (fileCode)
            {
                case "846":
                    fileType = EDIFileType.RURD;
                    break;
                case "873":
                    var bgnQuery = from item in _segments
                                   where item.StartsWith("BGN")
                                   select item;
                    string[] bgnLine = bgnQuery.FirstOrDefault().Split(dataSeperator);
                    string capFileCode = bgnLine[7];
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
                        case "Q2":
                            fileType = EDIFileType.SQTSOP;
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
        #endregion
        private void AppLogManager(string source, string type, string description, bool ForNom)
        {
            if (ForNom)
            {
                ApplicationLog log = new ApplicationLog();
                log.Source = source;
                log.Type = type;
                log.Description = description;
                log.CreatedDate = DateTime.Now;
                _applicationLogs.Add(log);
                _applicationLogs.Save();
            }
            else
            {
                UPRD.Model.ApplicationLog log = new UPRD.Model.ApplicationLog();
                log.Source = source;
                log.Type = type;
                log.Description = description;
                log.CreatedDate = DateTime.Now;
                uprdAppLogRepo.Add(log);
                uprdAppLogRepo.Save();
            }

        }

        #region Data Processing Methods
        private List<NMQRPerTransaction> FillUpNMQRData(string ediFile, string fileName, char dataSeperator, char segmentSeperator)
        {
            string pipelineDUNS = ediFile.Substring(35, 9);

            NMQR_DS ediNmqr = new NMQR_DS(ediFile
                , new char[] { segmentSeperator }//segement seperators
                , new char[] { dataSeperator }
                , fileName);
            return modalFactory.Create(ediNmqr.ReadNMQRFileData());
        }
        #endregion
    }
    public class CryptoService
    {
        IApplicationLogRepository _applicationLogs;
        IUPRDApplicationLogRepository _uprdApplicationLogs;
        public CryptoService(IApplicationLogRepository applicationLogs, IUPRDApplicationLogRepository uprdApplicationLogs)
        {
            this._applicationLogs = applicationLogs;
            this._uprdApplicationLogs = uprdApplicationLogs;
        }
        public string EncryptGisb(string signedGISBPath, string encKey, string unSignedGISBPath, bool ForNom)
        {
            string signedContent = "";
            try
            {
                PGPLib pgp = new PGPLib();
                pgp.EncryptFile(unSignedGISBPath, encKey, signedGISBPath, true);
                StreamReader signedFile = new StreamReader(signedGISBPath);
                signedContent = signedFile.ReadToEnd();
                signedFile.Close();
            }
            catch (Exception ex)
            {
                AppLogManager("GISB Signed(Enc) in Receiving", "Error", ex.Message, ForNom);
                signedGISBPath = "";
            }
            return signedContent;
        }

        public static string SignGISB(string stringToSign, string PrivateKey, string privateKeyPass)
        {
            PGPLib pgp = new PGPLib();
            return pgp.SignString(stringToSign, PrivateKey, privateKeyPass);
        }
        private void AppLogManager(string source, string type, string description, bool ForNom)
        {
            if (ForNom)
            {
                ApplicationLog log = new ApplicationLog();
                log.Source = source;
                log.Type = type;
                log.Description = description;
                log.CreatedDate = DateTime.Now;
                _applicationLogs.Add(log);
                _applicationLogs.Save();
            }
            else
            {
                UPRD.Model.ApplicationLog log = new UPRD.Model.ApplicationLog();
                log.Source = source;
                log.Type = type;
                log.Description = description;
                log.CreatedDate = DateTime.Now;
                _uprdApplicationLogs.Add(log);
                _uprdApplicationLogs.Save();
            }

        }
    }
}
