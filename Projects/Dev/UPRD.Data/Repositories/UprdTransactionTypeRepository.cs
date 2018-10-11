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
    public class UprdTransactionTypeRepository : RepositoryBase<metadataTransactionType>, IUPRDTransactionTypeRepository
    {



        public UprdTransactionTypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public bool DeleteTransactionByID(int id)
        {
            var objCounter = DbContext.metadataTransactionType.Where(a => a.ID == id).FirstOrDefault();

            objCounter.IsActive = false;
            DbContext.Entry(objCounter).State = EntityState.Modified;
            DbContext.SaveChanges();

            return true;
        }

        public metadataTransactionType GetTransactionByid(int id)
        {
            metadataTransactionType data = new metadataTransactionType();
            data = DbContext.metadataTransactionType.Where(a => a.ID == id).SingleOrDefault();

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

        public IEnumerable<metadataTransactionType> GetTransactions()
        {
            var data = DbContext.metadataTransactionType.ToList();

            //List<MetaDataTransactionTypesDTO> resultantData = new List<MetaDataTransactionTypesDTO>();
            //var data = (from pt in DbContext.Pipeline_TransactionType_Map
            //            join tt in DbContext.metadataTransactionType on pt.TransactionTypeID equals tt.ID
            //            where pt.PipeDuns == pipelineDuns
            //                   select new
            //            {
            //                id = pt.ID,
            //                name = tt.Name,
            //                identifier = tt.Identifier,
            //                sequence = tt.SequenceNo,
            //                isActive = tt.IsActive,
            //                pathType = pt.PathType,
            //                PipeLineDuns=pt.PipeDuns
            //            }).ToList();

            //foreach (var tt in data)
            //{
            //    MetaDataTransactionTypesDTO itemObj = new MetaDataTransactionTypesDTO();
            //    itemObj.ID = tt.id;
            //    itemObj.Identifier = tt.identifier;
            //    itemObj.Name = tt.name;
            //    itemObj.SequenceNo = tt.sequence;
            //    itemObj.IsActive = tt.isActive;
            //    itemObj.PathType = tt.pathType;
            //    itemObj.pipelineDuns = tt.PipeLineDuns;
            //    resultantData.Add(itemObj);
            //}
            return data;
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }

        public string UpdateTransactionByID(metadataTransactionType Transaction)
        {


            metadataTransactionType Transactions = new metadataTransactionType();
            //Pipeline_TransactionType_Map PipeLine = new Pipeline_TransactionType_Map();
            if (Transaction.ID == 0)
            {

                Transactions.Identifier = Transaction.Identifier;
                Transactions.Name = Transaction.Name;
                Transactions.SequenceNo = Transaction.SequenceNo;
                Transactions.IsActive = true;




                DbContext.metadataTransactionType.Add(Transactions);
                DbContext.SaveChanges();
                //if(Transactions.ID !=0)
                //{
                //    PipeLine.TransactionTypeID = Transactions.ID;
                //    PipeLine.PathType = Transaction.PathType;
                //    PipeLine.IsActive =  true;
                //    PipeLine.CreatedDate = DateTime.Now;
                //    PipeLine.LastModifiedDate = DateTime.Now;
                //    PipeLine.IsActive = true;
                //    PipeLine.PipeDuns = Transaction.pipelineDuns;
                //    DbContext.Pipeline_TransactionType_Map.Add(PipeLine);
                //}



                return ("Added Successfully");
            }
            else
            {
                //update location in location table 
                var objCon = DbContext.metadataTransactionType.Where(a => a.ID == Transaction.ID).FirstOrDefault();
                objCon.Name = Transaction.Name;
                objCon.Identifier = Transaction.Identifier;
                objCon.SequenceNo = Transaction.SequenceNo;

                DbContext.SaveChanges();
                //var objPipe= DbContext.Pipeline_TransactionType_Map.Where(a => a.TransactionTypeID == Transaction.ID).FirstOrDefault();

                //objPipe.PathType = Transaction.PathType;

                //objPipe.IsActive = true;
                //objPipe.CreatedDate = DateTime.Now;
                //objPipe.LastModifiedDate = DateTime.Now;
                //DbContext.Entry(objPipe).State = EntityState.Modified;

                //DbContext.SaveChanges();

                return ("Updated Successfully");
            }
        }
        public bool ActivateTrasaction(int id)
        {
            var objCounter = DbContext.metadataTransactionType.Where(a => a.ID == id).FirstOrDefault();

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
                    SqlCommand com = new SqlCommand("select * from metadataTransactionTypes", Sourcecon);
                    Sourcecon.Open();
                    using (SqlDataReader rdr = com.ExecuteReader())
                    {
                        using (SqlConnection Descon = new SqlConnection(item.ConnectionString))
                        {
                            SqlCommand com1 = new SqlCommand("Truncate table metadataTransactionTypes", Descon);
                            Descon.Open();
                            com1.ExecuteNonQuery();
                            using (SqlBulkCopy bc = new SqlBulkCopy(Descon))
                            {
                                bc.DestinationTableName = "metadataTransactionTypes";
                                bc.WriteToServer(rdr);
                            }

                        }

                    }


                }
            }

            //List<Pipeline_TransactionType_Map> SourceList = DbContext.Pipeline_TransactionType_Map.ToList();
            //var clientList = DbContext.ClientEnvironmentSetting.Where(a => a.Enginestatus == true).ToList();
            //foreach (var item in clientList)
            //{
            //    using (var DestContext = new UPRDEntities(item.ConnectionString))
            //    {
            //        DestContext.Database.ExecuteSqlCommand("Truncate table Pipeline_TransactionType_Map");
            //        foreach (var metadataItems in SourceList)
            //        {
            //            DestContext.Pipeline_TransactionType_Map.Add(metadataItems);
            //        }
            //        DestContext.SaveChanges();
            //    }


            //}


            return true;
        }
    }


    public interface IUPRDTransactionTypeRepository : IRepository<metadataTransactionType>
    {
        metadataTransactionType GetTransactionByid(int id);
        //IEnumerable<MetaDataTransactionTypesDTO> GetTransactions(string pipelineDuns);
        IEnumerable<metadataTransactionType> GetTransactions();
        bool ActivateTrasaction(int id);
        void Save();
        bool DeleteTransactionByID(int id);
        string UpdateTransactionByID(metadataTransactionType Transaction);
        bool ClientEnvironmentsetting();


    }
}
