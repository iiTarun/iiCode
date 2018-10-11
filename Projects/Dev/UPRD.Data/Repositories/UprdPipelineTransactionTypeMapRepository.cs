using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;

using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPRD.DTO;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdPipelineTransactionTypeMapRepository : RepositoryBase<Pipeline_TransactionType_Map>, IUprdPipelineTransactionTypeMapRepository
    {
        public UprdPipelineTransactionTypeMapRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }


        public bool DeleteTransactionByID(int id)
        {
            var objCounter = DbContext.Pipeline_TransactionType_Map.Where(a => a.ID == id).FirstOrDefault();

            objCounter.IsActive = false;
            DbContext.Entry(objCounter).State = EntityState.Modified;
            DbContext.SaveChanges();

            return true;
        }

        public Pipeline_TransactionType_Map GetTransactionByid(int id)
        {
            Pipeline_TransactionType_Map data = new Pipeline_TransactionType_Map();
            data = DbContext.Pipeline_TransactionType_Map.Where(a => a.ID == id).SingleOrDefault();
            //var data = (from pt in DbContext.Pipeline_TransactionType_Map
            //            join tt in DbContext.metadataTransactionType on pt.TransactionTypeID equals tt.ID
            //            where pt.ID == id 
            //            select new
            //            {
            //                id = pt.ID,
            //                name = tt.Name,
            //                identifier = tt.Identifier,
            //                sequence = tt.SequenceNo,
            //                isActive = tt.IsActive,
            //                pathType = pt.PathType,
            //                PipelineDun = pt.PipeDuns
            //            }).FirstOrDefault();

            //if (data != null)
            //{

            //    itemObj.ID = data.id;
            //    itemObj.Identifier = data.identifier;
            //    itemObj.Name = data.name;
            //    itemObj.SequenceNo = data.sequence;
            //    itemObj.IsActive = data.isActive;
            //    itemObj.PathType = data.pathType;
            //    itemObj.pipelineDuns = data.PipelineDun;
            //}
            return data;
            //return this.DbContext.metadataTransactionType.Where(a => a.ID == id).FirstOrDefault();
        }

        public List<Pipeline_TransactionType_MapDTO> GetTransactions(string pipelineDuns)
        {
            //var data = DbContext.Pipeline_TransactionType_Map.Where(a => a.PipeDuns == pipelineDuns).ToList();
            List<Pipeline_TransactionType_MapDTO> resultantData = new List<Pipeline_TransactionType_MapDTO>();
            var data = (from pt in DbContext.Pipeline_TransactionType_Map
                        join tt in DbContext.metadataTransactionType on pt.TransactionTypeID equals tt.ID
                        where pt.PipeDuns == pipelineDuns
                        select new
                        {
                            id = pt.ID,
                            name = tt.Name,
                            identifier = tt.Identifier,
               
                            isActive = pt.IsActive,
                            pathType = pt.PathType,
                            PipeLineDuns = pt.PipeDuns
                        }).ToList();

            foreach (var tt in data)
            {
                Pipeline_TransactionType_MapDTO itemObj = new Pipeline_TransactionType_MapDTO();
                itemObj.ID = tt.id;

                itemObj.Name = tt.name;

                itemObj.IsActive = tt.isActive;
                itemObj.PathType = tt.pathType;
                itemObj.pipelineDuns = tt.PipeLineDuns;
                resultantData.Add(itemObj);
            }

            return resultantData;
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }

        public string UpdateTransactionByID(Pipeline_TransactionType_Map Transaction)
        {


            Pipeline_TransactionType_Map Transactions = new Pipeline_TransactionType_Map();

            if (Transaction.ID == 0)
            {

                Transactions.PipeDuns = Transaction.PipeDuns;
                Transactions.TransactionTypeID = Transaction.TransactionTypeID;
                Transactions.CreatedBy = Transaction.CreatedBy;
                Transactions.CreatedDate = DateTime.Now;
                Transactions.PathType = Transaction.PathType;
                Transactions.LastModifiedBy = Transaction.LastModifiedBy;
                Transactions.LastModifiedDate = DateTime.Now;
                Transactions.IsActive = true;
                DbContext.Pipeline_TransactionType_Map.Add(Transactions);
                DbContext.SaveChanges();



                return ("Added Successfully");
            }
            else
            {
                //update location in location table 
                var objCon = DbContext.Pipeline_TransactionType_Map.Where(a => a.ID == Transaction.ID).FirstOrDefault();
                objCon.PipeDuns = Transaction.PipeDuns;
                objCon.TransactionTypeID = Transaction.TransactionTypeID;
                objCon.PathType = Transaction.PathType;
                objCon.LastModifiedBy = Transaction.LastModifiedBy;
                objCon.LastModifiedDate = DateTime.Now;
                objCon.IsActive = true;
                DbContext.Entry(objCon).State = EntityState.Modified;
                DbContext.SaveChanges();




                return ("Updated Successfully");
            }
        }
        public bool ActivateTrasaction(int id)
        {
            var objCounter = DbContext.Pipeline_TransactionType_Map.Where(a => a.ID == id).FirstOrDefault();

            objCounter.IsActive = true;
            DbContext.Entry(objCounter).State = EntityState.Modified;
            DbContext.SaveChanges();

            return true;
        }

        public bool ClientEnvironmentsetting()
        {
            var clientList = DbContext.ClientEnvironmentSetting.Where(a => a.Enginestatus == true).ToList();

            foreach (var item in clientList)
            {
                string sourceCon = ConfigurationManager.ConnectionStrings["UPRDEntities"].ConnectionString;
                using (SqlConnection Sourcecon = new SqlConnection(sourceCon))
                {
                    SqlCommand com = new SqlCommand("select * from Pipeline_TransactionType_Map", Sourcecon);
                    Sourcecon.Open();
                    using (SqlDataReader rdr = com.ExecuteReader())
                    {
                        using (SqlConnection Descon = new SqlConnection(item.ConnectionString))
                        {
                            SqlCommand com1 = new SqlCommand("Truncate table Pipeline_TransactionType_Map", Descon);
                            Descon.Open();
                            com1.ExecuteNonQuery();
                            using (SqlBulkCopy bc = new SqlBulkCopy(Descon))
                            {
                                bc.DestinationTableName = "Pipeline_TransactionType_Map";
                                bc.WriteToServer(rdr);
                            }

                        }

                    }


                }
            }

            //List<Pipeline_TransactionType_Map>  SourceList = DbContext.Pipeline_TransactionType_Map.ToList();
            //var clientList = DbContext.ClientEnvironmentSetting.Where(a=>a.Enginestatus==true).ToList();
            // foreach (var item in clientList)
            // {
            //         using (var DestContext = new UPRDEntities(item.ConnectionString))
            //     {
            //         DestContext.Database.ExecuteSqlCommand("Truncate table Pipeline_TransactionType_Map");
            //         foreach (var metadataItems in SourceList)
            //         {
            //             DestContext.Pipeline_TransactionType_Map.Add(metadataItems);
            //         }
            //         DestContext.SaveChanges();
            //     }


            // }


            return true;
        }
    }
    public interface IUprdPipelineTransactionTypeMapRepository : IRepository<Pipeline_TransactionType_Map>
    {
        Pipeline_TransactionType_Map GetTransactionByid(int id);
        List<Pipeline_TransactionType_MapDTO> GetTransactions(string pipelineDuns);
        //IEnumerable<metadataTransactionType> GetTransactions();
        bool ActivateTrasaction(int id);
        void Save();
        bool DeleteTransactionByID(int id);
        string UpdateTransactionByID(Pipeline_TransactionType_Map Transaction);
        bool ClientEnvironmentsetting();
    }
}

