using System.Collections.Generic;
using UPRD.DTO;

namespace UPRD.Services.Interface
{
    public interface ICrashFileService
    {
        List<CrashFileDTO> GetFiles(string shipperName);
        byte[] GetFileData(string FileName, string ShipperDuns);
    }
}
