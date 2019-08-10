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
        IEnumerable<SalesHeader> GetSalesHeaders(SalesHeader filters);

    }
}
