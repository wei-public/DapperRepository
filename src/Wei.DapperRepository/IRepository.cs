using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Wei.DapperRepository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        IDbConnection GetConnection();

        /// <summary>
        /// 开始事务
        /// </summary>
        void BeginTrans();

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <param name="il"></param>
        void BeginTrans(IsolationLevel il);

        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();

        #region CURD
        TEntity Insert(TEntity entity);

        int Insert(IEnumerable<TEntity> entities);

        bool Update(TEntity entity);

        bool Update(Expression<Func<TEntity, bool>> predicate, Action<TEntity> updateAction);

        bool Delete(object id);

        bool Delete(TEntity entity);

        bool Delete(Expression<Func<TEntity, bool>> predicate);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null);

        TEntity Get(object id);

        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null, string orderBySql = null);

        Tuple<long, IEnumerable<TEntity>> GetPage(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate = null, string orderBySql = null);


        #endregion

        #region CURD Async
        /// <summary>
        /// 新增(批量)
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>受影响的行数</returns>
        Task<int> InsertAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>新增后的实体</returns>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体(主键必须赋值)</param>
        /// <returns>是否成功</returns>
        Task<bool> UpdateAsync(TEntity entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="predicate">更新条件(不能为空)</param>
        /// <param name="action">更新指定字段回调</param>
        /// <returns>是否成功</returns>
        Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Action<TEntity> action);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns>是否成功</returns>
        Task<bool> DeleteAsync(object id);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体(主键必须赋值)</param>
        /// <returns>是否成功</returns>
        Task<bool> DeleteAsync(TEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="predicate">删除条件(不能为空)</param>
        /// <returns>是否成功</returns>
        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 获取第一个或者默认值
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>实体</returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"><主键Id/param>
        /// <returns>实体</returns>
        Task<TEntity> GetAsync(object id);

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="orderBySql">排序sql  eg:order by id desc</param>
        /// <returns>实体集合</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, string orderBySql = null);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="orderBySql">排序sql(默认为主键降序排列)</param>
        /// <returns>总条数,分页数据</returns>
        Task<Tuple<long, IEnumerable<TEntity>>> GetPageAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate = null, string orderBySql = null);


        #endregion
    }

    public interface IRepository<TEntity, TDbFactory> : IRepository<TEntity>
       where TEntity : class
       where TDbFactory : IDbFactory
    {

    }
}
