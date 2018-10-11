using System;
using UPRD.Data;
using UPRD.Model;
using Newtonsoft.Json;
using UPRD.Services.Interface;
using UPRD.Data.Extensions;
using UPRD.Data.SQLServerNotifier;

namespace UPRD.Services.Services
{
    public class NotifierEntityService: INotifierEntityService
    {
        private UPRDEntities db = new UPRDEntities();

        public NotifierEntityService(){ }

        public string GetNotifierEntityForNomStatus()
        {
            var result = string.Empty;
            var collection = db.DashNominationStatus;
            result = db.GetNotifierEntityForNomStatus(collection).ToJson(); 
            return result;
        }

    }
}
