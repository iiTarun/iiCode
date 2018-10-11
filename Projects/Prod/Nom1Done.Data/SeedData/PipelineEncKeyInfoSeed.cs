using Nom1Done.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace Nom1Done.Data.SeedData
{
    public class PipelineEncKeyInfoSeed
    {
        public static List<metadataPipelineEncKeyInfo> GetPipelineEncKeyInfo()
        {
            List<metadataPipelineEncKeyInfo> list = new List<metadataPipelineEncKeyInfo>();
            ISheet sheet;
            HSSFWorkbook hssfwb = new HSSFWorkbook(File.OpenRead(HostingEnvironment.MapPath("~/SeedFiles/PipelinesEcryptionKeyInfo.xls")));
            sheet = hssfwb.GetSheetAt(0);
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                metadataPipelineEncKeyInfo keyInfo = new metadataPipelineEncKeyInfo();
                if (sheet.GetRow(row) != null)
                {
                    keyInfo.PipelineId = Convert.ToInt32(sheet.GetRow(row).GetCell(1).NumericCellValue);
                    keyInfo.KeyName = sheet.GetRow(row).GetCell(2)!=null? sheet.GetRow(row).GetCell(2).StringCellValue:string.Empty;
                    keyInfo.PipeDuns = "";
                    list.Add(keyInfo);
                }
            }
            return list;
        }
    }
}
