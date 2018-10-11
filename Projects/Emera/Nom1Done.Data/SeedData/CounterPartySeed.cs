using Nom1Done.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace Nom1Done.Data.SeedData
{
    public class CounterPartySeed
    {
        public static List<CounterParty> GetCounterParty()
        {
            List<CounterParty> list = new List<CounterParty>();
            ISheet sheet;
            HSSFWorkbook hssfwb = new HSSFWorkbook(File.OpenRead(HostingEnvironment.MapPath("~/SeedFiles/CounterParty.xls")));
            sheet = hssfwb.GetSheetAt(0);
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                CounterParty con = new CounterParty();
                if (sheet.GetRow(row) != null)
                {
                    con.Name = sheet.GetRow(row).GetCell(1).StringCellValue;
                    con.Identifier = sheet.GetRow(row).GetCell(2).StringCellValue;
                    con.PropCode = sheet.GetRow(row).GetCell(3).StringCellValue;
                    con.PipelineID = 0;
                    con.IsActive = sheet.GetRow(row).GetCell(5).NumericCellValue == 0 ? false : true;
                    con.CreatedBy = sheet.GetRow(row).GetCell(6).StringCellValue;
                    con.CreatedDate = DateTime.Now;
                    con.ModifiedBy = sheet.GetRow(row).GetCell(8).StringCellValue;
                    con.ModifiedDate = DateTime.Now;
                    list.Add(con);
                }
            }
            return list;
        }
    }
}
