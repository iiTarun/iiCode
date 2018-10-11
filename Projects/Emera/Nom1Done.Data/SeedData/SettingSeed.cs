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
   public class SettingSeed
    {
        public static List<Setting> GetSettingData()
        {
            List<Setting> list = new List<Setting>();
            ISheet sheet;
            HSSFWorkbook hssfwb = new HSSFWorkbook(File.OpenRead(HostingEnvironment.MapPath("~/SeedFiles/Ensetting.xls")));
            sheet = hssfwb.GetSheetAt(0);
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                Setting keyInfo = new Setting();
                if (sheet.GetRow(row) != null)
                {
                    keyInfo.ID = Convert.ToInt32(sheet.GetRow(row).GetCell(0).NumericCellValue);
                    keyInfo.Name = sheet.GetRow(row).GetCell(1) != null ? sheet.GetRow(row).GetCell(1).StringCellValue : string.Empty;
                    keyInfo.Value = sheet.GetRow(row).GetCell(2) != null ? sheet.GetRow(row).GetCell(2).StringCellValue : string.Empty; ;

                    keyInfo.IsActive = sheet.GetRow(row).GetCell(3).NumericCellValue == 0 ? false : true;
                    keyInfo.CreatedBy = sheet.GetRow(row).GetCell(4) != null ? sheet.GetRow(row).GetCell(4).StringCellValue : string.Empty; 
                    keyInfo.CreatedDate = sheet.GetRow(row).GetCell(5).DateCellValue; 
                    keyInfo.ModifiedBy = sheet.GetRow(row).GetCell(6) != null ? sheet.GetRow(row).GetCell(6).StringCellValue : string.Empty; ;
                    keyInfo.ModifiedDate = sheet.GetRow(row).GetCell(7).DateCellValue;
                    list.Add(keyInfo);
                }

            }

            return list;
      }
    }
}
