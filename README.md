# Wei.DapperRepository
> nuget package **Wei.DapperRepository**

基于netstandard2.0平台对[Wei.DapperExtension](https://github.com/wei-public/DapperExtension) 组件封装Repository


|数据库| 是否支持 |
| ------------| --------------- |
| SqlServer | ✔ |
| MySql     | ✔ |
| SqlLite   | ✔ |
| 其他 ...  | 待支持 |

## 快速入门

***
1. **安装Nuget包**
> dotnet add package Wei.DapperExtension

2.  **自定义SqlServerDbFactory类，继承DbFactory基类**
```C#
public class SqlServerDbFactory : DbFactory
{
    private readonly IDbConnection _connection;
    public SqlServerDbFactory(IConfiguration configuration)
    {
        _connection = new SqlConnection(configuration.GetConnectionString("SqlServer"));
    }

    public override IDbConnection GetConnection() => _connection;
}
```


3.  **StartUp.ConfigureServices注入DapperRepository服务**
```C#
 public void ConfigureServices(IServiceCollection services)
{
    ...

    services.AddDapperRepository<SqlServerDbFactory>();

    ...
}
```



4. **在Controller中使用**
```C#
public class TestModelController : ControllerBase
{
    //泛型Repository
    private readonly IRepository<TestModelInt> _repository;

    public TestModelController(IRepository<TestModelInt> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public TestModelInt FirstOrDefault()
    {
        var result = _repository.FirstOrDefault();
        return result;
    }
}
```


***

## 通用CURD方法
> 所有接口分为同步和异步，下面只列出异步接口
```C#
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
```
***
## 多数据库支持
1. **ConfigureServices中配置 AddDbFactory<T>(),添加其他dbFactory**
```C#
 public void ConfigureServices(IServiceCollection services)
{
    services.AddDapperRepository<SqlServerDbFactory>(option =>
    {
        //多数据库支持
        option.AddDbFactory<MySqlDbFactory>();
    });
}
```
2. **使用Repository时指定DbFactory**
```C#
    //泛型Repository 指定DbFactory
    private readonly IRepository<TestModelInt, MySqlDbFactory> _repository;

    public TestModelController(IRepository<TestModelInt, MySqlDbFactory> repository)
    {
        _repository = repository;
    }
```
***

## 自定义Repository
1. **定义ITestModelRepository接口和TestModelRepository实现类**
```C#
public interface ITestModelRepository : IRepository<TestModelInt>
{
    Task<TestModelInt> UpdateResultAsync(int id, string result);
}

public class TestModelRepository : DapperRepository<TestModelInt>, ITestModelRepository
{
    public TestModelRepository(IEnumerable<IDbFactory> dbFactories) : base(dbFactories)
    {

    }
    
    public async Task<TestModelInt> UpdateResultAsync(int id, string result)
    {
        var isSuccess = await UpdateAsync(x => x.Id == id, x => x.Result = result);
        if (!isSuccess) return default;
        return await GetAsync(id);
    }

}
```
2. **使用自定义Repository**
```C#
    //自定义Repository
    private readonly ITestModelRepository _repository;

    public TestModelController(ITestModelRepository repository)
    {
        _repository = repository;
    }

    [HttpPut("custom-reposotry")]
    public Task<TestModelInt> UpdateResultAsync(int id, string result)
    {
        return _repository.UpdateResultAsync(id, result);
    }
```
***
## 事务Transaction
```C#
public async Task<int> InsertWithTransaction(bool isThrowException)
{
    try
    {
        BeginTrans();
        var entity = await InsertAsync(new TestModelInt { MethodName = "Transaction" });

        if (isThrowException)
            throw new Exception();

        Commit();
        return entity.Id;
    }
    catch (Exception)
    {
        Rollback();
        return 0;
    }
}
```
***
## 执行Dapper原生方法，执行SQL
>using Dapper;
```C#
_repository.GetConnection().Execute("sqlCmd");

```

