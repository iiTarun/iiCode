
using System.Collections.Generic;

using UPRD.DTO;

namespace UPRD.Services.Interface
{
   public interface IUprdLocationService
    {
        List<LocationsDTO> GetLocations(int PipelineID);
        List<LocationsDTO> GetLocationUsingDuns(string Keyword, string PipelineDuns);
        List<LocationsDTO> GetLocationByPipeline(string pipelineDuns);
        LocationsDTO GetLocationById(int id);
        bool AddLocationByPipeline(List<LocationsDTO> locList, string pipeDuns);
        bool DeleteLocationByID(int id);
        bool UpdateLocationByID(LocationsDTO loc);
        LocationsResultDTO AddLocsNotInTblLoc(string pipelineDuns);
    }
  
}
