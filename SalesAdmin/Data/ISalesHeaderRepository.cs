using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAdmin.Data
{
    interface ISalesHeaderRepository
    {
        int CreateSalesHeader(SalesHeader salesHeader);
        void UpdateSalesHeader(SalesHeader salesHeader);
        void DeleteSalesHeader(int id);
        SalesHeader GetSalesHeader(int id);
        IEnumerable<SalesHeader> GetSalesHeaders(SalesHeader filters);

    }
}
