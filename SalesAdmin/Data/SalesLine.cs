﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAdmin.Data
{
    public class SalesLine
    {
        public int Id { get; set; }
        public string DocumentNo { get; set; }
        public int LineNo { get; set; }
        public SalesLine_Type Type { get; set; }
        public string No { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }

    }
}
