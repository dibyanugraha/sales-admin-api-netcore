namespace SalesAdmin.Data.Dapper
{
    using global::Dapper.Contrib.Extensions;
    using global::Dapper;
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
        public async Task<int> CreateSalesHeaderAsync(SalesHeader salesHeader)
        {
            using (var conn = GetOpenConnection())
            {
                var id = await conn.ExecuteScalarAsync<int>(@"
                    insert into salesHeader(no, description, CreatedDateTime, LastModifiedDateTime, CreatedByUserId)
                    values(@No, @Description, @CreatedDateTime, @LastModifiedDateTime, @CreatedByUserId);

                    select LAST_INSERT_ID()", salesHeader);

                salesHeader.Id = id;

                return id;
            }
        }

        public int DeleteSalesHeader(string no)
        {
            using (var conn = GetOpenConnection())
            {
                return conn.Execute(
                    "delete from SalesHeader where No=@No",
                    new { no });
            }
        }

        public async Task<int> DeleteSalesHeaderAsync(string no)
        {
            using (var conn = GetOpenConnection())
            {
                return await conn.ExecuteAsync(
                    "delete from SalesHeader where No=@No",
                    new { no });
            }
        }

        public SalesHeader GetSalesHeader(string no)
        {
            using (var conn = GetOpenConnection())
            {
                var result = conn.Query<SalesHeader>
                    ("select * from salesHeader where No = @No", new { no });

                return result.FirstOrDefault();
            }
        }

        public async Task<SalesHeader> GetSalesHeaderAsync(string no)
        {
            using (var conn = GetOpenConnection())
            {
                var result = await conn.QueryAsync<SalesHeader>
                    ($"select * from salesHeader where No = '{ no }' limit 1");

                return result.FirstOrDefault();
            }
        }

        public IEnumerable<SalesHeader> GetSalesHeaders(int? page = 1, int? pageSize = 10, SalesHeader filters = null)
        {
            using (var conn = GetOpenConnection())
            {
                if ((filters == null)
                    && (page == null))
                {
                    var queryResult = conn.Query<SalesHeader>(
                        "select * from SalesHeader limit 1, 10");

                    return queryResult.ToArray();
                }

                SqlBuilder builder = new SqlBuilder();
                string query = "";

                if ((page.HasValue)
                    && (pageSize.HasValue))
                    query = $"select * from SalesHeader limit { page.Value }, { pageSize.Value } /**where**/";

                var selector = builder.AddTemplate(query);

                if (!string.IsNullOrEmpty(filters.No))
                    builder.Where("No = @No", new { filters.No });

                if (!string.IsNullOrEmpty(filters.Description))
                    builder.Where("Description = @Description", new { filters.Description });

                var result = conn.Query<SalesHeader>(selector.RawSql, selector.Parameters);

                return result.ToArray();
            }
        }

        public async Task<IEnumerable<SalesHeader>> GetSalesHeadersAsync(
            int? page = 1,
            int? pageSize = 10,
            SalesHeader filters = null)
        {
            using (var conn = GetOpenConnection())
            {
                if ((filters == null)
                    && (page == null))
                {
                    var queryResult = await conn.QueryAsync<SalesHeader>(
                        "select * from SalesHeader");

                    return queryResult.ToArray();
                }

                SqlBuilder builder = new SqlBuilder();
                string query = "";

                if ((page.HasValue)
                    && (pageSize.HasValue))
                    query = $"select * from SalesHeader limit { page.Value }, { pageSize.Value } /**where**/";

                var selector = builder.AddTemplate(query);

                if (!string.IsNullOrEmpty(filters.No))
                    builder.Where("No = @No", new { filters.No });

                if (!string.IsNullOrEmpty(filters.Description))
                    builder.Where("Description = @Description", new { filters.Description });

                var result = await conn.QueryAsync<SalesHeader>(selector.RawSql, selector.Parameters);

                return result.ToArray();
            }
        }

        public bool UpdateSalesHeader(SalesHeader salesHeader)
        {
            using (var conn = GetOpenConnection())
            {
                SalesHeader updatedHeader = conn.Get<SalesHeader>(salesHeader.Id);
                updatedHeader.Description = salesHeader.Description;
                return conn.Update(updatedHeader);
            }
        }

        public async Task<bool> UpdateSalesHeaderAsync(SalesHeader salesHeader)
        {
            using (var conn = GetOpenConnection())
            {
                SalesHeader updatedHeader = conn.Get<SalesHeader>(salesHeader.Id);
                updatedHeader.Description = salesHeader.Description;
                return await conn.UpdateAsync(updatedHeader);
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
