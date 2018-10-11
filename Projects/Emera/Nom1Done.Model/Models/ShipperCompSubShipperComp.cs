using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Model
{
    public class ShipperCompSubShipperComp
    {
        [Key]
        public int Id { get; set; }
        public string ShipperCompDuns { get; set; }
        public string SubShipperCompDuns { get; set; }
    }
}
