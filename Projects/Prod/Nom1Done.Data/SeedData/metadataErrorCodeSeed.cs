using Nom1Done.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace Nom1Done.Data.SeedData
{
    public class metadataErrorCodeSeed
    {
        public static List<metadataErrorCode> GetErrorCode()
        {
            List<metadataErrorCode> list = new List<metadataErrorCode>();
            ISheet sheet;
            HSSFWorkbook hssfwb = new HSSFWorkbook(File.OpenRead(HostingEnvironment.MapPath("~/SeedFiles/metadataErrorCode.xls")));
            sheet = hssfwb.GetSheetAt(0);
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                metadataErrorCode metadata = new metadataErrorCode();
                if (sheet.GetRow(row) != null)
                {
                    metadata.Code = sheet.GetRow(row).GetCell(1).StringCellValue;
                    metadata.DataElement = sheet.GetRow(row).GetCell(2).StringCellValue;
                    metadata.Description = Convert.ToString(sheet.GetRow(row).GetCell(3).StringCellValue);
                    metadata.IsRequired = sheet.GetRow(row).GetCell(4).NumericCellValue == 0 ? false : true;
                    metadata.IsActive = sheet.GetRow(row).GetCell(5).NumericCellValue == 0 ? false : true;
                }
                list.Add(metadata);
            }
            return list;
        }
    }
}
