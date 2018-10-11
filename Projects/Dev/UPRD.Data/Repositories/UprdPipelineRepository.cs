using System.Collections.Generic;
using System.Linq;
using System;
using UPRD.Infrastructure;
using UPRD.Model;
using UPRD.Enums;
using UPRD.DTO;

namespace UPRD.Data.Repositories
{
    public class UprdPipelineRepository : RepositoryBase<Pipeline>, IUprdPipelineRepository
    {
        public UprdPipelineRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        //public IEnumerable<PipelineDTO> GetAllActivePipeline()
        //{
        //    return DbContext.Pipeline.Where(a => a.IsActive == true);
        //}
        public IEnumerable<PipelineDTO> GetAllActivePipeline()
        {
            List<PipelineDTO> Query = (from b in DbContext.Pipeline
                                       //join b in DbContext.Pipeline on a.PipeDuns equals b.DUNSNo

                                       select new PipelineDTO
                                       {
                                           ID = b.ID,
                                           DUNSNo = b.DUNSNo,
                                           Name = b.Name + " (" + b.DUNSNo + ")",
                                           ModelTypeID = b.ModelTypeID,
                                           IsUprdActive = b.IsUprdActive,
                                           IsActive = b.IsActive,
                                           TSPId = b.TSPId,
                                           CreatedBy = b.CreatedBy,
                                           CreatedDate = b.CreatedDate,
                                           ModifiedBy = b.ModifiedBy,
                                           ModifiedDate = b.ModifiedDate,
                                           ToUseTSPDUNS = b.ToUseTSPDUNS,
                                           TempItem = b.DUNSNo + "-" + b.ModelTypeID
                                           //IsNoms = a.IsNoms,
                                           //IsUPRD = a.IsUPRD
                                       }).Distinct().OrderBy(c => c.Name).ToList();

            return Query;
        }

        public Pipeline GetPipelineByDuns(string DunsNo)
        {
            var Pipeline = this.DbContext.Pipeline.Where(c => c.DUNSNo == DunsNo).FirstOrDefault();
            return Pipeline;
        }


        public UPRD.Enums.NomType GetPathTypeByPipelineDuns(string pipelineDuns)
        {
            UPRD.Enums.NomType modelType = new UPRD.Enums.NomType();
            try
            {
                var pipeline = DbContext.Pipeline.Where(a => a.DUNSNo == pipelineDuns).FirstOrDefault();
                if (pipeline != null)
                {
                    if (pipeline.ModelTypeID == 1) { modelType = UPRD.Enums.NomType.Pathed; }
                    else if (pipeline.ModelTypeID == 2) { modelType = UPRD.Enums.NomType.PNT; }
                    else if (pipeline.ModelTypeID == 3) { modelType = UPRD.Enums.NomType.NonPathed; }
                    else if (pipeline.ModelTypeID == 4) { modelType = UPRD.Enums.NomType.HyPathedNonPathed; }
                    else if (pipeline.ModelTypeID == 5) { modelType = UPRD.Enums.NomType.HyPathedPNT; }
                    else if (pipeline.ModelTypeID == 6) { modelType = UPRD.Enums.NomType.HyNonPathedPNT; }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Pathtype not found for this duns.");
            }
            return modelType;
        }

        public void SaveChanges()
        {
            this.DbContext.SaveChanges();
        }

        public IEnumerable<Pipeline> GetActiveUprdPipelines()
        {
            return this.DbContext.Pipeline.Where(a => a.IsActive && a.IsUprdActive);
        }


       //<Admin methods>
        public IEnumerable<PipelineDTO> GetPipelinesByUserDto(PipelineByUserDTO pipelineByUser)
        {


            List<PipelineDTO> Query = (from a in DbContext.UserPipelineMappings
                                       join b in DbContext.Pipeline on a.PipeDuns equals b.DUNSNo
                                       where a.shipperId == pipelineByUser.ShipperID && a.userId == pipelineByUser.UserID && (a.IsNoms || a.IsUPRD)
                                       select new PipelineDTO
                                       {
                                           ID = b.ID,
                                           DUNSNo = b.DUNSNo,
                                           Name = b.Name + " (" + b.DUNSNo + ")",
                                           ModelTypeID = b.ModelTypeID,
                                           IsUprdActive = b.IsUprdActive,
                                           IsActive = b.IsActive,
                                           TSPId = b.TSPId,
                                           CreatedBy = b.CreatedBy,
                                           CreatedDate = b.CreatedDate,
                                           ModifiedBy = b.ModifiedBy,
                                           ModifiedDate = b.ModifiedDate,
                                           ToUseTSPDUNS = b.ToUseTSPDUNS,
                                           TempItem = b.DUNSNo + "-" + b.ModelTypeID,
                                           IsNoms = a.IsNoms,
                                           IsUPRD = a.IsUPRD
                                       }).Distinct().OrderBy(c => c.Name).ToList();

            return Query;

        }
        public IEnumerable<Pipeline> GetActivePipelineList(int CompanyID, string userId)
        {

            var query = (from a in DbContext.UserPipelineMappings
                         join b in DbContext.Pipeline on a.PipeDuns equals b.DUNSNo
                         where a.shipperId == CompanyID && a.userId == userId
                         select b).Distinct().OrderBy(c => c.Name).ToList();


            if (query.Count > 0)
            {
                return query;
            }
            else
            {
                return DbContext.Pipeline.Where(a => a.IsActive).OrderBy(a => a.Name).ToList();
            }

        }

        //<Admin methods>

        //public int GetTranTypeMapId(string PipeDuns, string TranTypeIden, string TranTypeDesc)
        //{
        //    var ttmap = (from tt in this.DbContext.metadataTransactionType
        //                 join ttm in this.DbContext.Pipeline_TransactionType_Map on tt.ID equals ttm.TransactionTypeID
        //                 where ttm.PipeDuns == PipeDuns
        //                 && tt.Name == TranTypeDesc && tt.Identifier == TranTypeIden
        //                 select ttm).FirstOrDefault();
        //    if (ttmap == null)
        //        return 0;
        //    return ttmap.ID;
        //}

        //public IEnumerable<Pipeline> GetActivePipelineList(int CompanyID)
        //{
        //    var query = (from a in DbContext.Shipper
        //                 join b in DbContext.ShipperCompany_Pipeline_Map on a.ShipperCompanyID equals b.CompanyID
        //                 join c in DbContext.Pipeline on b.PipelineID equals c.ID
        //                 where a.ShipperCompanyID == CompanyID
        //                 select c).Distinct().OrderBy(c => c.Name).ToList();
        //    if (query.Count > 0)
        //    {
        //        return query;
        //    }
        //    else
        //    {
        //        return DbContext.Pipeline.Where(a => a.IsActive).OrderBy(a => a.Name).ToList();
        //    }
        //    //var PipelineList = DbContext.ShipperCompany_Pipeline_Map.Where(a => a.CompanyID == CompanyID).Select(a => a.PipelineID).ToList();
        //    //if (PipelineList.Count > 0 && PipelineList[0] == -1)
        //    //{
        //    //    return DbContext.Pipeline.Where(a => a.IsActive).OrderBy(a => a.Name);
        //    //}
        //    //else
        //    //{

        //    //    return DbContext.Pipeline.Where(a => PipelineList.Contains(a.ID));
        //    //}




        //    //return DbContext.Pipeline.Where(a=>a.IsActive).OrderBy(a=>a.Name);
        //}

        //public Pipeline GetSelectedPipelineByUser(string UserId)
        //{
        //    var query = (from a in DbContext.Shipper
        //                 join b in DbContext.ShipperCompany_Pipeline_Map on a.ShipperCompanyID equals b.CompanyID
        //                 join c in DbContext.Pipeline on b.PipelineID equals c.ID
        //                 where a.UserId == UserId select c).OrderBy(c=>c.Name).FirstOrDefault();
        //    if (query != null)
        //    {
        //        return query;
        //    }
        //    else
        //    {
        //        return DbContext.Pipeline.Where(a => a.IsActive).OrderBy(a => a.Name).FirstOrDefault();
        //    }
        //}
    }

    public interface IUprdPipelineRepository : IRepository<Pipeline>
    {
        Pipeline GetPipelineByDuns(string DunsNo);
        IEnumerable<PipelineDTO> GetAllActivePipeline();
        IEnumerable<Pipeline> GetActiveUprdPipelines();
        UPRD.Enums.NomType GetPathTypeByPipelineDuns(string pipelineDuns);
        IEnumerable<PipelineDTO> GetPipelinesByUserDto(PipelineByUserDTO pipelineByUser);
        IEnumerable<Pipeline> GetActivePipelineList(int CompanyID, string userId);
        //Pipeline GetSelectedPipelineByUser(string UserId);
        void SaveChanges();
        //int GetTranTypeMapId(string PipeDuns, string TranTypeIden, string TranTypeDesc);
        //IEnumerable<Pipeline> GetActivePipelineList(int companyID);
    }
}
