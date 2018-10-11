using Nom1Done.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Hosting;

namespace Nom1Done.Data.SeedData
{
    public class ContractSeed
    {
        public static List<Contract> GetContract()
        {
            List<Contract> list = new List<Contract>();
            ISheet sheet;
            HSSFWorkbook hssfwb = new HSSFWorkbook(File.OpenRead(SeedPathHelper.MapPath("~/SeedFiles/NewContracts.xls")));
            //HSSFWorkbook hssfwb = new HSSFWorkbook(File.OpenRead(HostingEnvironment.MapPath("~/SeedFiles/NewContracts.xls")));
            sheet = hssfwb.GetSheetAt(0);
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                Contract con = new Contract();
                if (sheet.GetRow(row) != null)
                {
                    con.ID = Convert.ToInt32(sheet.GetRow(row).GetCell(0).NumericCellValue);
                    con.RequestNo = sheet.GetRow(row).GetCell(1).StringCellValue;
                    con.RequestTypeID = Convert.ToInt32(sheet.GetRow(row).GetCell(2).NumericCellValue);
                    con.FuelPercentage = Convert.ToDecimal(sheet.GetRow(row).GetCell(3).NumericCellValue);
                    con.MDQ = Convert.ToDecimal(sheet.GetRow(row).GetCell(4).NumericCellValue);
                    con.LocationFromID = Convert.ToInt32(sheet.GetRow(row).GetCell(5).NumericCellValue);
                    con.LocationToID = Convert.ToInt32(sheet.GetRow(row).GetCell(6).NumericCellValue);
                    con.ValidUpto = sheet.GetRow(row).GetCell(7).DateCellValue;
                    con.PipelineID = Convert.ToInt32(sheet.GetRow(row).GetCell(8).NumericCellValue);
                    con.ShipperID = Convert.ToInt32(sheet.GetRow(row).GetCell(9).NumericCellValue);
                    con.IsActive = sheet.GetRow(row).GetCell(10).NumericCellValue == 0 ? false : true;
                    con.CreatedBy = "";
                    con.CreatedDate = DateTime.Now;
                    con.ModifiedBy = "";
                    con.ModifiedDate = DateTime.Now;
                    con.ReceiptZone = sheet.GetRow(row).GetCell(15) != null ? sheet.GetRow(row).GetCell(15).StringCellValue : "";
                    con.DeliveryZone = sheet.GetRow(row).GetCell(16) != null ? sheet.GetRow(row).GetCell(16).StringCellValue : "";
                    con.PipeDuns = sheet.GetRow(row).GetCell(17) != null ? sheet.GetRow(row).GetCell(17).StringCellValue : "";
                    list.Add(con);
                }
            }
            return list;
        }
    }
}
