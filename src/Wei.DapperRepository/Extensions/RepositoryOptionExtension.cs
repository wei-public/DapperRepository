using Microsoft.Extensions.DependencyInjection;

namespace Wei.DapperRepository
{
    public static class RepositoryOptionExtension
    {
        /// <summary>
        /// 添加数据库工厂
        /// </summary>
        /// <typeparam name="TDbFactory">IDbFactory的实现类型</typeparam>
        /// <param name="option">配置</param>
        public static void AddDbFactory<TDbFactory>(this RepositoryOption option)
            where TDbFactory : class, IDbFactory => option.AddService(x => x.AddTransient<IDbFactory, TDbFactory>());
    }
}
