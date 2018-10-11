using DidiSoft.Pgp;
using Ninject;
using Nom1Done.Data.Repositories;
using Nom1Done.DTO;
using Nom1Done.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using EdiTools.EDIGenerator;
using Nom1Done.Service.Interface;
using Nom1Done.Service;
using Nom.ViewModel;

namespace Nom1Done.EDIEngineSendAndReceive
{
    public class SendPartFunctions
    {
        #region Annum Store(Services)
        IBatchRepository _serviceBatch;
        IOutboxRepository _serviceOutbox;
        ITransactionLogRepository _serviceTransactionLog;
        IApplicationLogRepository _applicationLogs;
        IJobWorkflowRepository _serviceJobWorkFlow;
        INominationsRepository _serviceNomination;
        IJobStackErrorLogRepository _serviceJobStackErrorLog;
        ITaskMgrJobsRepository _serviceTaskMgrJob;
        ITaskMgrXmlRepository _serviceTaskMgrXml;
        ImetadataPipelineEncKeyInfoRepository _servicemetadataPipelineEncKeyInfo;
        IPipelineRepository _servicePipeline;
        ITransportationServiceProviderRepository _serviceTSP;
        IShipperCompanyRepository _serviceShipperCompany;
        ITradingPartnerWorksheetRepository _serviceTPW;
        ImetadataDatasetRepository _servicemetadataDataset;
        IGISBOutboxRepository _serviceGISBOutbox;
        INominationStatusRepository _serviceNomStatus;
        ISQTSTrackOnNomRepository _serviceSqtsTrackOnNom;
        ImetadataCycleRepository _servicemetadataCycle;
        ISettingRepository _serviceSetting;
        IOutbox_MultipartFormRepository _serviceOutboxMultipartForm;
        IPipelineEDISettingRepository _servicePipelineEdiSetting;
        IModalFactory _modalFactory;
        IShipperRepository _serviceShipper;
        IUPRDStatuRepository _serviceUprdStatus;
        string iiPublicKey;
        #endregion
        #region Annum (Services Initializer)
        public SendPartFunctions()
        {
            StandardKernel Kernal = new StandardKernel();
            Kernal.Load(Assembly.GetExecutingAssembly());
            _serviceBatch = Kernal.Get<BatchRepository>();
            _serviceOutbox = Kernal.Get<OutboxRepository>();
            _serviceTransactionLog = Kernal.Get<TransactionLogRepository>();
            _applicationLogs = Kernal.Get<ApplicationLogRepository>();
            _serviceJobWorkFlow = Kernal.Get<JobWorkflowRepository>();
            _serviceNomination = Kernal.Get<NominationsRepository>();
            _serviceJobStackErrorLog = Kernal.Get<JobStackErrorLogRepository>();
            _serviceTaskMgrJob = Kernal.Get<TaskMgrJobsRepository>();
            _serviceTaskMgrXml = Kernal.Get<TaskMgrXmlRepository>();
            _servicemetadataPipelineEncKeyInfo = Kernal.Get<metadataPipelineEncKeyInfoRepository>();
            _servicePipeline = Kernal.Get<PipelineRepository>();
            _serviceTSP = Kernal.Get<TransportationServiceProviderRepository>();
            _serviceShipperCompany = Kernal.Get<ShipperCompanyRepository>();
            _serviceTPW = Kernal.Get<TradingPartnerWorksheetRepository>();
            _servicemetadataDataset = Kernal.Get<metadataDatasetRepository>();
            _serviceGISBOutbox = Kernal.Get<GISBOutboxRepository>();
            _serviceNomStatus = Kernal.Get<NominationStatusRepository>();
            _serviceSqtsTrackOnNom = Kernal.Get<SQTSTrackOnNomRepository>();
            _servicemetadataCycle = Kernal.Get<metadataCycleRepository>();
            _serviceSetting = Kernal.Get<SettingRepository>();
            _serviceOutboxMultipartForm = Kernal.Get<Outbox_MultipartFormRepository>();
            _servicePipelineEdiSetting = Kernal.Get<PipelineEDISettingRepository>();
            _serviceShipper = Kernal.Get<ShipperRepository>();
            _modalFactory = Kernal.Get<ModalFactory>();
            _serviceUprdStatus = Kernal.Get<UPRDStatuRepository>();
        }
        #endregion
        #region Auto KING UPRD
        public void PlaceUprdJobAndProcess(bool IsOacy,bool IsUnsc,bool IsSwnt)
        {
            bool IsTest = Convert.ToBoolean(_serviceSetting.GetById((int)Settings.UprdEDIForTest).Value);
            List<Pipeline> pipes = _servicePipeline.GetActiveUprdPipelines().ToList();
            pipes.ForEach(a => RaiseAndSendUprdRequest(a,IsOacy,IsUnsc,IsSwnt,IsTest,false));
        }
        private void RaiseAndSendUprdRequest(Pipeline pipe, bool isOacy, bool isUnsc, bool isSwnt, bool isTest,bool sendManu)
        {
            try
            {
                Guid transactionId = Guid.NewGuid();
                TaskMgrJob job = new TaskMgrJob();
                job.TransactionId = transactionId.ToString();
                job.DatasetId = 11;
                job.CreatedAt = DateTime.Now;
                job.EDICheckCount = 0;
                job.EndDate = DateTime.MaxValue;
                job.StartDate = DateTime.Now;
                job.IsSending = true;
                job.StageId = 3;
                job.StatusId = 2;
                job.NoOfXmlInEDI = 0;
                _serviceTaskMgrJob.Add(job);
                _serviceTaskMgrJob.Save();

                Outbox outbox = new Outbox();
                outbox.MessageID = transactionId;
                outbox.DatasetID = 11;
                outbox.StatusID = 0;
                outbox.GISB = "";
                outbox.IsTest = isTest;
                outbox.PipelineID = pipe.ID;
                outbox.CompanyID = 2;
                outbox.IsScheduled = false;
                outbox.ScheduledDate = DateTime.MaxValue;
                outbox.IsActive = true;
                outbox.CreatedBy = "8f76c512-199c-47f0-8d9e-1653d4e200dd";
                outbox.CreatedDate = DateTime.Now;
                outbox.ModifiedBy = "8f76c512-199c-47f0-8d9e-1653d4e200dd";
                outbox.ModifiedDate = DateTime.Now;
                _serviceOutbox.Add(outbox);
                _serviceOutbox.Save();

                #region Edi Generation
                Int64 workFlowId = Int64.MinValue;
                AddUpdateTransactionLog(job.TransactionId, (int)Statuses.InProgress);
                JobWorkflow workFlow = new JobWorkflow();
                workFlow.EndDate = DateTime.MaxValue;
                workFlow.StageId = (int)SendingStages.XML;
                workFlow.StartDate = DateTime.Now;
                workFlow.StatusId = (int)Statuses.InProgress;
                workFlow.TransactionId = Guid.Parse(job.TransactionId);
                _serviceJobWorkFlow.Add(workFlow);
                _serviceJobWorkFlow.Save();
                workFlowId = workFlow.WorkflowId;
                var pipelineEdiSetting = _modalFactory.Parse(_servicePipelineEdiSetting.GetPipelineSetting(pipe.DUNSNo, (int)DataSet.Upload_of_Requests_for_Download_of_Posted_Datasets, "078711334"));
                if (pipelineEdiSetting != null)
                {
                    UPRD_GN ediGen = new UPRD_GN();
                    var ediFile= ediGen.GenerateEDIUPRD(isOacy, isUnsc, isSwnt, isTest, pipe.DUNSNo, pipelineEdiSetting, sendManu);
                    if (!string.IsNullOrEmpty(ediFile.ToString()))
                    {
                        var r = ediFile.Segments.Where(a => a.Id.Contains("BIA")).FirstOrDefault();
                        string requestID = !string.IsNullOrEmpty(r.Elements[2].ToString()) ? r.Elements[2].ToString() : string.Empty;
                        
                        #region add UPRD status
                        UPRDStatu uprdStatus = new UPRDStatu();
                        uprdStatus.TransactionId = outbox.MessageID;
                        uprdStatus.RequestID = requestID;
                        uprdStatus.CreatedDate = DateTime.Now;
                        if (isOacy)
                        {
                            uprdStatus.DatasetRequested = (int)DataSet.Operational_Capacity;
                            uprdStatus.DatasetSummary = "OACY";
                        }
                        if (isUnsc)
                        {
                            uprdStatus.DatasetRequested = (int)DataSet.Unsubscribed_Capacity;
                            uprdStatus.DatasetSummary = "UNSC";
                        }
                        if (isSwnt)
                        {
                            uprdStatus.DatasetRequested = (int)DataSet.System_Wide_Notices;
                            uprdStatus.DatasetSummary = "SWNT";
                        }
                        uprdStatus.PipeDuns = pipe.DUNSNo;
                        _serviceUprdStatus.Add(uprdStatus);
                        _serviceUprdStatus.Save();
                        #endregion

                        SaveXmlToDB(job.TransactionId, pipe.ID, null, ediFile.ToString(), null, null);
                        UpdateJobStatus(job.TransactionId, (int)Statuses.Completed, (int)SendingStages.EDI);
                        UpdateWorkflow(workFlowId, (int)Statuses.Completed);
                    }
                    else
                    {
                        UpdateJobStatus(job.TransactionId, (int)Statuses.Error, (int)SendingStages.EDI);
                        UpdateWorkflow(workFlowId, (int)Statuses.Error);
                    }
                }
                else
                {
                    SaveErrorLogs("Generate EDI UPRD Task Manager Send " + job.TransactionId + " Pipeline EDI Setting is null. for pipeline " + pipe.Name + " DunsNo:- " + pipe.DUNSNo, Guid.Parse(job.TransactionId), workFlowId, (int)SendingStages.EDI);
                    AppLogManager(new StringBuilder("Generate EDI UPRD Task Manager Send " + job.TransactionId), "Error", new StringBuilder("Pipeline EDI setting in Null for UPRD file for pipeline " + pipe.Name + " Duns NO " + pipe.DUNSNo));
                    UpdateJobStatus(job.TransactionId, (int)Statuses.Error, (int)SendingStages.EDI);
                    UpdateWorkflow(workFlowId, (int)Statuses.Error);
                }
                #endregion
                #region Encryption
                workFlow = new JobWorkflow();
                workFlow.EndDate = DateTime.MaxValue;
                workFlow.StageId = (int)SendingStages.EncryptedEDI;
                workFlow.StartDate = DateTime.Now;
                workFlow.StatusId = (int)Statuses.InProgress;
                workFlow.TransactionId = Guid.Parse(job.TransactionId);
                _serviceJobWorkFlow.Add(workFlow);
                _serviceJobWorkFlow.Save();
                workFlowId = workFlow.WorkflowId;/*GetJobWorkFlow(job.TransactionId, SendingStages.EncryptedEDI, Status.InProgress);*/
                StartEncryptingEDI(job, workFlowId);
                #endregion
                #region Sending
                UpdateJobStatus(job.TransactionId, (int)Statuses.InProgress, (int)SendingStages.SendingFile);
                Int64 workflowId = Int64.MinValue;
                workFlow = new JobWorkflow();
                workFlow.EndDate = DateTime.MaxValue;
                workFlow.StageId = (int)SendingStages.SendingFile;
                workFlow.StartDate = DateTime.Now;
                workFlow.StatusId = (int)Statuses.InProgress;
                workFlow.TransactionId = Guid.Parse(job.TransactionId);
                _serviceJobWorkFlow.Add(workFlow);
                _serviceJobWorkFlow.Save();
                workflowId = workFlow.WorkflowId;
                //Console.WriteLine("Sending files updated status of job in DB.");
                FileSendingManager(job, workflowId);
                #endregion
            }
            catch (Exception ex)
            {
                AppLogManager(new StringBuilder("Raise UPRD in Task manager send"), "Error", new StringBuilder(ex.Message+" Inner Exception:- "+ex.InnerException));
            }
        }
        #endregion
        #region Manual KING-Nomination/UPRD(OACY,UNSC,SWNT) 
        public void PendingNominationJobs()
        {
            try
            {
                #region troops(Nomination Manual)
                TaskMgrJob pendingJob = _serviceTaskMgrJob.GetPendingJobSend((int)Statuses.NotProcessed, (int)SendingStages.Initial);//GetAll().Where(a => a.StatusId == (int)Statuses.NotProcessed && a.StageId == (int)SendingStages.Initial && a.IsSending == true).OrderBy(a=>a.CreatedAt).FirstOrDefault();
                if (pendingJob != null)
                {
                    UpdateJobStatus(pendingJob.TransactionId, (int)Statuses.InProgress, (int)SendingStages.Initial);
                    
                    try
                    {
                        V4_Batch batch = _serviceBatch.GetByTransactionID(Guid.Parse(pendingJob.TransactionId));
                        
                        AddUpdateTransactionLog(pendingJob.TransactionId, (int)Statuses.InProgress);
                        JobWorkflow workFlow = new JobWorkflow();
                        workFlow.EndDate = DateTime.MaxValue;
                        workFlow.StageId = (int)SendingStages.XML;
                        workFlow.StartDate = DateTime.Now;
                        workFlow.StatusId = (int)Statuses.InProgress;
                        workFlow.TransactionId = Guid.Parse(pendingJob.TransactionId);
                        _serviceJobWorkFlow.Add(workFlow);
                        _serviceJobWorkFlow.Save();
                        #region take&Process job of Nom generator(process)
                        MaintainLogNominationEDIJobs(pendingJob, batch.PipelineID, workFlow);
                        #endregion
                        
                    }
                    catch (Exception ex)
                    {
                        AppLogManager(new StringBuilder("ManagePendingXMLJobs Task Manager send"), "Error", new StringBuilder(ex.Message));
                        //Console.WriteLine("ManagePendingXMLJobs Exception occur while updation of job Status : " + ex.Message);
                    }
                    //Console.WriteLine("XML generation invoke XML Generator thread.");
                }
                #endregion
                #region troops of doom(UPRD Manual)
                PipelineEDISetting pipelineEDISetting = _servicePipelineEdiSetting.GetPipelineSettingForManuallySend((int)DataSet.Upload_of_Requests_for_Download_of_Posted_Datasets, "078711334");
                if (pipelineEDISetting != null)
                {
                    pipelineEDISetting.SendManually = false;
                    _servicePipelineEdiSetting.Update(pipelineEDISetting);
                    _servicePipelineEdiSetting.SaveChanges();
                    bool IsTest = Convert.ToBoolean(_serviceSetting.GetById((int)Settings.UprdEDIForTest).Value);
                    Pipeline a = _servicePipeline.GetPipelineByDuns(pipelineEDISetting.PipeDuns);
                    RaiseAndSendUprdRequest(a, pipelineEDISetting.ForOacy, pipelineEDISetting.ForUnsc, pipelineEDISetting.ForSwnt, IsTest, true);
                }
                #endregion
            }
            catch (Exception ex)
            {
                AppLogManager(new StringBuilder("ManagePendingXMLJobs Task Manager Send"), "Error", new StringBuilder(ex.Message));
            }
            //Console.WriteLine("Xml generator timer starts looking for another job.");
        }
        #endregion
        #region NOM Manager
        /// <summary>
        /// Nom manager
        /// </summary>
        /// <param name="pendingJob">task identity</param>
        /// <param name="pipeID">TSP identity</param>
        /// <param name="workFlow">task workflow</param>
        private void MaintainLogNominationEDIJobs(TaskMgrJob pendingJob, int pipeID, JobWorkflow workFlow)
        {
            #region Nomination generator
            StringBuilder GeneratedNomEDI = new StringBuilder(GenerateEDINomination(pendingJob.TransactionId, pipeID, workFlow.WorkflowId));
            if (!String.IsNullOrEmpty(GeneratedNomEDI.ToString()))
            {
                SaveXmlToDB(pendingJob.TransactionId, pipeID, null, GeneratedNomEDI.ToString(), null, null);
                UpdateJobStatus(pendingJob.TransactionId, (int)Statuses.Completed, (int)SendingStages.EDI);
                UpdateWorkflow(workFlow.WorkflowId, (int)Statuses.Completed);
            }
            else
            {
                UpdateJobStatus(pendingJob.TransactionId, (int)Statuses.Error, (int)SendingStages.EDI);
                UpdateWorkflow(workFlow.WorkflowId, (int)Statuses.Error);
                SaveErrorLogs("Generate EDI Nomination Task Manager Send " + pendingJob.TransactionId + " Edi not generated (Empty file received).", Guid.Parse(pendingJob.TransactionId), workFlow.WorkflowId, (int)SendingStages.EDI);
            }
            #endregion
            #region Encryption
            workFlow = new JobWorkflow();
            workFlow.EndDate = DateTime.MaxValue;
            workFlow.StageId = (int)SendingStages.EncryptedEDI;
            workFlow.StartDate = DateTime.Now;
            workFlow.StatusId = (int)Statuses.InProgress;
            workFlow.TransactionId = Guid.Parse(pendingJob.TransactionId);
            _serviceJobWorkFlow.Add(workFlow);
            _serviceJobWorkFlow.Save();
            //workFlowId = workFlow.WorkflowId;/*GetJobWorkFlow(job.TransactionId, SendingStages.EncryptedEDI, Status.InProgress);*/
            StartEncryptingEDI(pendingJob, workFlow.WorkflowId);
            #endregion
            #region Sending
            UpdateJobStatus(pendingJob.TransactionId, (int)Statuses.InProgress, (int)SendingStages.SendingFile);
            workFlow = new JobWorkflow();
            workFlow.EndDate = DateTime.MaxValue;
            workFlow.StageId = (int)SendingStages.SendingFile;
            workFlow.StartDate = DateTime.Now;
            workFlow.StatusId = (int)Statuses.InProgress;
            workFlow.TransactionId = Guid.Parse(pendingJob.TransactionId);
            _serviceJobWorkFlow.Add(workFlow);
            _serviceJobWorkFlow.Save();
            //Console.WriteLine("Sending files updated status of job in DB.");
            FileSendingManager(pendingJob, workFlow.WorkflowId);
            #endregion
        }
        public string GenerateEDINomination(string TransactionId, int pipeID, Int64 workFlowId)
        {
            try
            {
                bool nomForTest = Convert.ToBoolean(_serviceSetting.GetById((int)Settings.NomEDIForTest).Value);
                Pipeline pipe = _servicePipeline.GetById(pipeID);
                BatchDTO batch = _modalFactory.Parse(_serviceBatch.GetByTransactionID(Guid.Parse(TransactionId)));
                if (batch != null && batch.NomTypeID == (int)NomType.PNT)
                {
                    List<V4_NominationDTO> NomList = _modalFactory.Parse(_serviceNomination.GetAllNomsByTransactionId(Guid.Parse(TransactionId)));
                    int shipperCompId = _serviceShipper.GetbyUserId(batch.CreatedBy).ShipperCompanyID;
                    string shipperCompDuns = _serviceShipperCompany.GetById(shipperCompId).DUNS;
                    var pipelineEdiSetting = _modalFactory.Parse(_servicePipelineEdiSetting.GetPipelineSetting(pipe.DUNSNo, (int)DataSet.Nomination_PNT, shipperCompDuns));
                    if (NomList.Count() > 0 && pipelineEdiSetting != null)
                    {
                        NMST_GN ediGen = new NMST_GN();
                        var ediFile = ediGen.GenerateEDIPNTNomination(batch, NomList, pipelineEdiSetting, nomForTest);
                        var SLNList = ediFile.Segments.Where(a => a.Id.Contains("SLN")).ToList();
                        foreach (var sln in SLNList)
                        {
                            var trackId = !string.IsNullOrEmpty(sln.Elements[0].ToString()) ? sln.Elements[0].ToString() : string.Empty;
                            if (!string.IsNullOrEmpty(trackId))
                            {
                                AddSqtsTrackOnNom(TransactionId, trackId);
                            }
                        }
                        return ediFile.ToString();
                    }
                    else
                    {
                        throw new Exception("Nomination or pipeline edi setting is null.");
                    }
                }
                else if (batch != null && batch.NomTypeID == (int)NomType.Pathed)
                {
                    List<V4_NominationDTO> NomList = _modalFactory.Parse(_serviceNomination.GetAllNomsByTransactionId(Guid.Parse(TransactionId)));
                    int shipperCompId = _serviceShipper.GetbyUserId(batch.CreatedBy).ShipperCompanyID;
                    string shipperCompDuns = _serviceShipperCompany.GetById(shipperCompId).DUNS;
                    var pipelineEdiSetting = _modalFactory.Parse(_servicePipelineEdiSetting.GetPipelineSetting(pipe.DUNSNo, (int)DataSet.Nomination, shipperCompDuns));
                    if (NomList.Count() > 0 && pipelineEdiSetting != null)
                    {
                        NMST_GN ediGen = new NMST_GN();
                        var ediFile = ediGen.GenerateEDIPathedNomination(batch, NomList.FirstOrDefault(), pipelineEdiSetting, nomForTest);
                        var SLNList = ediFile.Segments.Where(a => a.Id.Contains("SLN")).ToList();
                        foreach (var sln in SLNList)
                        {
                            var trackId = !string.IsNullOrEmpty(sln.Elements[0].ToString()) ? sln.Elements[0].ToString() : string.Empty;
                            if (!string.IsNullOrEmpty(trackId))
                            {
                                AddSqtsTrackOnNom(TransactionId, trackId);
                            }
                        }
                        return ediFile.ToString();
                    }
                    else
                    {
                        throw new Exception("Nomination or pipeline edi setting is null.");
                    }
                }
                else if (batch != null && batch.NomTypeID == (int)NomType.NonPathed)
                {
                    List<V4_NominationDTO> Nom= _modalFactory.Parse(_serviceNomination.GetAllNomsByTransactionId(Guid.Parse(TransactionId)));
                    int shipperCompId = _serviceShipper.GetbyUserId(batch.CreatedBy).ShipperCompanyID;
                    string shipperCompDuns = _serviceShipperCompany.GetById(shipperCompId).DUNS;
                    var pipelineEdiSetting = _modalFactory.Parse(_servicePipelineEdiSetting.GetPipelineSetting(pipe.DUNSNo, (int)DataSet.Nomination, shipperCompDuns));
                    if (Nom != null && pipelineEdiSetting != null)
                    {
                        NMST_GN ediGen = new NMST_GN();
                        var ediFile = ediGen.GenerateEDINonPathNomination(batch, Nom, pipelineEdiSetting, nomForTest);
                        var SLNList = ediFile.Segments.Where(a => a.Id.Contains("SLN")).ToList();
                        foreach (var sln in SLNList)
                        {
                            var trackId = !string.IsNullOrEmpty(sln.Elements[0].ToString()) ? sln.Elements[0].ToString() : string.Empty;
                            if (!string.IsNullOrEmpty(trackId))
                            {
                                AddSqtsTrackOnNom(TransactionId, trackId);
                            }
                        }
                        return ediFile.ToString();
                    }
                    else
                    {
                        throw new Exception("Nomination or pipeline edi setting is null.");
                    }
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                SaveErrorLogs("GenerateEDI Nomination Task Manager Send " + TransactionId + ex.Message, Guid.Parse(TransactionId), workFlowId, (int)SendingStages.XML);
                UpdateJobStatus(TransactionId, (int)Statuses.Error, (int)SendingStages.XML);
                UpdateTransactionLogs(TransactionId, (int)Statuses.Error);
                UpdateWorkflow(workFlowId, (int)Statuses.Error);
                AppLogManager(new StringBuilder("GenerateEDI Nomination Task Manager Send " + TransactionId), "Error", new StringBuilder(ex.Message));
                return string.Empty;
            }
        }
        #endregion
        #region Encryptor of DOC(UPRD/NOM)
        /// <summary>
        /// Encryptor for all docs
        /// </summary>
        /// <param name="job">task identity</param>
        /// <param name="workFlowId">task workflow logging</param>
        private void StartEncryptingEDI(TaskMgrJob job, Int64 workFlowId)
        {
            try
            {
                TaskMgrXml EDIFile = _serviceTaskMgrXml.GetbyTransactionId(job.TransactionId);
                if (EDIFile != null)
                {
                    string encEDIFile = null;
                    metadataPipelineEncKeyInfo keyInfo = _servicemetadataPipelineEncKeyInfo.GetByPipelineId(EDIFile.PipelineId);
                    string PgpKey = keyInfo.PgpKey;

                    var iiTestOn = _serviceSetting.GetById((int)Settings.iiTestOn).Value;
                    if (Convert.ToBoolean(iiTestOn))
                    {
                        var releaseToShipper = _serviceSetting.GetById((int)Settings.releaseToShipper).Value;
                        ShipperCompany shipComp = null;
                        if (!string.IsNullOrEmpty(releaseToShipper))
                            shipComp = _serviceShipperCompany.GetShipperCompanyByDuns(releaseToShipper);
                        if (shipComp.ID == (int)ShipperComps.Enercross)
                        {
                            iiPublicKey = _serviceSetting.GetById((int)Settings.iiPublicKey).Value;
                        }
                        if (shipComp.ID == (int)ShipperComps.Emera)
                        {
                            iiPublicKey = _serviceSetting.GetById((int)Settings.emeraPublicKey).Value;
                        }
                        encEDIFile = EncryptFile(iiPublicKey, EDIFile.EDIData);
                    }
                    else
                        encEDIFile = EncryptFile(PgpKey, EDIFile.EDIData);

                    AppLogManager(new StringBuilder(job.TransactionId), "Info", new StringBuilder("Encrypted by " + keyInfo.KeyName));
                    SaveXmlToDB(job.TransactionId, EDIFile.PipelineId, null, null, encEDIFile, null);
                    UpdateJobStatus(job.TransactionId, (int)Statuses.Completed, (int)SendingStages.EncryptedEDI);
                    UpdateTransactionLogs(job.TransactionId, (int)Statuses.Completed);
                    UpdateWorkflow(workFlowId, (int)Statuses.Completed);
                }
                else
                {
                    UpdateJobStatus(job.TransactionId, (int)Statuses.Error, (int)SendingStages.EncryptedEDI);
                    UpdateTransactionLogs(job.TransactionId, (int)Statuses.Error);
                    UpdateWorkflow(workFlowId, (int)Statuses.Error);
                }
            }
            catch (Exception ex)
            {
                SaveErrorLogs("StartEncryptingEDI Task Manager Send " + ex.Message, Guid.Parse(job.TransactionId), workFlowId, (int)SendingStages.EncryptedEDI);
                UpdateJobStatus(job.TransactionId, (int)Statuses.Error, (int)SendingStages.EncryptedEDI);
                UpdateTransactionLogs(job.TransactionId, (int)Statuses.Error);
                UpdateWorkflow(workFlowId, (int)Statuses.Error);
                AppLogManager(new StringBuilder("StartEncryptingEDI Task Manager send"), "Error", new StringBuilder(ex.Message));
            }
        }
        #region Encryption Using DidiSoft 
        public string EncryptFile(string keyPath, string ediToEnc)
        {
            try
            {
                PGPLib pgp = new PGPLib();
                return pgp.EncryptString(ediToEnc, keyPath);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }
        #endregion
        #endregion
        #region Sending DOC(UPRD/NOM) to TSP
        private void FileSendingManager(TaskMgrJob job, Int64 workflowId)
        {
            try
            {
                //***Request:- Pipeline, IsPipe_UseTspDuns, FileInBytes, PostParams, ReqToTspRetResponse.
                HttpWebResponse response = CollectingDataForSendingFile(job.TransactionId);
                //***Response:- ProcessResponse, UpdateTransaction, JobWorkFlow, storeGISB, Send mail.
                bool GISBReceived = ProcessResponseFromTsp(response, job.TransactionId);
                if (GISBReceived)
                {
                    SendMailOfSendingFile(job.TransactionId, GISBReceived);
                    UpdateJobStatus(job.TransactionId, (int)Statuses.Completed, (int)SendingStages.SendingFile);
                    UpdateTransactionLogs(job.TransactionId, (int)Statuses.Completed);
                    UpdateWorkflow(workflowId, (int)Statuses.Completed);
                }
                else
                {
                    var outbox = _serviceOutbox.GetByTransactionId(Guid.Parse(job.TransactionId));
                    outbox.GISB = "Gisb not received";
                    outbox.StatusID = (int)Statuses.Error;
                    _serviceOutbox.Update(outbox);
                    SendMailOfSendingFile(job.TransactionId, GISBReceived);
                    UpdateJobStatus(job.TransactionId, (int)Statuses.Error, (int)SendingStages.SendingFile);
                    UpdateTransactionLogs(job.TransactionId, (int)Statuses.Error);
                    UpdateWorkflow(workflowId, (int)Statuses.Error);
                }
            }
            catch (Exception ex)
            {
                //Console.Clear();
                //Console.WriteLine("SendNominationFile " + job.TransactionId + " " + ex.Message, Guid.Parse(job.TransactionId), workflowId, (int)SendingStages.SendingFile);
                SaveErrorLogs("SendNominationFile " + job.TransactionId + " " + ex.Message, Guid.Parse(job.TransactionId), workflowId, (int)SendingStages.SendingFile);
                UpdateJobStatus(job.TransactionId, (int)Statuses.Error, (int)SendingStages.SendingFile);
                UpdateTransactionLogs(job.TransactionId, (int)Statuses.Error);
                UpdateWorkflow(workflowId, (int)Statuses.Error);
                AppLogManager(new StringBuilder("File sending manager." + job.TransactionId), "Error", new StringBuilder(ex.Message));
                AppLogManager(new StringBuilder("Full Error Log." + job.TransactionId), "Error", new StringBuilder(ex.ToString()));                
            }
        }
        private void SendMailOfSendingFile(string transactionId, bool GISBReceived)
        {
            try
            {
                Outbox newFile = _serviceOutbox.GetByTransactionId(Guid.Parse(transactionId));
                if (newFile != null)
                {
                    Pipeline pipeline = _servicePipeline.GetById(newFile.PipelineID);
                    Hashtable emailVariables = new Hashtable();
                    metadataDataset dataset = _servicemetadataDataset.GetById(newFile.DatasetID);
                    emailVariables.Add("%FileName", newFile.MessageID.ToString());
                    emailVariables.Add("%CreatedDate", newFile.CreatedDate);
                    emailVariables.Add("%Pipeline", pipeline.Name + "( " + pipeline.DUNSNo + " )");
                    emailVariables.Add("%Environment", newFile.IsTest ? "Test" : "Live");
                    emailVariables.Add("%TypeOfFile", dataset.Name);

                    if (GISBReceived)
                        emailVariables.Add("%GISB", newFile.GISB.Replace("\r\n", "<br>"));
                    else
                        emailVariables.Add("%Status", "Failed. Got empty Gisb in return.");
                    string mailTO = _serviceSetting.GetById((int)Settings.sendMailTo).Value;
                    //Email.SendEmail(new string[] { mailTO }, null, null, (int)TemplateType.FileSentReceipt, emailVariables, newFile.CreatedBy);
                    GISBOutbox outboxGisb = new GISBOutbox();
                    outboxGisb.MessageID = newFile.MessageID.ToString();
                    outboxGisb.Gisb = newFile.GISB;
                    outboxGisb.ErrorCode = newFile.StatusID == (int)NomStatus.Submitted ? 0 : 1;
                    outboxGisb.CreatedDate = DateTime.UtcNow;
                    int indexRequestStatus = newFile.GISB.IndexOf("request-status");
                    int indexServerid = newFile.GISB.IndexOf("server-id");
                    outboxGisb.ErrorDescription = newFile.StatusID == (int)NomStatus.Submitted ? "ok" : newFile.GISB.Substring(indexRequestStatus + 15, indexServerid - (indexRequestStatus + 15 + 3));
                    _serviceGISBOutbox.Add(outboxGisb);
                    _serviceGISBOutbox.Save();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private HttpWebResponse CollectingDataForSendingFile(string transactionId)
        {
            try
            {
                Outbox outbox = _serviceOutbox.GetByTransactionId(Guid.Parse(transactionId));
                if (outbox == null)
                    throw new Exception("Outbox is null for " + transactionId);
                else
                {
                    Pipeline pipe = _servicePipeline.GetById(outbox.PipelineID);
                    if (pipe == null)
                        throw new Exception("Pipeline is null for " + transactionId);
                    else
                    {
                        string TspOrPipeDuns = string.Empty;
                        ShipperCompany shipper = null;
                        if (pipe.ToUseTSPDUNS)
                            TspOrPipeDuns = _serviceTSP.GetById(pipe.TSPId).DUNSNo;
                        else
                            TspOrPipeDuns = pipe.DUNSNo;
                        if (string.IsNullOrEmpty(TspOrPipeDuns))
                            throw new Exception("Duns empty from TSP/Pipe.");
                        else
                        {
                            shipper = _serviceShipperCompany.GetById(outbox.CompanyID);
                            if (shipper == null)
                                throw new Exception("Shippper duns is null for " + transactionId);
                            else
                            {
                                Dictionary<string, object> sendingFilePacket = GeneratePacketOfSendingFile(TspOrPipeDuns, shipper.DUNS, transactionId);
                                return SendFileToTSP(outbox.ID, sendingFilePacket, pipe.ID, outbox.IsTest, shipper.Name, transactionId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private HttpWebResponse SendFileToTSP(int outboxID, Dictionary<string, object> sendingFilePacket, int pipeId, bool isTest, string shipperName, string transactionId)
        {
            //Console.WriteLine("****************************************************************************");
            //Console.WriteLine("\r\nSendzFileToTSP");
            string postURL = "", username = "", password = "", userAgent = "";
            if (Convert.ToBoolean(_serviceSetting.GetById((int)Settings.iiTestOn).Value))
            {
                postURL = ConfigurationManager.AppSettings["TestUrl"];
                username = ConfigurationManager.AppSettings["username"];
                password = ConfigurationManager.AppSettings["password"];
            }
            else
            {
                TradingPartnerWorksheet tpw = _serviceTPW.GetByPipelineId(pipeId);
                if (tpw == null)
                    throw new Exception("TPW is null for pipeline " + pipeId);
                else
                {
                    postURL = isTest ? tpw.URLTest : tpw.URLLive;
                    username = isTest ? tpw.UsernameTest : tpw.UsernameLive;
                    password = isTest ? tpw.PasswordTest : tpw.PasswordLive;
                }
            }
            SaveXmlToDB(transactionId, pipeId, null, null, null, postURL);
            userAgent = isTest ? shipperName + " Test File Client" : shipperName + " Live File Client";
            FormUpload formUpload = new FormUpload(_serviceOutboxMultipartForm);
            return formUpload.MultipartFormDataPost(outboxID, postURL, userAgent, sendingFilePacket, username, password);
        }
        private bool ProcessResponseFromTsp(HttpWebResponse webResponse, string TransactionId)
        {
            try
            {

                //Console.Clear();
                AppLogManager(new StringBuilder("Web Response Status"), "Error", new StringBuilder("Response : " + webResponse.ToString()));
                if (webResponse.StatusCode == HttpStatusCode.OK)
                {

                }
                    //Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}", webResponse.StatusDescription);
                else
                {
                    throw new Exception("web response is null and GISB is not received.");
                }
                  

                var response = webResponse.GetResponseStream();
                AppLogManager(new StringBuilder("0--"), "Error", new StringBuilder("Response : " + response));
                StreamReader responseReader = new StreamReader(response);
                
                string fullResponse = responseReader.ReadToEnd();
                AppLogManager(new StringBuilder("01--"), "Error", new StringBuilder("Response : " + fullResponse));

                if (string.IsNullOrEmpty(fullResponse))
                {
                    AppLogManager(new StringBuilder("Send Nomination File Task Manager Task Manager Send " + TransactionId), "Error", new StringBuilder("In ProcessResponse response is empty."));
                    return false;
                }
                else
                {
                    NominationStatu nomStatus = _serviceNomStatus.GetByTransactionId(Guid.Parse(TransactionId));
                    V4_Batch batch = _serviceBatch.GetByTransactionID(Guid.Parse(TransactionId));
                    Outbox newFile = _serviceOutbox.GetByTransactionId(Guid.Parse(TransactionId));
                    if (newFile != null)
                    {
                        newFile.GISB = fullResponse;
                        if (newFile.GISB.Contains("request-status=ok"))
                            newFile.StatusID = (int)NomStatus.Submitted;
                        else
                            newFile.StatusID = (int)NomStatus.GISBError;
                        _serviceOutbox.Update(newFile);
                        _serviceOutbox.Save();
                    }
                    if (batch != null)
                    {
                        if (nomStatus == null)
                        {
                            nomStatus = new NominationStatu();
                            nomStatus.NOM_ID = Guid.Parse(TransactionId);
                            nomStatus.CreatedDate = DateTime.Now;
                            nomStatus.IsAutomated = false;
                            nomStatus.ReferenceNumber = batch.ReferenceNumber;
                            _serviceNomStatus.Add(nomStatus);
                            _serviceNomStatus.Save();
                        }
                        batch.StatusID = newFile.StatusID;
                        _serviceBatch.Update(batch);
                        _serviceBatch.SaveChages();
                    }
                    TransactionLog Log = _serviceTransactionLog.GetByTransactionId(TransactionId);
                    if (Log != null)
                    {
                        Log.EndDate = DateTime.Now;
                        _serviceTransactionLog.Update(Log);
                        _serviceTransactionLog.Save();
                    }
                }
                return true;
            }
            
            catch (Exception ex)
            {
                AppLogManager(new StringBuilder("SendNominationFile Task Manager Task Manager Send " + TransactionId), "Error", new StringBuilder("In processResponse " + ex.Message));
                AppLogManager(new StringBuilder("1SendNominationFile Task Manager Task Manager Send " + TransactionId), "Error", new StringBuilder(ex.ToString()));
                AppLogManager(new StringBuilder("2SendNominationFile Task Manager Task Manager Send " + TransactionId), "Error", new StringBuilder(ex.StackTrace));
                return false;
            }
        }
        private Dictionary<string, object> GeneratePacketOfSendingFile(string tspOrPipeDuns, string shipperDuns, string transactionId)
        {
            string encEdi = _serviceTaskMgrXml.GetbyTransactionId(transactionId).EncEDIData;
            if (string.IsNullOrEmpty(encEdi))
                throw new Exception("Encrypted file not found for " + transactionId);
            else
            {
                byte[] data = Encoding.UTF8.GetBytes(encEdi);
                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                postParameters.Add("from", shipperDuns);
                postParameters.Add("to", tspOrPipeDuns);
                postParameters.Add("version", "3.0");
                postParameters.Add("receipt-disposition-to", shipperDuns);
                postParameters.Add("receipt-report-type", "gisb-acknowledgement-receipt");
                postParameters.Add("receipt-security-selection", "signed-receipt-protocol=required,pgp-signature;signed-receipt-micalg=required,md5");
                postParameters.Add("input-format", "X12");
                postParameters.Add("input-data", new FormUpload.FileParameter(data, transactionId + ".asc", "multipart/encrypted"));
                return postParameters;
            }
        }
        #endregion
        #region  Logging
        private void SaveXmlToDB(string transactionId, int pipelineId, string Xmltemplate, string EdiFile, string EncEDIFile,string sendingUrl)
        {
            TaskMgrXml xml = _serviceTaskMgrXml.GetbyTransactionId(transactionId);//GetAll().Where(a => a.TransactionId == transactionId).FirstOrDefault();
            if (xml != null)
            {
                if (Xmltemplate != null)
                    xml.XmlData = Xmltemplate;
                if (EdiFile != null)
                    xml.EDIData = EdiFile;
                if (EncEDIFile != null)
                    xml.EncEDIData = EncEDIFile;
                if (sendingUrl != null)
                    xml.SendUrl = sendingUrl;
                xml.IsProccessed = false;
                _serviceTaskMgrXml.Update(xml);
            }
            else
            {
                xml = new TaskMgrXml();
                xml.AddedOn = DateTime.UtcNow;
                xml.TransactionId = transactionId;
                xml.PipelineId = pipelineId;
                if (Xmltemplate != null)
                    xml.XmlData = Xmltemplate;
                if (EdiFile != null)
                    xml.EDIData = EdiFile;
                if (EncEDIFile != null)
                    xml.EncEDIData = EncEDIFile;
                if (sendingUrl != null)
                    xml.SendUrl = sendingUrl;
                xml.IsProccessed = false;
                _serviceTaskMgrXml.Add(xml);
            }
            _serviceTaskMgrXml.Save();
        }
        private void UpdateJobStatus(string TransactionID, int status, int stageId)
        {
            try
            {
                TaskMgrJob pendingJob = _serviceTaskMgrJob.GetByTransactionId(TransactionID);//GetAll().Where(a => a.TransactionId == TransactionID).FirstOrDefault();//BLLTaskMgrJobs.GetItem(TransactionID);
                if (pendingJob != null)
                {
                    pendingJob.StartDate = status == (int)Statuses.InProgress ? DateTime.UtcNow : pendingJob.StartDate;
                    pendingJob.EndDate = status != (int)Statuses.InProgress ? DateTime.UtcNow : pendingJob.EndDate;
                    pendingJob.StatusId = status;
                    pendingJob.StageId = stageId;
                    _serviceTaskMgrJob.Update(pendingJob);
                    _serviceTaskMgrJob.Save();
                    if((pendingJob.StageId == (int)SendingStages.SendingFile && pendingJob.StatusId == (int)Statuses.Completed)|| (pendingJob.StatusId == (int)Statuses.Error))
                    {
                        var outbox = _serviceOutbox.GetByTransactionId(Guid.Parse(pendingJob.TransactionId));
                        if (outbox != null)
                        {
                            if ((pendingJob.StageId == (int)SendingStages.SendingFile) && (pendingJob.StatusId == (int)Statuses.Completed))
                                outbox.StatusID = 5;
                            if (pendingJob.StatusId == (int)Statuses.Error)
                                outbox.StatusID = 8;
                            _serviceOutbox.Update(outbox);
                            _serviceOutbox.Save();
                        }
                        V4_Batch batch = _serviceBatch.GetByTransactionID(Guid.Parse(pendingJob.TransactionId));
                        if (batch != null)
                        {
                            batch.StatusID = outbox.StatusID;
                            _serviceBatch.Update(batch);
                            _serviceBatch.SaveChages();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogManager(new StringBuilder("UpdateJobStatus Task Manager Task Manager Send"), "Error", new StringBuilder(ex.Message));
            }
        }
        private void UpdateTransactionLogs(string transId, int status)
        {
            try
            {
                TransactionLog log = _serviceTransactionLog.GetByTransactionId(transId);//.GetAll().Where(a => a.TransactionId == transId).FirstOrDefault();
                if (log != null)
                {
                    log.StatusId = status;
                    log.EndDate = status != (int)Statuses.InProgress ? DateTime.Now : log.EndDate;
                    _serviceTransactionLog.Update(log);
                }
                else
                {
                    log = new TransactionLog();
                    log.CreatedAt = DateTime.Now;
                    log.EndDate = DateTime.MaxValue;
                    log.StartDate = DateTime.Now;
                    log.StatusId = (int)Statuses.InProgress;
                    log.TransactionId = transId;
                    _serviceTransactionLog.Add(log);
                }
                _serviceTransactionLog.Save();
            }
            catch (Exception ex) { AppLogManager(new StringBuilder("UpdateTransactionLogs Task Manager Send"), "Error", new StringBuilder(ex.Message)); }
        }
        private void UpdateWorkflow(Int64 workflowId, int status)
        {
            try
            {
                JobWorkflow workFlow = _serviceJobWorkFlow.GetByWorkflowId(workflowId);//.GetAll().Where(a => a.WorkflowId == workflowId).FirstOrDefault();//BLLJobWorkflow.GetItem(workflowId);
                if (workFlow != null)
                {
                    workFlow.EndDate = DateTime.Now;
                    workFlow.StatusId = status;
                    _serviceJobWorkFlow.Update(workFlow);//BLLJobWorkflow.Update(workFlow);
                }
                _serviceJobWorkFlow.Save();
            }
            catch (Exception ex) { AppLogManager(new StringBuilder("UpdateWorkflow Task Manager Send"), "Error", new StringBuilder(ex.Message)); }
        }
        private void SaveErrorLogs(string message, Guid transId, Int64 workflowId, int stageId)
        {
            JobStackErrorLog error = new JobStackErrorLog();
            error.ErrorMessage = message;
            error.ErrorType = "NA";
            error.StageId = stageId;
            error.TranactionId = transId;
            error.WorkflowId = workflowId;
            _serviceJobStackErrorLog.Add(error);//BLLJobStackErrorLogs.Save(error);
            _serviceJobStackErrorLog.Save();
        }
        private void AppLogManager(StringBuilder source, string type, StringBuilder errMsg)
        {
            ApplicationLog log = new ApplicationLog();
            log.Source = source.ToString();
            log.Type = type;
            log.Description = errMsg.ToString();
            log.CreatedDate = DateTime.Now;
            _applicationLogs.Add(log);
            _applicationLogs.Save();
        }
        private void AddUpdateTransactionLog(string transactionId, int statusId)
        {
            TransactionLog log = _serviceTransactionLog.GetByTransactionId(transactionId);//.GetAll().Where(a => a.TransactionId == transactionId).FirstOrDefault();
            if (log != null)
            {
                log.StatusId = statusId;
                log.StartDate = DateTime.Now;
                log.EndDate = DateTime.MaxValue;
                _serviceTransactionLog.Update(log);
            }
            else
            {
                log = new TransactionLog();
                log.CreatedAt = DateTime.Now;
                log.EndDate = DateTime.MaxValue;
                log.StartDate = DateTime.Now;
                log.StatusId = statusId;
                log.TransactionId = transactionId;
                _serviceTransactionLog.Add(log);
            }
            _serviceTransactionLog.Save();
        }
        private void AddSqtsTrackOnNom(string transactionID, string NomTrackingId)
        {
            SQTSTrackOnNom sqtsTrackObj = _serviceSqtsTrackOnNom.GetSqtsOnNomTrackId(Guid.Parse(transactionID), NomTrackingId);
            if (sqtsTrackObj == null)
            {
                sqtsTrackObj = new SQTSTrackOnNom();
                sqtsTrackObj.NomTrackingId = NomTrackingId;
                sqtsTrackObj.NomTransactionID = Guid.Parse(transactionID);
                sqtsTrackObj.ReceiptPointQuantity = "";
                sqtsTrackObj.DeliveryPointQuantity = "";
                sqtsTrackObj.ReductionReason = "";
                _serviceSqtsTrackOnNom.Add(sqtsTrackObj);
            }
            _serviceSqtsTrackOnNom.Save();
        }
        #endregion
    }
}
