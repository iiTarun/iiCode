using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using UPRD.Model.Enums;
using WatchlistMailManagement.Repositories;
using WatchlistMailManagement.Services;

namespace WatchlistMailManagement.JobSchedular
{

    public class WatchListAvailAlertOACYSendMail : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            ApplicationLogRepository applogs = new ApplicationLogRepository();
            WatchlistService WatchlistService = new WatchlistService();
            try
            {
                var resultoacy = WatchlistService.GetOacyFromMappingNSendMail();
                return null;
            }
            catch (Exception ex)
            {
                applogs.AppLogManager("WatchListAvailAlertOACYSendMail", "WatchListAlertJob", "Error: " + ex.ToString());
                return null;
            }
        }
    }

    public class WatchListAvailAlertOACY : IJob
    {       
        public Task Execute(IJobExecutionContext context)
        {
            ApplicationLogRepository applogs = new ApplicationLogRepository();
            WatchlistService WatchlistService =new WatchlistService();
            try
            {
                var resultoacy = WatchlistService.ExecuteWatchListMailAlertOACY(WatchlistAlertFrequency.WhenAvailable);
                return null;
            }
            catch (Exception ex)
            {
                applogs.AppLogManager("WatchListAvailAlertOACY", "WatchListAlertJob", "Error: " + ex.ToString());
                return null;
            }
        }
    }





    public class WatchListAvailAlertUNSC : IJob
    {        
        public Task Execute(IJobExecutionContext context)
        {
            ApplicationLogRepository applogs = new ApplicationLogRepository();
            WatchlistService WatchlistService = new WatchlistService();
            try
            {
                var resultunsc = WatchlistService.ExecuteWatchListMailAlertUNSC(WatchlistAlertFrequency.WhenAvailable);
                return null;
            }
            catch (Exception ex)
            {
                applogs.AppLogManager("WatchListAvailAlertUNSC", "WatchListAlertJob", "Error: " + ex.ToString());
                return null;
            }
        }
    }


    public class WatchListAvailAlertUNSCSendMail : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            ApplicationLogRepository applogs = new ApplicationLogRepository();
            WatchlistService WatchlistService = new WatchlistService();
            try
            {
                var resultunsc = WatchlistService.GetUnscFromMappingNSendMail();
                return null;
            }
            catch (Exception ex)
            {
                applogs.AppLogManager("WatchListAvailAlertUNSCSendMail", "WatchListAlertJob", "Error: " + ex.ToString());
                return null;
            }
        }
    }

    

    public class WatchListAvailAlertSWNT : IJob
    {       
        public Task Execute(IJobExecutionContext context)
        {
            ApplicationLogRepository applogs = new ApplicationLogRepository();
            WatchlistService WatchlistService = new WatchlistService();
            try
            {
                var resultswnt = WatchlistService.ExecuteWatchListMailAlertSWNT(WatchlistAlertFrequency.WhenAvailable);
                 return null;
            }
            catch (Exception ex)
            {
                applogs.AppLogManager("WatchListAvailAlertSWNT", "WatchListAlertJob", "Error: " + ex.ToString());
                return null;
            }
        }
    }

    public class WatchListAvailAlertSWNTSendMail : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            ApplicationLogRepository applogs = new ApplicationLogRepository();
            WatchlistService WatchlistService = new WatchlistService();
            try
            {
                var resultswnt = WatchlistService.GetSwntFromMappingNSendMail();
                return null;
            }
            catch (Exception ex)
            {
                applogs.AppLogManager("WatchListAvailAlertSWNTSendMail", "WatchListAlertJob", "Error: " + ex.ToString());
                return null;
            }
        }
    }



    //public class WatchListMailAlertDailyJob : IJob
    //{       
    //    public Task Execute(IJobExecutionContext context)
    //    {
    //        ApplicationLogRepository applogs = new ApplicationLogRepository();
    //        WatchlistService WatchlistService = new WatchlistService();
    //        try {
    //        var resultoacy = WatchlistService.ExecuteWatchListMailAlertOACY(WatchlistAlertFrequency.Daily);                  
    //        var resultunsc = WatchlistService.ExecuteWatchList(WatchlistAlertFrequency.Daily, UprdDataSet.UNSC);
    //        var resultswnt = WatchlistService.ExecuteWatchList(WatchlistAlertFrequency.Daily, UprdDataSet.SWNT);
    //        // applogs.AppLogManager("WatchListMailAlertDailyJob", "WatchListAlertJob", "Watch List Alert Mail- Schedular Working.");
    //         return null;
    //        }
    //        catch (Exception ex) {
    //            applogs.AppLogManager("WatchListMailAlertDailyJob", "WatchListAlertJob","Error: " + ex.ToString());
    //            return null;
    //        }
    //    }
    //}  
}