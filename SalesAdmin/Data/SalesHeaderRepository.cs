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

    public class SalesHeaderRepository : ISalesHeaderRepository
    {
        private readonly string _connectionString;

        public SalesHeaderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public int CreateSalesHeader(SalesHeader salesHeader)
        {
            using (var conn = GetOpenConnection())
            {
                var id = conn.ExecuteScalar<int>(@"
                    insert into salesHeader(id, no, description)
                    values(@id, @no, @description);

                    select LAST_INSERT_ID()", salesHeader);

                salesHeader.Id = id;

                return id;
            }
        }

        public void DeleteSalesHeader(string no)
        {
            using (var conn = GetOpenConnection())
            {
                conn.Execute(
                    "delete from SalesHeader where No=@No",
                    new { no });
            }
        }

        public SalesHeader GetSalesHeader(string no)
        {
            using (var conn = GetOpenConnection())
            {
                return conn.QueryFirstOrDefault<SalesHeader>
                    ("select from salesHeader where No = @No", new { no });
            }
        }

        public IEnumerable<SalesHeader> GetSalesHeaders(
            int? page = 1,
            int? pageSize = 10,
            SalesHeader filters = null)
        {
            using (var conn = GetOpenConnection())
            {
                if ((filters == null)
                    && (page.Value == 1)
                    && (pageSize.Value == 10))
                    return conn.Query<SalesHeader>(
                        "select * from SalesHeader limit 1, 10").ToArray();

                SqlBuilder builder = new SqlBuilder();
                string query = "";

                if ((page.HasValue) && (page.Value > 1)
                    && (pageSize.HasValue))
                    query = $"select * from SalesHeader limit { page. Value }, { pageSize.Value } /**where**/";

                var selector = builder.AddTemplate(query);

                if (!string.IsNullOrEmpty(filters.No))
                    builder.Where("No = @No", new { filters.No });

                if (!string.IsNullOrEmpty(filters.Description))
                    builder.Where("Description = @Description", new { filters.Description });

                return conn.Query<SalesHeader>(selector.RawSql, selector.Parameters).ToArray();
            }
        }

        public void UpdateSalesHeader(SalesHeader salesHeader)
        {
            using (var conn = GetOpenConnection())
            {
                SalesHeader updatedHeader = conn.Get<SalesHeader>(salesHeader.Id);
                updatedHeader.Description = salesHeader.Description;
                conn.Update(updatedHeader);
            }
        }

        private DbConnection GetOpenConnection()
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}
