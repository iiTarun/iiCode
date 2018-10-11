using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralisedUprd.Api.UPRD.DTO
{
    public class UnscResultDTO
    {
        public List<UnscPerTransactionDTO> unscPerTransactionDTO { get; set; } = new List<UnscPerTransactionDTO>();
        public int RecordCount { get; set; }
    }
}