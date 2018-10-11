using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using Nom1Done.Enums;
using System;
using Nom1Done.DTO;

namespace Nom1Done.Data.Repositories
{
    public class PipelineRepository : RepositoryBase<Pipeline>, IPipelineRepository
    {
        public PipelineRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<Pipeline> GetAllActivePipeline()
        {
            return DbContext.Pipeline.Where(a => a.IsActive == true);
        }

        public Pipeline GetPipelineByDuns(string DunsNo)
        {
            var Pipeline = this.DbContext.Pipeline.Where(c => c.DUNSNo == DunsNo).FirstOrDefault();
            return Pipeline;
        }


        public NomType GetPathTypeByPipelineDuns(string pipelineDuns)
        {
            NomType modelType = new NomType();
            try
            {
                var pipeline = DbContext.Pipeline.Where(a => a.DUNSNo == pipelineDuns).FirstOrDefault();
                if (pipeline != null)
                {
                    if (pipeline.ModelTypeID == 1) { modelType = NomType.Pathed; }
                    else if (pipeline.ModelTypeID == 2) { modelType = NomType.PNT; }
                    else if (pipeline.ModelTypeID == 3) { modelType = NomType.NonPathed; }
                    else if (pipeline.ModelTypeID == 4) { modelType = NomType.HyPathedNonPathed; }
                    else if (pipeline.ModelTypeID == 5) { modelType = NomType.HyPathedPNT; }
                    else if (pipeline.ModelTypeID == 6) { modelType = NomType.HyNonPathedPNT; }
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

        public int GetTranTypeMapId(string PipeDuns, string TranTypeIden, string TranTypeDesc)
        {
            var ttmap = (from tt in this.DbContext.metadataTransactionType
                         join ttm in this.DbContext.Pipeline_TransactionType_Map on tt.ID equals ttm.TransactionTypeID
                         where ttm.PipeDuns == PipeDuns
                         && tt.Name == TranTypeDesc && tt.Identifier == TranTypeIden
                         select ttm).FirstOrDefault();
            if (ttmap == null)
                return 0;
            return ttmap.ID;
        }

        public IEnumerable<Pipeline> GetActivePipelineList(int CompanyID, string userId)
        {
            var query = (from a in DbContext.UserPipelineMapping
                         join b in DbContext.Pipeline on a.pipelineId equals b.ID
                         where a.shipperId == CompanyID && a.userId == userId
                         select b).Distinct().OrderBy(c => c.Name).ToList();

            //var query = (from a in DbContext.Shipper
            //             join b in DbContext.ShipperCompany_Pipeline_Map on a.ShipperCompanyID equals b.CompanyID
            //             join c in DbContext.Pipeline on b.PipelineID equals c.ID
            //             where a.ShipperCompanyID == CompanyID
            //             select c).Distinct().OrderBy(c => c.Name).ToList();
            if (query.Count > 0)
            {
                return query;
            }
            else
            {
                return DbContext.Pipeline.Where(a => a.IsActive).OrderBy(a => a.Name).ToList();
            }
            //var PipelineList = DbContext.ShipperCompany_Pipeline_Map.Where(a => a.CompanyID == CompanyID).Select(a => a.PipelineID).ToList();
            //if (PipelineList.Count > 0 && PipelineList[0] == -1)
            //{
            //    return DbContext.Pipeline.Where(a => a.IsActive).OrderBy(a => a.Name);
            //}
            //else
            //{

            //    return DbContext.Pipeline.Where(a => PipelineList.Contains(a.ID));
            //}




            //return DbContext.Pipeline.Where(a=>a.IsActive).OrderBy(a=>a.Name);
        }

        public Pipeline GetSelectedPipelineByUser(string UserId, int companyId)
        {
            //var query = (from a in DbContext.UserPipelineMapping
            //             join b in DbContext.Pipeline on a.pipelineId equals b.ID
            //             where a.shipperId == companyId && a.userId == UserId
            //             select b).Distinct().OrderBy(c => c.Name).FirstOrDefault();

            ////var query = (from a in DbContext.Shipper
            ////             join b in DbContext.ShipperCompany_Pipeline_Map on a.ShipperCompanyID equals b.CompanyID
            ////             join c in DbContext.Pipeline on b.PipelineID equals c.ID
            ////             where a.UserId == UserId select c).OrderBy(c=>c.Name).FirstOrDefault();
            //if (query != null)
            //{
            //    return query;
            //}
            //else
            //{
            //    return DbContext.Pipeline.Where(a => a.IsActive).OrderBy(a => a.Name).FirstOrDefault();
            //}
            return null;
        }
    }

    public interface IPipelineRepository : IRepository<Pipeline>
    {
        Pipeline GetPipelineByDuns(string DunsNo);
        IEnumerable<Pipeline> GetAllActivePipeline();
        IEnumerable<Pipeline> GetActiveUprdPipelines();
        NomType GetPathTypeByPipelineDuns(string pipelineDuns);
        Pipeline GetSelectedPipelineByUser(string UserId, int companyId);
        void SaveChanges();
        int GetTranTypeMapId(string PipeDuns, string TranTypeIden, string TranTypeDesc);
        IEnumerable<Pipeline> GetActivePipelineList(int companyID, string userId);
    }
}
