using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wei.DapperExtension;

namespace Wei.DapperRepository
{
    public abstract class BaseRepository<TEntity> : IDisposable where TEntity : class
    {

        private IDbConnection _connection;
        private IDbTransaction _transaction;
        public abstract IDbFactory DbFactory { get; }
        public IDbConnection GetConnection() => _connection = DbFactory.GetConnection();
        public void BeginTrans(IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted)
        {
            if (_connection != null && _connection.State != ConnectionState.Open)
                _connection.Open();

            _transaction = _connection.BeginTransaction(isolationLevel);
        }
        public void Rollback() => _transaction?.Rollback();
        public void Commit() => _transaction?.Commit();

        public void Dispose()
        {
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
        public TEntity Insert(TEntity entity) => _connection.Insert(entity, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public int Insert(IEnumerable<TEntity> entities) => _connection.Insert(entities, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public bool Update(TEntity entity) => _connection.Update(entity, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore) > 0;

        public bool Update(Expression<Func<TEntity, bool>> predicate, Action<TEntity> updateAction) => _connection.Update(predicate, updateAction, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore) > 0;

        public bool Delete(object id) => _connection.Delete<TEntity>(id, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore) > 0;

        public bool Delete(TEntity entity) => _connection.Delete(entity, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore) > 0;

        public bool Delete(Expression<Func<TEntity, bool>> predicate) => _connection.Delete(predicate, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore) > 0;

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null) => _connection.FirstOrDefault(predicate, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public TEntity Get(object id) => _connection.Get<TEntity>(id, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null, string orderBySql = null) => _connection.GetAll(predicate, orderBySql, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public Tuple<long, IEnumerable<TEntity>> GetPage(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate = null, string orderBySql = null) => _connection.GetPage(pageIndex, pageSize, predicate, orderBySql, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);


        #endregion

        #region CURD Async
        public Task<int> InsertAsync(IEnumerable<TEntity> entities) => _connection.InsertAsync(entities, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public Task<TEntity> InsertAsync(TEntity entity) => _connection.InsertAsync(entity, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public async Task<bool> UpdateAsync(TEntity entity) => (await _connection.UpdateAsync(entity, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore)) > 0;

        public async Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Action<TEntity> action) => (await _connection.UpdateAsync(predicate, action, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore)) > 0;

        public async Task<bool> DeleteAsync(object id) => (await _connection.DeleteAsync<TEntity>(id, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore)) > 0;

        public async Task<bool> DeleteAsync(TEntity entity) => (await _connection.DeleteAsync(entity, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore)) > 0;

        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate) => (await _connection.DeleteAsync(predicate, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore)) > 0;

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null) => _connection.FirstOrDefaultAsync(predicate, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public Task<TEntity> GetAsync(object id) => _connection.GetAsync<TEntity>(id, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, string orderBySql = null) => _connection.GetAllAsync(predicate, orderBySql, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);

        public Task<Tuple<long, IEnumerable<TEntity>>> GetPageAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate = null, string orderBySql = null) => _connection.GetPageAsync(pageIndex, pageSize, predicate, orderBySql, _transaction, DbFactory.TimeOut, DbFactory.SqlExecuteBefore);


        #endregion
    }
}
