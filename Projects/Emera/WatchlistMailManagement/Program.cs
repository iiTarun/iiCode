using System;
using UPRD.Model.Enums;
using WatchlistMailManagement.JobSchedular;
using WatchlistMailManagement.Services;

namespace WatchlistMailManagement
{
    class Program
    {
        static WatchlistService WatchlistService = new WatchlistService();
        static void Main(string[] args)
        {
           JobSchedularForAlerts.Start();           
         //  WatchlistService.ExecuteWatchListMailAlertOACY(WatchlistAlertFrequency.WhenAvailable);
         //  WatchlistService.GetOacyFromMappingNSendMail();
   
        // WatchlistService.ExecuteWatchListMailAlertUNSC(WatchlistAlertFrequency.WhenAvailable);
         // WatchlistService.GetUnscFromMappingNSendMail();

        //  WatchlistService.ExecuteWatchListMailAlertSWNT(WatchlistAlertFrequency.WhenAvailable);
        // WatchlistService.GetSwntFromMappingNSendMail();

            Console.Write("Running New WatchList Mail Alerts Approach...");
            Console.Read();

        }
    }
}
