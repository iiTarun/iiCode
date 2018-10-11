using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdDashNominationStatus : RepositoryBase<DashNominationStatus>, IUprdDashNominationStatus
    {
        public UprdDashNominationStatus(DbFactory dbFactory): base(dbFactory)
        {
           

        }
        public void Save()
        {
            this.DbContext.SaveChanges();
        }

        public IQueryable<DashNominationStatus> GetDashNomStatus(string shipperDuns)
        {
            IQueryable<DashNominationStatus> data;
            //InProcess = 2,Sbmitted = 5,Draft = 11,Rejected = 10, Accepted = 7,Error = 8,GISBError = 0,Replaced = 12
            if (!string.IsNullOrEmpty(shipperDuns))
            {
                 data = DbContext.DashNominationStatus.Where(x => x.ShipperDUNS == shipperDuns && (x.StatusId == 2 || x.StatusId == 5 || x.StatusId == 0)).OrderByDescending(x => x.SubmittedDate);
            }
            else
            {
                 data = DbContext.DashNominationStatus.Where(x => x.StatusId == 2 || x.StatusId == 5 || x.StatusId == 0).OrderByDescending(x => x.SubmittedDate); 
            }

            return data;
        }  

        public bool UpdatetblTrigger(String ShipperDuns, String pipeDuns, int StatusID)
        {
            var objDashboard = DbContext.DashNominationStatus.Where(a => a.ShipperDUNS == ShipperDuns && a.PipeDuns == pipeDuns && a.StatusId == StatusID).FirstOrDefault();
            try
            {
                objDashboard.AlertTrigger = true;
                DbContext.Entry(objDashboard).State = EntityState.Modified;
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
              
            }
            return false;
        }

        public string GetConStringByShipperDuns(String shipperDuns)
        {
            if (!string.IsNullOrEmpty(shipperDuns))
            {
                return DbContext.ClientEnvironmentSetting
                    .Where(a => a.ShipperDuns == shipperDuns)
                    .Select(a => a.ConnectionString).FirstOrDefault();
            }
            return null;
        }


        public string GetEngineStatus(String ShipperDuns)
        {
            string conString = GetConStringByShipperDuns(ShipperDuns);
                using (UPRDEntities db = new UPRDEntities(conString.ToString()))
                {
                   var ResultSetting = db.Setting.Where(s => s.Name.ToLower() == "iiteston").FirstOrDefault();
                   return ResultSetting.Value;
                }
        }



        public bool UpdateEngineStatus(String ShipperDuns, bool EngineStatus)
        {
            string conString = GetConStringByShipperDuns(ShipperDuns);
            try
            {
                using (UPRDEntities db = new UPRDEntities(conString))
                {
                    var settingResult = db.Setting.Where(s => s.Name.ToLower() == "iiteston").FirstOrDefault();
                    settingResult.Value = EngineStatus.ToString();
                    db.Entry(settingResult).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            { }
            return false;         
        }



    }



    public interface IUprdDashNominationStatus : IRepository<DashNominationStatus>
    {    
        IQueryable<DashNominationStatus> GetDashNomStatus(string shipperDuns);
        bool UpdatetblTrigger(String ShipperDuns, String pipeDuns, int StatusID);
        string GetConStringByShipperDuns(String shipperDuns);
        bool UpdateEngineStatus(String ShipperDuns, bool EngineStatus);
        string GetEngineStatus(String ShipperDuns);
    }
}
