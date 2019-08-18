namespace SalesAdmin.Models.SalesHeader
{
    using System;
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
