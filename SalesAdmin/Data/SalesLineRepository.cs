namespace SalesAdmin.Data
{
    using MySql.Data.MySqlClient;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;

    public class SalesLineRepository : ISalesLineRepository
    {
        private readonly string _connectionString;

        public SalesLineRepository(string repo)
        {
            _connectionString = repo;
        }
        private DbConnection GetOpenConnection()
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
        public int CreateSalesLine(SalesLine line)
        {
            throw new NotImplementedException();
        }

        public void DeleteSalesLine(string no, int lineNo)
        {
            throw new NotImplementedException();
        }

        public SalesLine GetSalesLine(string no, int lineNo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SalesLine> GetSalesLines(SalesLine filters)
        {
            throw new NotImplementedException();
        }

        public void UpdateSalesLine(SalesLine line)
        {
            throw new NotImplementedException();
        }
    }
}
