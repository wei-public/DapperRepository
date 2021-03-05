using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Wei.DapperRepository
{
    public interface IDbFactory
    {
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        /// <returns></returns>
        IDbConnection GetConnection();

        /// <summary>
        /// 超时时间
        /// </summary>
        int? TimeOut { get; }

        /// <summary>
        /// sql执行之前回调
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameter">参数</param>
        void SqlExecuteBefore(string sql, object parameter);
    }

    public abstract class DbFactory : IDbFactory
    {
        /// <summary>
        /// 超时时间
        /// </summary>
        public virtual int? TimeOut { get; }

        public abstract IDbConnection GetConnection();

        /// <summary>
        /// Sql执行之前回调
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        public virtual void SqlExecuteBefore(string sql, object parameter)
        {

        }
    }
}
