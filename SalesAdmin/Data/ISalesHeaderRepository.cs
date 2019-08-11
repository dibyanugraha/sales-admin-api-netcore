using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAdmin.Data
{
    public interface ISalesHeaderRepository
    {
        int CreateSalesHeader(SalesHeader salesHeader);
        void UpdateSalesHeader(SalesHeader salesHeader);
        void DeleteSalesHeader(string no);
        SalesHeader GetSalesHeader(string no);
        IEnumerable<SalesHeader> GetSalesHeaders(
            int? page = 1, 
            int? pageSize = 10,
            SalesHeader filters = null);
        Task<int> CreateSalesHeaderAsync(SalesHeader salesHeader);
        void UpdateSalesHeaderAsync(SalesHeader salesHeader);
        void DeleteSalesHeaderAsync(string no);
        Task<SalesHeader> GetSalesHeaderAsync(string no);
        Task<IEnumerable<SalesHeader>> GetSalesHeadersAsync(
            int? page = 1,
            int? pageSize = 10,
            SalesHeader filters = null);

    }
}
