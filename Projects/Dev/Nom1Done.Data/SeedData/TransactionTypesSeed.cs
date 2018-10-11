using Nom1Done.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace Nom1Done.Data.SeedData
{
    public class TransactionTypesSeed
    {
        public static List<metadataTransactionType> GetTransactionTypes()
        {
            List<metadataTransactionType> list = new List<metadataTransactionType>();
            ISheet sheet;
            HSSFWorkbook hssfwb = new HSSFWorkbook(File.OpenRead(HostingEnvironment.MapPath("~/SeedFiles/TransactionTypes.xls")));
            sheet = hssfwb.GetSheetAt(0);
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                metadataTransactionType tt = new metadataTransactionType();
                if (sheet.GetRow(row) != null)
                {
                    tt.ID = Convert.ToInt32(sheet.GetRow(row).GetCell(0).NumericCellValue);
                    tt.Identifier = sheet.GetRow(row).GetCell(1).StringCellValue;
                    tt.Name = sheet.GetRow(row).GetCell(2).StringCellValue;
                    tt.SequenceNo = Convert.ToInt32(sheet.GetRow(row).GetCell(3).NumericCellValue);
                    tt.IsActive = sheet.GetRow(row).GetCell(4).NumericCellValue == 0 ? false : true;
                    list.Add(tt);
                }
            }
            return list;

 //           return new List<metadataTransactionType>
 //           {
 //               new metadataTransactionType
 //               {
 //                   Identifier="02",
 //                   Name="Authorized Contract Overrun",
 //                   SequenceNo = 20,
 //                   IsActive=true
 //               },
 //               new metadataTransactionType
 //               {
 //                   Identifier="105",
 //                   Name="Authorized Contract Overrun - Firm",
 //                   SequenceNo = 20,
 //                   IsActive=true
 //               },
 //               new metadataTransactionType
 //               {
 //                   Identifier="106",
 //                   Name="Authorized Contract Overrun - Interruptible",
 //                   SequenceNo = 20,
 //                   IsActive=true
 //               },
 //               new metadataTransactionType
 //               {
 //                   Identifier="12",
 //                   Name="Authorized Injection Overrun",
 //                   SequenceNo = 20,
 //                   IsActive=true
 //               },


 //new metadataTransactionType{Identifier="2",Name="	Authorized Contract Overrun",SequenceNo = 20,IsActive=  true}
 //,new metadataTransactionType{Identifier="105",Name="Authorized Contract Overrun - Firm",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="106",Name="Authorized Contract Overrun - Interruptible",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="12",Name="Authorized Injection Overrun",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="135",Name="Authorized Overrun Incremental Capacity – TSP Defined 1",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="48",Name="Authorized Point Overrun",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="57",Name="Authorized Point Overrun – Backhaul",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="58",Name="Authorized Point Overrun – Shorthaul",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="13",Name="Authorized Withdrawal Overrun",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="55",Name="Backhaul",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="59",Name="Bank",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="60",Name="Bank Payback",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="1",Name="Current Business (Buy)",SequenceNo =  1   ,IsActive=  true}
 //,new metadataTransactionType{Identifier="51",Name="Capacity Release",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="10",Name="Cashout",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="1",Name="Current Business",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="19",Name="Delivery of Claimed Suspense Gas",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="66",Name="Excess Injection – Daily",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="65",Name="Excess Injection Storage injection in excess of inventory contractual rights. 65",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="67",Name="Excess Withdrawal",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="68",Name="Excess Withdrawal – Daily",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="14",Name="Extended Receipt/Delivery Service",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="136",Name="Facilities Reimbursement",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="138",Name="Flash Gas",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="56",Name="Flow Day Diversion",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="81",Name="Fuel Adjustment",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="49",Name="Gathering",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="123",Name="Hourly Overrun",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="124",Name="Hourly Overrun 1/12",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="125",Name="Hourly Overrun 1/16",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="112",Name="Imbalance Payback – Overrun",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="111",Name="Imbalance Payback – within Contract",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="3",Name="Imbalance Payback from Transportation Service Provider",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="63",Name="Imbalance Payback from Transportation Service Provider – Prior Business Period",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="4",Name="Imbalance Payback to Transportation Service Provider",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="64",Name="Imbalance Payback to Transportation Service Provider – Prior Business Period",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="103",Name="Imbalance Resolution – Firm",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="104",Name="Imbalance Resolution – Interruptible",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="9",Name="Imbalance Transfer",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="134",Name="Incremental Capacity – TSP Defined 1",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="126",Name="Incremental Reservation",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="69",Name="Inventory Addition",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="70",Name="Inventory Reduction",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="28",Name="Loan",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="29",Name="Loan Payback",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="108",Name="Lost/Unaccounted For Gas",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="31",Name="Meter Bounce",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="102",Name="Meter Bounce – Overrun",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="121",Name="Meter Bounce Delivery",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="122",Name="Meter Bounce Receipt",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="137",Name="Minimum Revenue Commitment",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="71",Name="Netting Injection",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="72",Name="Netting Withdrawal",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="98",Name="Nominated No-Notice",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="100",Name="Nominated No-Notice – Small Shipper",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="94",Name="Nominated No-Notice – Small Shipper / Firm Storage",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="95",Name="Nominated No-Notice – Small Shipper / Interruptible Storage",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="92",Name="Nominated No-Notice / Firm Storage",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="93",Name="Nominated No-Notice / Interruptible  Storage",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="101",Name="No-Notice – Small Shipper",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="16",Name="No-Notice Balancing",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="25",Name="No-Notice Due Service Requester Balancing",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="24",Name="No-Notice Due Transportation Service Provider Balancing",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="17",Name="No-Notice Pre-Injection",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="22",Name="No-Notice Service",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="117",Name="Off-system Market",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="118",Name="Off-system Supply",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="107",Name="Operational Balancing Agreement",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="140",Name="Out of Cycle",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="128",Name="Overrun – Seasonal",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="26",Name="Park",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="127",Name="Park and Loan Highest Daily Balance",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="139",Name="Park and Loan Highest Daily Balance",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="27",Name="Park Withdrawal",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="5",Name="Plant Thermal Reduction",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="8",Name="Pooling",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="54",Name="Pool-to-Pool",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="119",Name="Purchase",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="90",Name="Reservation / Firm Storage",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="91",Name="Reservation / Interruptible Storage",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="129",Name="Reservation Volume",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="133",Name="Reservation/Enhanced Hourly Flow",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="109",Name="Retrograde",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="110",Name="Retrograde – Flash",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="120",Name="Sale",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="84",Name="Segmented",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="85",Name="Segmented – Volumetric",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="88",Name="Segmented – Volumetric / Firm Storage",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="89",Name="Segmented – Volumetric / Interruptible Storage",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="86",Name="Segmented / Firm Storage",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="87",Name="Segmented / Interruptible Storage",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="1",Name="Current Business (Sell)",SequenceNo =  2   ,IsActive=  true}
 //,new metadataTransactionType{Identifier="53",Name="SR Deficiency Credit",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="130",Name="Storage Capacity – Seasonal Firm",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="6",Name="Storage Injection",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="113",Name="Storage Injection – Firm",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="114",Name="Storage Injection – Interruptible",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="131",Name="Storage Inventory",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="41",Name="Storage Inventory Cycling",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="11",Name="Storage Inventory Transfer",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="7",Name="Storage Withdrawal",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="115",Name="Storage Withdrawal – Firm",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="116",Name="Storage Withdrawal – Interruptible",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="18",Name="Suspense Gas Claim",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="61",Name="Take",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="62",Name="Take Payback",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="73",Name="Trade Injection",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="74",Name="Trade Withdrawal",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="75",Name="Transfer Injection",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="76",Name="Transfer Withdrawal",SequenceNo =  3   ,IsActive=  true}
 //,new metadataTransactionType{Identifier="52",Name="TSP Deficiency Credit",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="77",Name="Unauthorized Injection Overrun",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="50",Name="Unauthorized Overrun",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="82",Name="Unauthorized Transfer",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="78",Name="Unauthorized Withdrawal Overrun",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="79",Name="Un-nominated Injection",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="80",Name="Un-nominated Withdrawal",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="99",Name="Volumetric",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="96",Name="Volumetric / Firm Storage",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="97",Name="Volumetric / Interruptible Storage",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="132",Name="Wheeling",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="1",Name="Withdrawl",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="83",Name="Write Off",SequenceNo =  20  ,IsActive=  true}
 //,new metadataTransactionType{Identifier="1",Name="Current Business (Transport)",SequenceNo =  1   ,IsActive=  true}



 //           };

        }
    }
}
