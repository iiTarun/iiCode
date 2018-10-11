using System;
using Nom1Done.Data;
using Nom1Done.Model;
using Nom1Done.Service.Interface;
using Newtonsoft.Json;

namespace Nom1Done.Service
{
    public class NotifierEntityService: INotifierEntityService
    {
        private NomEntities db = new NomEntities();

        public NotifierEntityService(){ }


        public string GetNotifierEntityForNoms(string userId)
        {
            var result = string.Empty;
            var collection = db.V4_Batch;
            result = db.GetNotifierEntityForNoms(collection, userId).ToJson();            
            return result;
        }


        public string GetNotifierEntityOfUprdStatus()
        {
            var result = string.Empty;
            var collection = db.UPRDStatus;
            result = db.GetNotifierEntity<UPRDStatu>(collection).ToJson();
            return result;
        }
    }
}
