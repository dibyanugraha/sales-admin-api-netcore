using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAdmin.Data
{
    public interface ISalesLineRepository
    {
        int CreateSalesLine(SalesLine line);
        void UpdateSalesLine(SalesLine line);
        void DeleteSalesLine(string no, int lineNo);
        SalesLine GetSalesLine(string no, int lineNo);
        IEnumerable<SalesLine> GetSalesLines(SalesLine filters);

    }
}
