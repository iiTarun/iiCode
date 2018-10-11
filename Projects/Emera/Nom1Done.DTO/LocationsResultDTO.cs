using Nom.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.DTO
{
  public class LocationsResultDTO
    {
        public List<LocationsDTO> locationsDTO { get; set; }
        public int RecordCount { get; set; }
    }
}
