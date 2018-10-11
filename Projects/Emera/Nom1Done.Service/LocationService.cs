using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nom.ViewModel;
using Nom1Done.Data.Repositories;
using Nom1Done.Model;

namespace Nom1Done.Service
{
    public class LocationService : ILocationService
    {
        ILocationRepository _ILocationRepository;
        IPipelineRepository _IPipelineRepository;
        IModalFactory modalFactory;

        public LocationService(ILocationRepository ILocationRepository, IPipelineRepository IPipelineRepository, IModalFactory modalFactory) {
            _ILocationRepository = ILocationRepository;
            _IPipelineRepository = IPipelineRepository;
            this.modalFactory = modalFactory;
        }

        public IEnumerable<LocationsDTO> GetAll()
        {
            return _ILocationRepository.GetAll().Select(a=>modalFactory.Parse(a));
        }

        public List<LocationsDTO> GetLocations(int PipelineID)
        {    
            var pipelineduns = _IPipelineRepository.GetById(PipelineID).DUNSNo;
            List<LocationsDTO> filteredLocations= _ILocationRepository.GetLocations(string.Empty, pipelineduns);
         
          return filteredLocations;
        }

        public List<LocationsDTO> MappingFromLocationToLOcationsDTO(IEnumerable<Location>  LocationList)
        {
            List<LocationsDTO> locationDTOList = new List<LocationsDTO>();
            foreach (var Location in LocationList)
            {
                LocationsDTO locationDto = new LocationsDTO();
                locationDto.ID = Location.ID;
                locationDto.Name = Location.Name;
                locationDto.Identifier = Location.Identifier;
                locationDto.PropCode = Location.PropCode;
                locationDto.RDUsageID = Location.RDUsageID;
                if (locationDto.RDUsageID == 1)
                {
                    locationDto.RDB = "R";
                }
                else if (locationDto.RDUsageID == 2)
                {
                    locationDto.RDB = "D";
                }
                else
                    locationDto.RDB = "B";
                locationDto.IsActive = Location.IsActive;
                locationDto.CreatedBy = Location.CreatedBy;
                locationDto.CreatedDate = Location.CreatedDate;
                locationDto.ModifiedBy = Location.ModifiedBy;
                locationDto.ModifiedDate = Location.ModifiedDate;
                locationDTOList.Add(locationDto);
            }
            return locationDTOList;
        }


        public  List<LocationsDTO> GetLocationUsingDuns(string Keyword, string PipelineDuns)
        {
            return _ILocationRepository.GetLocations(Keyword, PipelineDuns);
        }

    }
}
