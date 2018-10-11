using Nom1Done.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace Nom1Done.Data.SeedData
{
    public class PipelineTransactionTypeMapSeed
    {
        public static List<Pipeline_TransactionType_Map> GetPipelineTransactionTypeMap()
        {
            List<Pipeline_TransactionType_Map> list = new List<Pipeline_TransactionType_Map>();
            ISheet sheet;
            HSSFWorkbook hssfwb = new HSSFWorkbook(File.OpenRead(HostingEnvironment.MapPath("~/SeedFiles/PipelinesTransactionMap.xls")));
            sheet = hssfwb.GetSheetAt(0);
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                Pipeline_TransactionType_Map map = new Pipeline_TransactionType_Map();
                if (sheet.GetRow(row) != null)
                {
                    map.PipelineID=Convert.ToInt32(sheet.GetRow(row).GetCell(1).NumericCellValue);
                    map.TransactionTypeID= Convert.ToInt32(sheet.GetRow(row).GetCell(2).NumericCellValue);
                    map.IsActive = sheet.GetRow(row).GetCell(3).NumericCellValue == 0 ? false : true;
                    map.CreatedBy = "";
                    map.CreatedDate = DateTime.Now;
                    map.LastModifiedBy = "";
                    map.LastModifiedDate = DateTime.Now;
                    map.PathType = sheet.GetRow(row).GetCell(8).StringCellValue;
                    list.Add(map);
                }
            }
            return list;
        }
    }
}
