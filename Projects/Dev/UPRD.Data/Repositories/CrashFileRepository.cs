using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using UPRD.DTO;
using UPRD.Infrastructure;
using UPRD.Models;

namespace UPRD.Data.Repositories
{
    public class CrashFileRepository: RepositoryBase<CrashFileDTO>, ICrashFileRepository
    {
        public CrashFileRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            
        }

        public byte[] GetFileData(string FileName, string FilePath)
        {
            try
            {
                byte[] fileData=null;
                string[] files = Directory.GetFiles(FilePath);
                foreach (var file in files)
                {
                    if (Path.GetFileNameWithoutExtension(file) == FileName)
                    {
                        fileData = File.ReadAllBytes(file);
                    }
                }
                return fileData;
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }

        public List<CrashFileDTO> GetFiles(string ShipperDuns, string FilePath)
        {
            try
            {
                List<CrashFileDTO> FileInfo = new List<CrashFileDTO>();
                string[] files = Directory.GetFiles(FilePath);
                    
                foreach (string file in files)
                {
                    FileInfo.Add(new CrashFileDTO
                    {
                         FileName=Path.GetFileNameWithoutExtension(file),
                         CreatedOn=File.GetCreationTime(FilePath),
                         ShipperDuns=ShipperDuns
                    });
                }
                return FileInfo;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error:", ex.Message);
                return null;
            }
        }
    }

    public interface ICrashFileRepository : IRepository<CrashFileDTO>
    {
        List<CrashFileDTO> GetFiles(string ShipperDuns, string FilePath);
        byte[] GetFileData(string FileName, string FilePath);
    }
}
