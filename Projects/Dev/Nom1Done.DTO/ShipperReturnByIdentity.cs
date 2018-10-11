using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.DTO
{
    public class ShipperReturnByIdentity
    {
        public string CompanyId { get; set; }
        public string ShipperDuns { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ShipperName { get; set; }     
        public int FirstSelectedPipeIdByUser { get; set; }
    }
}
