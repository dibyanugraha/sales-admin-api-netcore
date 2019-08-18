using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAdmin.Data
{
    public interface ISalesHeaderRepository
    {
        int CreateSalesHeader(SalesHeader salesHeader);
        bool UpdateSalesHeader(SalesHeader salesHeader);
        int DeleteSalesHeader(string no);
        SalesHeader GetSalesHeader(string no);
        IEnumerable<SalesHeader> GetSalesHeaders(
            int? page = 1, 
            int? pageSize = 10,
            SalesHeader filters = null);
        Task<int> CreateSalesHeaderAsync(SalesHeader salesHeader);
        Task<bool> UpdateSalesHeaderAsync(SalesHeader salesHeader);
        Task<int> DeleteSalesHeaderAsync(string no);
        Task<SalesHeader> GetSalesHeaderAsync(string no);
        Task<IEnumerable<SalesHeader>> GetSalesHeadersAsync(
            int? page = 1,
            int? pageSize = 10,
            SalesHeader filters = null);

    }
}
