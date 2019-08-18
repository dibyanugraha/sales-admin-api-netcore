namespace SalesAdmin.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ISalesLineRepository
    {
        int CreateSalesLine(SalesLine line);
        void UpdateSalesLine(SalesLine line);
        void DeleteSalesLine(string documentNo, int lineNo);
        SalesLine GetSalesLine(string documentNo, int lineNo);
        IEnumerable<SalesLine> GetSalesLines(SalesLine filters);

        Task<int> CreateSalesLineAsync(SalesLine line);
        void UpdateSalesLineAsync(SalesLine line);
        void DeleteSalesLineAsync(string documentNo, int lineNo);
        Task<SalesLine> GetSalesLineAsync(string documentNo, int lineNo);
        Task<IEnumerable<SalesLine>> GetSalesLinesAsync(SalesLine filters);
    }
}
