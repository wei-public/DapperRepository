using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Wei.DapperRepository;

namespace Wei.Demo.WebApi.DbFactories
{
    public class SqlServerDbFactory : DbFactory
    {
        private readonly IDbConnection _connection;
        public SqlServerDbFactory(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("SqlServer"));
        }

        public override IDbConnection GetConnection() => _connection;

        /// <summary>
        /// sql执行之前回调函数
        /// </summary>
        /// <param name="sql"sql语句</param>
        /// <param name="parameter">参数</param>
        public override void SqlExecuteBefore(string sql, object parameter)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(sql);
            Console.ResetColor();
        }

    }
}
