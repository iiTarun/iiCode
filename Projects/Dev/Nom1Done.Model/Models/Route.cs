using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Model
{
    public class Route
    {
        [Key]
        public int ID { get; set; }
        public string EDIRouteId { get; set; }
        public string RouteDescription { get; set; }
        public string DirectionId { get; set; }
        public string DirectionDescription { get; set; }

    }

}
