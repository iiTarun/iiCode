using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System.Linq;
using System.Collections.Generic;
using Nom.ViewModel;
using System;

namespace Nom1Done.Data.Repositories
{
    public class metadataTransactionTypeRepository : RepositoryBase<metadataTransactionType>, ImetadataTransactionTypeRepository
    {
        public metadataTransactionTypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public metadataTransactionType GetTTUsingIdentifier(string TTIdentifier, string pathType, int pipelineId)
        {
            var tt= DbContext.metadataTransactionType.Where(a => a.IsActive
                    && a.Identifier == TTIdentifier
                    && DbContext.Pipeline_TransactionType_Map.Any(b => b.IsActive && b.PipelineID == pipelineId && b.PathType.Trim() == pathType && b.TransactionTypeID == a.ID)).FirstOrDefault();           
            return tt;
        }

        public List<TransactionTypesDTO> GetTTForHybridWithNonPathed(string pipelineDuns,string pathType)
        {
            List<TransactionTypesDTO> resultantData = new List<TransactionTypesDTO>();
            var data= (from pt in DbContext.Pipeline_TransactionType_Map
                             join tt in DbContext.metadataTransactionType on pt.TransactionTypeID equals tt.ID
                             where pt.PipeDuns == pipelineDuns && pt.IsActive == true && (pt.PathType.Trim() == pathType || pt.PathType.Trim() == "NP")
                             && tt.IsActive 
                             select new{
                             id= pt.ID,
                             name= tt.Name,
                             identifier=tt.Identifier,
                             sequence=tt.SequenceNo,
                             isActive=tt.IsActive,
                             pathType=pt.PathType
                             }).ToList();

            foreach (var tt in data)
            {
                TransactionTypesDTO itemObj = new TransactionTypesDTO();
                itemObj.ID = tt.id;
                itemObj.Identifier = tt.identifier;
                itemObj.Name = tt.name;
                itemObj.SequenceNo = tt.sequence;
                itemObj.IsActive = tt.isActive;
                itemObj.PathType = tt.pathType;
                resultantData.Add(itemObj);
            }
            return resultantData;
        }

        public List<TransactionTypesDTO> GetTTByMappingPipeline(string pipelineDuns, string pathType)
        {
            List<TransactionTypesDTO> resultantData = new List<TransactionTypesDTO>();
            var data = (from pt in DbContext.Pipeline_TransactionType_Map
                        join tt in DbContext.metadataTransactionType on pt.TransactionTypeID equals tt.ID
                        where pt.PipeDuns == pipelineDuns && pt.IsActive == true && (pt.PathType.Trim() == pathType)
                        && tt.IsActive
                        select new
                        {
                            id = pt.ID,
                            name = tt.Name,
                            identifier = tt.Identifier,
                            sequence = tt.SequenceNo,
                            isActive = tt.IsActive,
                            pathType = pt.PathType
                        }).ToList();

            foreach (var tt in data)
            {
                TransactionTypesDTO itemObj = new TransactionTypesDTO();
                itemObj.ID = tt.id;
                itemObj.Identifier = tt.identifier;
                itemObj.Name = tt.name;
                itemObj.SequenceNo = tt.sequence;
                itemObj.IsActive = tt.isActive;
                itemObj.PathType = tt.pathType;
                resultantData.Add(itemObj);
            }
            return resultantData;
        }

        public TransactionTypesDTO GetTTUsingttnameTTCode(string TTidentifier, string TTName, string PipelineDuns)
        {
            TransactionTypesDTO itemObj = new TransactionTypesDTO();
            var data = (from pt in DbContext.Pipeline_TransactionType_Map
                        join tt in DbContext.metadataTransactionType on pt.TransactionTypeID equals tt.ID
                        where pt.PipeDuns == PipelineDuns && pt.IsActive == true && tt.IsActive
                        && tt.Name == TTName && tt.Identifier== TTidentifier
                        select new
                        {
                            id = pt.ID,
                            name = tt.Name,
                            identifier = tt.Identifier,
                            sequence = tt.SequenceNo,
                            isActive = tt.IsActive,
                            pathType = pt.PathType
                        }).FirstOrDefault();

            if (data != null)
            {
                
                itemObj.ID = data.id;
                itemObj.Identifier = data.identifier;
                itemObj.Name = data.name;
                itemObj.SequenceNo = data.sequence;
                itemObj.IsActive = data.isActive;
                itemObj.PathType = data.pathType;
            }
            return itemObj;
        }

        public TransactionTypesDTO GetTTUsingMapId(int ttMapId)
        {
            TransactionTypesDTO itemObj = new TransactionTypesDTO();
            var data = (from pt in DbContext.Pipeline_TransactionType_Map
                        join tt in DbContext.metadataTransactionType on pt.TransactionTypeID equals tt.ID
                        where pt.ID == ttMapId && pt.IsActive == true && tt.IsActive
                        select new
                        {
                            id = pt.ID,
                            name = tt.Name,
                            identifier = tt.Identifier,
                            sequence = tt.SequenceNo,
                            isActive = tt.IsActive,
                            pathType = pt.PathType
                        }).FirstOrDefault();

            if(data!=null)
            {
                
                itemObj.ID = data.id;
                itemObj.Identifier = data.identifier;
                itemObj.Name = data.name;
                itemObj.SequenceNo = data.sequence;
                itemObj.IsActive = data.isActive;
                itemObj.PathType = data.pathType;                
            }
            return itemObj;
        }

        public bool FindIsSpecialLocsUsingTTPipeMapId(int ttPipeMapId) {
            try {
                return DbContext.Pipeline_TransactionType_Map.Where(a => a.ID == ttPipeMapId).FirstOrDefault().IsSpecialLocs;
            } catch (Exception ex) {
                return false;
            }
           
        }


    }
    public interface ImetadataTransactionTypeRepository:IRepository<metadataTransactionType>
    {
        bool FindIsSpecialLocsUsingTTPipeMapId(int ttPipeMapId);
        TransactionTypesDTO GetTTUsingMapId(int ttMapId);
        TransactionTypesDTO GetTTUsingttnameTTCode(string TTidentifier, string TTName, string PipelineDuns);

        metadataTransactionType GetTTUsingIdentifier(string TTIdentifier, string TTfrom, int PipelineId);

        List<TransactionTypesDTO> GetTTForHybridWithNonPathed(string pipelineDuns, string pathType);

        List<TransactionTypesDTO> GetTTByMappingPipeline(string pipelineDuns, string pathtype);
    }
}
