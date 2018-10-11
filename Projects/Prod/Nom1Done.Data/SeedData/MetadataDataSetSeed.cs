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
   public class MetadataDataSetSeed
    {

        public static List<metadataDataset> GetDataSet()
        {
            List<metadataDataset> list = new List<metadataDataset>();
            ISheet sheet;
            HSSFWorkbook hssfwb = new HSSFWorkbook(File.OpenRead(HostingEnvironment.MapPath("~/SeedFiles/EnMetadataDataSet.xls")));
            sheet = hssfwb.GetSheetAt(0);
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                metadataDataset metadata = new metadataDataset();
                if (sheet.GetRow(row) != null)
                {
                    metadata.Id = Convert.ToInt32(sheet.GetRow(row).GetCell(0).NumericCellValue);
                    metadata.Name = sheet.GetRow(row).GetCell(1).StringCellValue;
                    metadata.Identifier = sheet.GetRow(row).GetCell(2).StringCellValue;
                    metadata.Code = Convert.ToString(sheet.GetRow(row).GetCell(3).StringCellValue);
                    metadata.Description = sheet.GetRow(row).GetCell(4) != null ? sheet.GetRow(row).GetCell(4).StringCellValue : "";
                    metadata.CategoryID = Convert.ToInt32(sheet.GetRow(row).GetCell(5).NumericCellValue);
                    metadata.Direction = Convert.ToString(sheet.GetRow(row).GetCell(6).StringCellValue);
                    metadata.IsUPRDAttribute = sheet.GetRow(row).GetCell(7).NumericCellValue == 0 ? false : true;
                    metadata.IsActive = sheet.GetRow(row).GetCell(8).NumericCellValue == 0 ? false : true;
                }
                list.Add(metadata);
            }
           return list;
        }
    }
}
