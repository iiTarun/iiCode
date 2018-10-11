
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPRD.Data.Repositories;
using UPRD.DTO;
using UPRD.Model;
using UPRD.Services.Interface;

namespace UPRD.Service.Interface
{
    public class UprdLocationService : IUprdLocationService
    {
        IUPRDLocationRepository _ILocationRepository;
        UprdPipelineRepository _IPipelineRepository;
        IModalFactory modalFactory;

        public UprdLocationService(IUPRDLocationRepository ILocationRepository, UprdPipelineRepository IPipelineRepository, IModalFactory modalFactory)
        {
            _ILocationRepository = ILocationRepository;
            _IPipelineRepository = IPipelineRepository;
            this.modalFactory = modalFactory;
        }

        public IEnumerable<LocationsDTO> GetAll()
        {
            return _ILocationRepository.GetAll().Select(a => modalFactory.Parse(a));
        }

        public List<LocationsDTO> GetLocations(int PipelineID)
        {
            var pipelineduns = _IPipelineRepository.GetById(PipelineID).DUNSNo;
            var filteredLocations = _ILocationRepository.GetLocationByPipeline(pipelineduns).ToList();

            return LocationStatusDTO(filteredLocations);
        }

        //public List<LocationsDTO> MappingFromLocationToLOcationsDTO(IEnumerable<Location> LocationList)
        //{
        //    List<LocationsDTO> locationDTOList = new List<LocationsDTO>();
        //    foreach (var Location in LocationList)
        //    {
        //        LocationsDTO locationDto = new LocationsDTO();
        //        locationDto.ID = Location.ID;
        //        locationDto.Name = Location.Name;
        //        locationDto.Identifier = Location.Identifier;
        //        locationDto.PropCode = Location.PropCode;
        //        locationDto.RDUsageID = Location.RDUsageID;
        //        if (locationDto.RDUsageID == 1)
        //        {
        //            locationDto.RDB = "R";
        //        }
        //        else if (locationDto.RDUsageID == 2)
        //        {
        //            locationDto.RDB = "D";
        //        }
        //        else
        //            locationDto.RDB = "B";
        //        locationDto.IsActive = Location.IsActive;
        //        locationDto.CreatedBy = Location.CreatedBy;
        //        locationDto.CreatedDate = Location.CreatedDate;
        //        locationDto.ModifiedBy = Location.ModifiedBy;
        //        locationDto.ModifiedDate = Location.ModifiedDate;
        //        locationDTOList.Add(locationDto);
        //    }
        //    return locationDTOList;
        //}


        public List<LocationsDTO> GetLocationUsingDuns(string Keyword, string PipelineDuns)
        {
            var result = _ILocationRepository.GetLocations(Keyword, PipelineDuns).ToList();
            return LocationStatusDTO(result);
        }
        public List<LocationsDTO> GetLocationByPipeline(string pipelineDuns)
        {
            var result = _ILocationRepository.GetLocationByPipeline(pipelineDuns);
            return LocationStatusDTO(result);

        }

        private List<LocationsDTO> LocationStatusDTO(List<Location> list)
        {
            List<LocationsDTO> locationStatusList = new List<LocationsDTO>();
            if (list != null && list.Count > 0)
                foreach (var item in list)
                {
                    locationStatusList.Add(modalFactory.Parse(item));
                }
            return locationStatusList;
        }
        private List<Location> LocationConvertDTO(List<LocationsDTO> list)
        {
            List<Location> locationCreateList = new List<Location>();
            if (list != null && list.Count > 0)
                foreach (var item in list)
                {
                    locationCreateList.Add(modalFactory.Create(item));
                }
            return locationCreateList;
        }

        public LocationsDTO GetLocationById(int id)
        {
            LocationsDTO result = new LocationsDTO();
            if (id != 0)
            {
                result = modalFactory.Parse(_ILocationRepository.GetLocationById(id));
            }
            else
            {
                result = new LocationsDTO();
            }
            if (result.RDUsageID == 1)
                result.RDB = "R";
            else if (result.RDUsageID == 2)
                result.RDB = "D";
            else
                result.RDB = "B";
            return result;
        }

        public bool AddLocationByPipeline(List<LocationsDTO> locList, string pipeDuns)
        {

            bool result = _ILocationRepository.AddLocationByPipeline(LocationConvertDTO(locList), pipeDuns);

            return result;
        }

        public bool DeleteLocationByID(int id)
        {
            bool result = _ILocationRepository.DeleteLocationByID(id);
            return result;
        }
        public bool UpdateLocationByID(LocationsDTO loc)
        {
            bool result = _ILocationRepository.UpdateLocationByID(modalFactory.Create(loc));
            return result;
        }
        public LocationsResultDTO AddLocsNotInTblLoc(string pipelineDuns)
        {
            LocationsResultDTO result = new LocationsResultDTO();
            result = _ILocationRepository.GetLocFromOacyUnsc(pipelineDuns);


            return result;
        }
        public bool ClientEnvironmentsetting()
        {
            _ILocationRepository.ClientEnvironmentsetting();

            return true;
        }

        public bool ActivateLocation(int id)
        {
            var Activate = _ILocationRepository.ActivateLocation(id);
            return Activate;
        }
    }


}