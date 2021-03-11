using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Wei.DapperExtension.Utils;

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

        /// <summary>
        /// 设置数据库表名称(对于表名称动态生成的，可以动态设置表名称)
        /// <param name="tableName">表名称</param>
        /// </summary>
        public new DapperRepository<TEntity> SetTableName(string tableName)
        {
            base.SetTableName(tableName);
            return this;
        }
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

        /// <summary>
        /// 设置数据库表名称(对于表名称动态生成的，可以动态设置表名称)
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public new DapperRepository<TEntity, TDbFactory> SetTableName(string tableName)
        {
            base.SetTableName(tableName);
            return this;
        }
    }
}
