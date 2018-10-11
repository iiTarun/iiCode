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
   public class EmailTemplateSeed
    {

        public static List<EmailTemplate> GetEmailSeed() {
            List<EmailTemplate> list = new List<EmailTemplate>();
            ISheet sheet;
            HSSFWorkbook hssfwb = new HSSFWorkbook(File.OpenRead(HostingEnvironment.MapPath("~/SeedFiles/EnEmailTemplate.xls")));
            sheet = hssfwb.GetSheetAt(0);
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                EmailTemplate metadata = new EmailTemplate();
                if (sheet.GetRow(row) != null)
                {
                    metadata.Id = Convert.ToInt32(sheet.GetRow(row).GetCell(0).NumericCellValue);
                    metadata.Name = sheet.GetRow(row).GetCell(1).StringCellValue;
                    metadata.Description = sheet.GetRow(row).GetCell(2) != null ? sheet.GetRow(row).GetCell(2).StringCellValue : "";
                    metadata.Subject = sheet.GetRow(row).GetCell(3) != null ? sheet.GetRow(row).GetCell(3).StringCellValue : "";
                    metadata.Body = sheet.GetRow(row).GetCell(4) != null ? sheet.GetRow(row).GetCell(4).StringCellValue : "";
                }
                list.Add(metadata);
            }
         return list;
        }

    }
}
