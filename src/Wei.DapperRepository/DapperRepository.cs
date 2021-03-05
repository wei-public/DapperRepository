using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Wei.DapperRepository
{
    public class DapperRepository<TEntity>
        : BaseRepository<TEntity>, IRepository<TEntity>
        where TEntity : class
    {
        public DapperRepository(IEnumerable<IDbFactory> dbFactories)
        {
            DbFactory = dbFactories.First();
            base.GetConnection();
        }

        public override IDbFactory DbFactory { get; }
    }

    public class DapperRepository<TEntity, TDbFactory>
        : BaseRepository<TEntity>, IRepository<TEntity, TDbFactory>
        where TEntity : class
        where TDbFactory : class, IDbFactory
    {
        public DapperRepository(IEnumerable<IDbFactory> dbFactories)
        {
            DbFactory = dbFactories.Single(x => x.GetType().Name == typeof(TDbFactory).Name);
            base.GetConnection();
        }

        public override IDbFactory DbFactory { get; }
    }
}
