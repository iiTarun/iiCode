using Nom.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Service.Interface
{
   public interface ILocationService
    {
        IEnumerable<LocationsDTO> GetAll();
        List<LocationsDTO> GetLocations(int PipelineID);

        List<LocationsDTO> GetLocationUsingDuns(string Keyword, string PipelineDuns);
    }
}
