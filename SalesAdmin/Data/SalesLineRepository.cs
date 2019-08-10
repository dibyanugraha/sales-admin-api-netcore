namespace SalesAdmin.Data
{
    using Dapper;
    using Dapper.Contrib.Extensions;
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
            using (var conn = GetOpenConnection())
            {
                var id = conn.ExecuteScalar<int>(@"
                    insert into 
                        SalesLine(id, documentNo, lineNo, type, no, description, quantity, unitPrice)
                        values(@Id, @DocumentNo, @LineNo, @Type, @No, @Description, @Quantity, @UnitPrice);

                    select LAST_INSERT_ID()", line);

                line.Id = id;

                return id;
            }
        }

        public void DeleteSalesLine(string documentNo, int lineNo)
        {
            using (var conn = GetOpenConnection())
            {
                conn.Execute(
                    "delete from SalesLine " +
                    "   where documentNo=@DocumentNo " +
                    "       and lineNo=@LineNo",
                    new {
                        documentNo,
                        lineNo
                    });
            }
        }

        public SalesLine GetSalesLine(string documentNo, int lineNo)
        {

            using (var conn = GetOpenConnection())
            {
                return conn.QueryFirstOrDefault<SalesLine>(
                    "select from SalesLine where documentNo = @DocumentNo", 
                    new { documentNo, lineNo });
            }
        }

        public void UpdateSalesLine(SalesLine line)
        {
            using (var conn = GetOpenConnection())
            {
                SalesLine updatedLine = conn.Get<SalesLine>(line.Id);
                updatedLine.DocumentNo = line.DocumentNo;
                updatedLine.LineNo = line.LineNo;
                updatedLine.Type = line.Type;
                updatedLine.No = line.No;
                updatedLine.Description = line.Description;
                updatedLine.Quantity = line.Quantity;
                updatedLine.UnitPrice = line.UnitPrice;
                conn.Update(updatedLine);
            }
        }

        public IEnumerable<SalesLine> GetSalesLines(SalesLine filters)
        {
            if (filters == null)
                return null;

            using (var conn = GetOpenConnection())
            {
                SqlBuilder builder = new SqlBuilder();
                string query = "";

                query = @"select * from SalesLine /**where**/";

                var selector = builder.AddTemplate(query);

                if (!string.IsNullOrEmpty(filters.DocumentNo))
                    builder.Where("DocumentNo = @DocumentNo", new { filters.DocumentNo });

                builder.Where("LineNo = @LineNo", new { a = filters.LineNo.GetValueOrDefault() });

                builder.Where("Type = @Type", new { a = filters.Type.GetValueOrDefault() });

                if (!string.IsNullOrEmpty(filters.No))
                    builder.Where("No = @No", new { filters.No });

                if (!string.IsNullOrEmpty(filters.Description))
                    builder.Where("Description = @Description", new { filters.Description });

                builder.Where("Quantity = @Quantity", new { a = filters.Quantity.GetValueOrDefault() });
                builder.Where("UnitPrice = @UnitPrice", new { a = filters.UnitPrice.GetValueOrDefault() });

                return conn.Query<SalesLine>(selector.RawSql, selector.Parameters).ToArray();
            }
        }
    }
}
