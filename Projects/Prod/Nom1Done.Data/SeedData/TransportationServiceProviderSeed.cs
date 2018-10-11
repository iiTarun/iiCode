using Nom1Done.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace Nom1Done.Data.SeedData
{
    public class TransportationServiceProviderSeed
    {
        public static List<TransportationServiceProvider> GetTransportationServiceProvider()
        {
            List<TransportationServiceProvider> list = new List<TransportationServiceProvider>();
            ISheet sheet;
            HSSFWorkbook hssfwb = new HSSFWorkbook(File.OpenRead(HostingEnvironment.MapPath("~/SeedFiles/TSP.xls")));
            sheet = hssfwb.GetSheetAt(0);
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                TransportationServiceProvider tsp = new TransportationServiceProvider();
                if (sheet.GetRow(row) != null)
                {
                    tsp.ID = Convert.ToInt32(sheet.GetRow(row).GetCell(0).NumericCellValue);
                    tsp.Name = sheet.GetRow(row).GetCell(1).StringCellValue;
                    tsp.DUNSNo = sheet.GetRow(row).GetCell(2) != null ? sheet.GetRow(row).GetCell(2).StringCellValue : "";
                    tsp.IsActive = sheet.GetRow(row).GetCell(3) != null ? sheet.GetRow(row).GetCell(3).NumericCellValue == 0 ? false : true : false;
                    tsp.CreatedBy = "";
                    tsp.CreatedDate = DateTime.Now;
                    tsp.ModifiedBy = "";
                    tsp.ModifiedDate = DateTime.Now;
                    list.Add(tsp);
                }
            }
            return list;
        }
    }
}
