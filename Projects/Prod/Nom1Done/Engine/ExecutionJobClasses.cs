using Newtonsoft.Json;
using Nom.ViewModel;
using Nom1Done.Data;
using Nom1Done.Data.Repositories;
using Nom1Done.EDIEngineSendAndReceive;
using Nom1Done.Enums;
using Nom1Done.Service.Interface;
using Quartz;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Serialization;

namespace Nom1Done.Schedular
{
    #region Match noms
    public class MatchNomination : IJob
    {
        IBatchRepository batchRepo;
        public MatchNomination(IBatchRepository batchRepo)
        {
            this.batchRepo = batchRepo;
        }
        public void Execute(IJobExecutionContext context)
        {
            batchRepo.matchPathedNomination();
            //batchRepo.matchPNTNomination();
            //batchRepo.matchNonPathed();
        }
    }
    #endregion
    #region sqts scenerio 2nd
    public class SqtsScenerio2nd : IJob
    {
        ISQTSPerTransactionRepository sqtsRepo;
        public SqtsScenerio2nd(ISQTSPerTransactionRepository sqtsRepo)
        {
            this.sqtsRepo = sqtsRepo;
        }
        public void Execute(IJobExecutionContext context)
        {
            sqtsRepo.Sqts2ndScenerio();
        }
    }
    #endregion
    #region Engine Jobs
    #region Shooting form here(send files processing job)
    public class JobManagerNomination : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                SendPartFunctions obj = new SendPartFunctions();
                obj.PendingNominationJobs();
            }
            catch(Exception ex)
            {

            }
        }
    }
    #endregion
    #endregion
    #region Job for xml generation for
    public class GenerateXmlForLocationAndCounterParty : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            GenerateXmlForCounterPartyAndLoaction();
        }
        private void GenerateXmlForCounterPartyAndLoaction()
        {
            try
            {
                string apiBaseUrl = ConfigurationManager.AppSettings.Get("BaseUrlOfUprdApi");
                RestClient clientForLoc = new RestClient(apiBaseUrl + "/api/Location");
                var requestForLoc = new RestRequest(string.Format("GetLocationList"), Method.GET);
                var responseForLoc = clientForLoc.Execute(requestForLoc);
                if (responseForLoc.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = responseForLoc.Content;
                    var path = Path.Combine(HostingEnvironment.MapPath("~/App_Data"), "Location.xml");
                    var locationList = JsonConvert.DeserializeObject<List<LocationsDTO>>(data);
                    XmlSerializer serialiser = new XmlSerializer(typeof(List<LocationsDTO>));
                    TextWriter Filestream = new StreamWriter(path);
                    locationList = locationList.Where(a => !string.IsNullOrEmpty(a.Identifier)).ToList();
                    serialiser.Serialize(Filestream, locationList);
                    Filestream.Close();
                }
                RestClient clientForCp = new RestClient(apiBaseUrl + "/api/CounterParty");
                var requestForCp = new RestRequest(string.Format("GetCounterPartyList"), Method.GET);
                var responseForCp = clientForCp.Execute(requestForCp);
                if (responseForCp.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = responseForCp.Content;
                    var path = Path.Combine(HostingEnvironment.MapPath("~/App_Data"), "CounterParty.xml");
                    var cpList = JsonConvert.DeserializeObject<List<CounterPartiesDTO>>(data);
                    XmlSerializer serialiser = new XmlSerializer(typeof(List<CounterPartiesDTO>));
                    TextWriter Filestream = new StreamWriter(path);
                    cpList = cpList.Where(a => !string.IsNullOrEmpty(a.Identifier)).ToList();
                    serialiser.Serialize(Filestream, cpList);
                    Filestream.Close();
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
    #endregion
    
}