using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatchlistMailManagement.UPRD.DTO
{
    public class OacyResultDTO
    {
        public List<OACYPerTransactionDTO> oacyPerTransactionDTO { get; set; } = new List<OACYPerTransactionDTO>();
        public int RecordCount { get; set; }
    }
}