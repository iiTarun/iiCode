﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatchlistMailManagement.UPRD.DTO
{
    public class SwntResultDTO
    {
        public List<SwntPerTransactionDTO> swntPerTransactionDTO { get; set; } = new List<SwntPerTransactionDTO>();
        public int RecordCount { get; set; }
    }
}