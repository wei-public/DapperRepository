using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Wei.DapperRepository;

namespace Wei.Demo.WebApi.DbFactories
{
    public class SQLiteDbFactory : DbFactory
    {
        private readonly IDbConnection _connection;
        public SQLiteDbFactory(IConfiguration configuration)
        {
            _connection = new SQLiteConnection(configuration.GetConnectionString("SQLite"));
        }

        public override IDbConnection GetConnection() => _connection;

    }
}
