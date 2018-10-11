using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.DTO
{
  public class SwntResultDTO
    {
        public List<SwntPerTransactionDTO> swntPerTransactionDTO { get; set; }
        public int RecordCount { get; set; }
    }
}
