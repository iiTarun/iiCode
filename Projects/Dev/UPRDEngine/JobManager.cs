using Quartz;
using System;
using UPRDEngine.EDIEngineSendAndReceive;

namespace UPRDEngine.JobManager
{
    #region Shooting form here(send files processing job)
    public class JobManagerOacyJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Oacy Job hit (check every 10 Sec). {0}", DateTime.Now);
            SendPartFunctions obj = new SendPartFunctions();
            obj.PlaceUprdJobAndProcess(true, false, false);
        }
    }
    public class JobManagerUnscJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Unsc Job hit (check every 10 Sec). {0}", DateTime.Now);
            SendPartFunctions obj = new SendPartFunctions();
            obj.PlaceUprdJobAndProcess(false, true, false);
        }
    }
    public class JobManagerSwntJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("SWNT Job hit (check every 10 Sec). {0}", DateTime.Now);
            SendPartFunctions obj = new SendPartFunctions();
            obj.PlaceUprdJobAndProcess(false, false, true);
        }
    }
    #endregion
    #region (Receive files processing job associated class)
    public class JobManagerReceiveUprdProcessing : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Console.WriteLine("Receive file Job hit (check every 5 Sec). {0}", DateTime.Now);
                ReceivePartModified engObj = new ReceivePartModified();
                engObj.ProcessFiles(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:- " + ex.Message);
            }

        }
    }
    public class JobManagerReceiveUprdProcessingAgain : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Console.WriteLine("Receive file Job hit (check every 5 Sec). {0}", DateTime.Now);
                ReceivePartModified engObj = new ReceivePartModified();
                engObj.ProcessFiles(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:- " + ex.Message);
            }

        }
    }

    #endregion
}
