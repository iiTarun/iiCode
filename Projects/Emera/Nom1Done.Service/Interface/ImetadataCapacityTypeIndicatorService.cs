using Nom1Done.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Service.Interface
{
    public interface ImetadataCapacityTypeIndicatorService
    {
        List<CapacityIndicatorDTO> GetCapacityTypes();
    }
}
