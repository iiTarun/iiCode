using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nom1Done.DTO
{
    public class CommonDTO
    {
    }


    public class NomStatusDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class CycleIndicatorDTO
    {
        public int CycleID { get; set; }
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<CycleIndicatorDTO> Cycles { get; set; }
    }

    public class CapacityIndicatorDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<CapacityIndicatorDTO> Cycles { get; set; }
    }
    public class BidUpIndicatorDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<BidUpIndicatorDTO> Cycles { get; set; }
    }
    public class QuantityIndicatorDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<QuantityIndicatorDTO> Cycles { get; set; }
    }
    public class ExportDeclarationDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<ExportDeclarationDTO> Cycles { get; set; }
    }
}