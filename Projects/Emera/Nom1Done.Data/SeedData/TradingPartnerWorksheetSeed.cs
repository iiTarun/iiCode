using Nom1Done.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace Nom1Done.Data.SeedData
{
    public class TradingPartnerWorksheetSeed
    {
        public static List<TradingPartnerWorksheet> GetTradingPartnerWorksheet()
        {
            List<TradingPartnerWorksheet> list = new List<TradingPartnerWorksheet>();
            ISheet sheet;
            HSSFWorkbook hssfwb = new HSSFWorkbook(File.OpenRead(HostingEnvironment.MapPath("~/SeedFiles/TPW.xls")));
            sheet = hssfwb.GetSheetAt(0);
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                TradingPartnerWorksheet tpw = new TradingPartnerWorksheet();
                if (sheet.GetRow(row) != null)
                {
                    tpw.ID = Convert.ToInt32(sheet.GetRow(row).GetCell(0).NumericCellValue);
                    tpw.Name = sheet.GetRow(row).GetCell(1).StringCellValue;
                    tpw.PipelineID = Convert.ToInt32(sheet.GetRow(row).GetCell(2).NumericCellValue);
                    tpw.UsernameLive = sheet.GetRow(row).GetCell(3) != null ? sheet.GetRow(row).GetCell(3).StringCellValue : "";
                    tpw.PasswordLive = sheet.GetRow(row).GetCell(3) != null ? sheet.GetRow(row).GetCell(4).StringCellValue : "";
                    tpw.URLLive = sheet.GetRow(row).GetCell(5) != null ? sheet.GetRow(row).GetCell(5).StringCellValue : "";
                    tpw.KeyLive = sheet.GetRow(row).GetCell(6) != null ? sheet.GetRow(row).GetCell(6).StringCellValue : "";
                    tpw.UsernameTest = sheet.GetRow(row).GetCell(7) != null ? sheet.GetRow(row).GetCell(7).StringCellValue : "";
                    tpw.PasswordTest = sheet.GetRow(row).GetCell(8) != null ? sheet.GetRow(row).GetCell(8).StringCellValue : "";
                    tpw.URLTest = sheet.GetRow(row).GetCell(9) != null ? sheet.GetRow(row).GetCell(9).StringCellValue : "";
                    tpw.KeyTest = sheet.GetRow(row).GetCell(10) != null ? sheet.GetRow(row).GetCell(10).StringCellValue : "";
                    tpw.ReceiveSubSeperator = "";
                    tpw.ReceiveDataSeperator = "";
                    tpw.ReceiveSegmentSeperator = "";
                    tpw.SendSubSeperator = "";
                    tpw.SendDataSeperator = "";
                    tpw.SendSegmentSeperator = "";
                    tpw.IsTest= sheet.GetRow(row).GetCell(17).NumericCellValue==0?false:true;
                    tpw.IsActive= sheet.GetRow(row).GetCell(18).NumericCellValue == 0 ? false : true;
                    tpw.CreatedBy = "";
                    tpw.CreatedDate = DateTime.Now;
                    tpw.ModifiedBy = "";
                    tpw.ModifiedDate = DateTime.Now;
                    list.Add(tpw);
                }
            }
            return list;
        }
    }
}
