using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatchlistMailManagement.UPRD.DTO
{
    public class LocationsResultDTO
    {
        public List<LocationsDTO> locationsDTO { get; set; }
        public int RecordCount { get; set; }
    }
}