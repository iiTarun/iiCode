using Nom1Done.DTO;
using System.Collections.Generic;

namespace Nom1Done.Service.Interface
{
    public interface ISettingService
    {
        SettingDTO GetById(int Id);
        IEnumerable<SettingDTO> GetAll();

    }
}
