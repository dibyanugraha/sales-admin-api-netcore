namespace SalesAdmin.Models.SalesHeader
{
    using System.Collections.Generic;
    public class SalesHeaderListResponse
    {
        public IEnumerable<SalesHeaderResponse> SalesHeaders { get; set; }
    }
}
