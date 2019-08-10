using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAdmin.Models
{
    public class SalesHeaderResponse
    {
        public int Id { get; set; }
        public string No { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int CreatedByUserId { get; set; }
    }
}
