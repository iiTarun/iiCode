using Nom1Done.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace Nom1Done.Data.SeedData
{
    public class LocationsSeed
    {
        public static List<Location> GetLocations()
        {
            List<Location> lst = new List<Location>();
            ISheet sheet;
            HSSFWorkbook hssfwb = new HSSFWorkbook(File.OpenRead(HostingEnvironment.MapPath("~/SeedFiles/NewLocations.xls")));
            sheet = hssfwb.GetSheetAt(0);
            for (int row = 1; row <= sheet.LastRowNum; row++) 
            {
                Location model = new Location();
                if (sheet.GetRow(row) != null)
                {
                    model.Name = sheet.GetRow(row).GetCell(1).StringCellValue;
                    model.Identifier = sheet.GetRow(row).GetCell(2).StringCellValue;
                    model.PropCode  = sheet.GetRow(row).GetCell(3).StringCellValue;
                    model.RDUsageID = Convert.ToInt32(sheet.GetRow(row).GetCell(4).NumericCellValue);
                    model.PipelineID = Convert.ToInt32(sheet.GetRow(row).GetCell(5).NumericCellValue);
                    model.IsActive = true;
                    model.CreatedBy= "";
                    model.ModifiedBy = "";
                    model.CreatedDate = DateTime.Now;
                    model.ModifiedDate = DateTime.Now;
                    model.PipeDuns = sheet.GetRow(row).GetCell(11).StringCellValue;
                    lst.Add(model);
                }
            }

            return lst;
        }
    }
}
