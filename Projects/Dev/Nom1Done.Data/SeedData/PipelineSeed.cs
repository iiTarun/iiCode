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
    public class PipelineSeed
    {
        public static List<Pipeline> GetPipelines()
        {
            List<Pipeline> lst = new List<Pipeline>();
            ISheet sheet;
            HSSFWorkbook hssfwb = new HSSFWorkbook(File.OpenRead(HostingEnvironment.MapPath("~/SeedFiles/Pipelines.xls")));
            sheet = hssfwb.GetSheetAt(0);
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                Pipeline model = new Pipeline();
                if (sheet.GetRow(row) != null)
                {
                    model.ID= Convert.ToInt32(sheet.GetRow(row).GetCell(0).NumericCellValue);
                    model.Name = sheet.GetRow(row).GetCell(1).StringCellValue;
                    model.DUNSNo = sheet.GetRow(row).GetCell(2).StringCellValue;
                    model.TSPId = Convert.ToInt32(sheet.GetRow(row).GetCell(3).NumericCellValue);
                    model.ModelTypeID = Convert.ToInt32(sheet.GetRow(row).GetCell(4).NumericCellValue);
                    model.ToUseTSPDUNS = sheet.GetRow(row).GetCell(5).NumericCellValue == 0 ? false : true;
                    model.IsActive = sheet.GetRow(row).GetCell(6).NumericCellValue == 0 ? false:true;
                    model.CreatedBy = "";
                    model.ModifiedBy = "";
                    model.CreatedDate = DateTime.Now;
                    model.ModifiedDate = DateTime.Now;
                    lst.Add(model);
                }
            }
            return lst;
        }
    }
}
