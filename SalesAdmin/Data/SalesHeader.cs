using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAdmin.Data
{
    public class SalesHeader
    {
        public int Id { get; set; }
        public string No { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public int CreatedByUserId { get; set; }

    }
}
