using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.DTO
{
    public class UnscResultDTO
    {
        public List<UnscPerTransactionDTO> unscPerTransactionDTO { get; set; }
        public int RecordCount { get; set; }
    }
}
