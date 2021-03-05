using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Wei.DapperRepository;

namespace Wei.Demo.WebApi.DbFactories
{
    public class MySqlDbFactory : DbFactory
    {
        private readonly List<IDbConnection> _connections = new List<IDbConnection>();
        public MySqlDbFactory(IConfiguration configuration)
        {
            _connections.Add(new MySqlConnection(configuration.GetConnectionString("MySql.Master")));
            _connections.Add(new MySqlConnection(configuration.GetConnectionString("MySql.Slave")));
        }

        public override IDbConnection GetConnection()
        {
            return _connections[new Random().Next(_connections.Count)];
        }

    }
}
