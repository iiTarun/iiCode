using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPRD.Model;


using UPRD.Infrastructure;
using System.Data.Entity;
using UPRD.DTO;
using System.Configuration;
using System.Data.SqlClient;

namespace UPRD.Data.Repositories
{
    public class UprdCounterPartyRepository : RepositoryBase<CounterParty>, IUPRDCounterPartyRepository

    {


        public UprdCounterPartyRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }





        public CounterParty GetCounterPartyByid(int id)
        {
            //return this.DbContext.CounterParty.Where(x => x.ID == id).FirstOrDefault<CounterPartiesDTO>();

            return this.DbContext.CounterParty.Where(a => a.ID == id).FirstOrDefault();
        }

        public string UpdateCounterPartyByID(CounterParty counter)
        {


            CounterParty Counters = new CounterParty();
            if (counter.ID == 0)
            {
                Counters.CreatedDate = DateTime.Now;
                Counters.Name = counter.Name;
                Counters.Identifier = counter.Identifier;
                Counters.ModifiedBy = counter.ModifiedBy;
                Counters.ModifiedDate = DateTime.Now;
                Counters.PropCode = counter.PropCode;
                Counters.IsActive = true;
                Counters.CreatedBy = counter.CreatedBy;
                Counters.PipelineID = counter.PipelineID;


                DbContext.CounterParty.Add(Counters);
                DbContext.SaveChanges();
                return ("Added Successfully");
            }
            else
            {
                //update location in location table 
                var objCon = DbContext.CounterParty.Where(a => a.ID == counter.ID).FirstOrDefault();
                objCon.Name = counter.Name;
                objCon.Identifier = counter.Identifier;
                objCon.PropCode = counter.PropCode;
                objCon.ModifiedBy = counter.ModifiedBy;
                objCon.ModifiedDate = DateTime.Now;
                DbContext.Entry(objCon).State = EntityState.Modified;
                DbContext.SaveChanges();
                return ("Updated Successfully");
            }

        }
        public bool DeleteCounterPartiesByID(int ID)
        {

            var objCounter = DbContext.CounterParty.Where(a => a.ID == ID).FirstOrDefault();

            objCounter.IsActive = false;
            objCounter.ModifiedDate = DateTime.Now;
            DbContext.Entry(objCounter).State = EntityState.Modified;
            DbContext.SaveChanges();

            return true;
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }

        public List<CounterParty> GetCounterParties()

        {


            var data = DbContext.CounterParty.ToList();
            //var data = DbContext.CounterParty.Where(a => a.IsActive == true).ToList();
            return data;


        }
        
        public bool ClientEnvironmentsetting()
        {

            //List<CounterParty> SourceList = DbContext.CounterParty.ToList();
            var clientList = DbContext.ClientEnvironmentSetting.Where(a => a.Enginestatus == true).ToList();

            foreach (var item in clientList)
            {
                string sourceCon = ConfigurationManager.ConnectionStrings["UPRDEntities"].ConnectionString;
                using (SqlConnection Sourcecon = new SqlConnection(sourceCon))
                {
                    SqlCommand com = new SqlCommand("select * from CounterParties", Sourcecon);
                    Sourcecon.Open();
                    using (SqlDataReader rdr = com.ExecuteReader())
                    {
                        using (SqlConnection Descon = new SqlConnection(item.ConnectionString))
                        {
                            SqlCommand com1 = new SqlCommand("Truncate table CounterParties", Descon);
                            Descon.Open();
                            com1.ExecuteNonQuery();
                            using (SqlBulkCopy bc = new SqlBulkCopy(Descon))
                            {
                                bc.DestinationTableName = "CounterParties";
                                bc.WriteToServer(rdr);
                            }

                        }

                    }


                }
            }

            //foreach (var item in clientList)
            //{
            //    using (var DestContext = new UPRDEntities(item.ConnectionString))
            //    {
            //        DestContext.Database.ExecuteSqlCommand("Truncate table CounterParties");
            //        foreach (var metadataItems in SourceList)
            //        {
            //            DestContext.CounterParty.Add(metadataItems);
            //        }
            //        DestContext.SaveChanges();
            //    }


            //}


            return true;
        }

        public bool ActivateCounterParty(int ID)
        {
            var objCounter = DbContext.CounterParty.Where(a => a.ID == ID).FirstOrDefault();

            objCounter.IsActive = true;
            objCounter.ModifiedDate = DateTime.Now;
            DbContext.Entry(objCounter).State = EntityState.Modified;
            DbContext.SaveChanges();

            return true;
        }
    }

    public interface IUPRDCounterPartyRepository : IRepository<CounterParty>
    {
        CounterParty GetCounterPartyByid(int id);
        List<CounterParty> GetCounterParties();

        void Save();
        bool DeleteCounterPartiesByID(int id);
        string UpdateCounterPartyByID(CounterParty Counter);
        bool ClientEnvironmentsetting();
        bool ActivateCounterParty(int id);
    }

}
