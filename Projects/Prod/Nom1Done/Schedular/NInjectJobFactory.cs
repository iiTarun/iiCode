using Ninject.Syntax;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Ninject;
using System.Globalization;

namespace Nom1Done.Schedular
{
    public class NInjectJobFactory : IJobFactory
    {
        private readonly IResolutionRoot resolutionRoot;

        public NInjectJobFactory(IResolutionRoot resolutionRoot)
        {
            this.resolutionRoot = resolutionRoot;
        }       
        

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            IJobDetail jobDetail = bundle.JobDetail;
            Type jobType = jobDetail.JobType;
            try
            {  
                return this.resolutionRoot.Get(jobType) as IJob;
            }
            catch (Exception e)
            {
                SchedulerException se = new SchedulerException(string.Format(CultureInfo.InvariantCulture, "Problem instantiating class '{0}'", jobDetail.JobType.FullName), e);
                throw se;
            }
            //return (IJob)this.resolutionRoot.Get<IJob>();
        }

        public void ReturnJob(IJob job)
        {
            this.resolutionRoot.Release(job);
        }
    }
}