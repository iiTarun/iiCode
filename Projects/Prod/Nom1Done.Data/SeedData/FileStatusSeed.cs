using Nom1Done.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Nom1Done.Data.SeedData
{
   public class FileStatusSeed
    {
        public static List<metadataFileStatu> GetFileStatus()
        {
            List<metadataFileStatu> filestatusList = new List<metadataFileStatu>();
            ISheet sheet;
            HSSFWorkbook hssfwb = new HSSFWorkbook(File.OpenRead(HostingEnvironment.MapPath("~/SeedFiles/EnFileStatus.xls")));
            sheet = hssfwb.GetSheetAt(0);
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                metadataFileStatu filestatus = new metadataFileStatu();
                if (sheet.GetRow(row) != null)
                {                 
                    filestatus.Name = sheet.GetRow(row).GetCell(1) != null ? sheet.GetRow(row).GetCell(1).StringCellValue : string.Empty;
                }
                filestatusList.Add(filestatus);
            }
            return filestatusList;
        }
    }
}
