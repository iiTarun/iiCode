using Quartz;
using System;
using System.Threading.Tasks;

namespace CentralisedUprd.Api.JobSchedular
{
    public class UprdStatusResultDailyAlert : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            UprdStatusResultAlert obj = new UprdStatusResultAlert();
            DateTime date = DateTime.Now.Date;
            obj.GenerateUprdStatusFireAlert(date);
            return null;
        }

    }
}