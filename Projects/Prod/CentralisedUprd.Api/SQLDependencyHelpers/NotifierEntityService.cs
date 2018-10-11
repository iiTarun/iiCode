using CentralisedUprd.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CentralisedUprd.Api.SQLDependencyHelpers
{
    public static class NotifierEntityService 
    {
        // private UprdDbEntities1 db = new UprdDbEntities1("UprdDbEntities1");
       static UprdDbEntities1 db = new UprdDbEntities1();

       // public NotifierEntityService() { }  

        //public string GetNotifierEntityOfUprdStatus()
        //{
        //    var result = string.Empty;
        //    var collection = db.UPRDStatus;
        //    result = db.GetNotifierEntity<UPRDStatu>(collection).ToJson();
        //    return result;
        //}

        //public static string GetNotifierEntityOfOACY()
        //{   
            //var result = string.Empty;
            //var collection = db.OACYPerTransactions;
            //result = db.GetNotifierEntity<OACYPerTransaction>(collection).ToJson();
            //return result;
        //}

        //public static string GetNotifierEntityOfSWNT()
        //{
        //    var result = string.Empty;
        //    var collection = db.SwntPerTransactions;
        //    result = db.GetNotifierEntity<SwntPerTransaction>(collection).ToJson();
        //    return result;
        //}

        //public static string GetNotifierEntityOfUNSC()
        //{
        //    var result = string.Empty;
        //    var collection = db.UnscPerTransactions;
        //    result = db.GetNotifierEntity<UnscPerTransaction>(collection).ToJson();
        //    return result;
        //}
    }
}