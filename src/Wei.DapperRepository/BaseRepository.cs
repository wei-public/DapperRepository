using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Wei.DapperExtension;
using Wei.DapperExtension.Attributes;
using Wei.DapperExtension.Utils;

namespace Wei.DapperRepository
{
    public abstract class BaseRepository<TEntity> : IDisposable where TEntity : class
    {
        private IDbConnection _connection;

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                if (!string.IsNullOrEmpty(_tableName))
                    CacheUtil.SetTableName<TEntity>(_tableName);
                return _connection;
            }
        }

        private IDbTransaction _transaction;

        public abstract IDbFactory DbFactory { get; }
        private string _tableName;

        protected void SetTableName(string tableName) => _tableName = tableName;
        public IDbConnection GetConnection()
        {
            _connection = DbFactory.GetConnection();
            return Connection;
        }
        public void BeginTrans()
        {
            if (_connection != null && _connection.State != ConnectionState.Open)
                _connection.Open();

            _transaction = _connection.BeginTransaction();
        }

        public void BeginTrans(IsolationLevel il)
        {
            if (_connection != null && _connection.State != ConnectionState.Open)
                _connection.Open();

            _transaction = _connection.BeginTransaction(il);
        }

        public void Rollback() => _transaction?.Rollback();
        public void Commit() => _transaction?.Commit();

        public void Dispose()
        {
            if (!string.IsNullOrEmpty(_tableName))
            {
                _tableName = string.Empty;
                CacheUtil.ReSetTableName<TEntity>();
            }
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }

        #region CURD
        public TEntity Insert(TEntity entity) => Connection.Insert(entity, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public int Insert(IEnumerable<TEntity> entities) => Connection.Insert(entities, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public bool Update(TEntity entity) => Connection.Update(entity, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore) > 0;

        public bool Update(Expression<Func<TEntity, bool>> predicate, Action<TEntity> updateAction) => Connection.Update(predicate, updateAction, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore) > 0;

        public bool Delete(object id) => Connection.Delete<TEntity>(id, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore) > 0;

        public bool Delete(TEntity entity) => Connection.Delete(entity, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore) > 0;

        public bool Delete(Expression<Func<TEntity, bool>> predicate) => Connection.Delete(predicate, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore) > 0;

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null) => Connection.FirstOrDefault(predicate, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public TEntity Get(object id) => Connection.Get<TEntity>(id, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null, string orderBySql = null) => Connection.GetAll(predicate, orderBySql, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public Tuple<long, IEnumerable<TEntity>> GetPage(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate = null, string orderBySql = null) => Connection.GetPage(pageIndex, pageSize, predicate, orderBySql, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);


        #endregion

        #region CURD Async
        public Task<int> InsertAsync(IEnumerable<TEntity> entities) => Connection.InsertAsync(entities, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public Task<TEntity> InsertAsync(TEntity entity) => Connection.InsertAsync(entity, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public async Task<bool> UpdateAsync(TEntity entity) => (await Connection.UpdateAsync(entity, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore)) > 0;

        public async Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Action<TEntity> action) => (await Connection.UpdateAsync(predicate, action, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore)) > 0;

        public async Task<bool> DeleteAsync(object id) => (await Connection.DeleteAsync<TEntity>(id, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore)) > 0;

        public async Task<bool> DeleteAsync(TEntity entity) => (await Connection.DeleteAsync(entity, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore)) > 0;

        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate) => (await Connection.DeleteAsync(predicate, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore)) > 0;

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null) => Connection.FirstOrDefaultAsync(predicate, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public Task<TEntity> GetAsync(object id) => Connection.GetAsync<TEntity>(id, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, string orderBySql = null) => Connection.GetAllAsync(predicate, orderBySql, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public Task<Tuple<long, IEnumerable<TEntity>>> GetPageAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate = null, string orderBySql = null) => Connection.GetPageAsync(pageIndex, pageSize, predicate, orderBySql, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);


        #endregion
    }
}
