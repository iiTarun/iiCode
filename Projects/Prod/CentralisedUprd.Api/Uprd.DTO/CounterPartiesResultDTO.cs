using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralisedUprd.Api.UPRD.DTO
{
    public class CounterPartiesResultDTO
    {
        public List<CounterPartiesDTO> CounterParties { get; set; }
        public int RecordCount { get; set; }
    }
}