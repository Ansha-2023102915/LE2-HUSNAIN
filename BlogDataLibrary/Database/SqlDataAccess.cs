using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDataLibrary.Database
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;
        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<T>> LoadData<T, U>(string sql, U parameters, string connectionStringName)
        {
            string connStr = _config.GetConnectionString(connectionStringName);
            using (IDbConnection conn = new SqlConnection(connStr))
            {
                var result = await conn.QueryAsync<T>(sql, parameters);
                return result;
            }
        }

        public async Task<int> SaveData<T>(string sql, T parameters, string connectionStringName)
        {
            string connStr = _config.GetConnectionString(connectionStringName);
            using (IDbConnection conn = new SqlConnection(connStr))
            {
                var rows = await conn.ExecuteAsync(sql, parameters);
                return rows;
            }
        }
    }
}
