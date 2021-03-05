using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Wei.DapperRepository;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtention
    {
        /// <summary>
        /// 注册DapperRepository
        /// </summary>
        /// <typeparam name="TDbFactory">IDbFactory实现类型</typeparam>
        /// <param name="services">ServiceCollection</param>
        /// <param name="optionAction">配置回调</param>
        /// <returns></returns>
        public static IServiceCollection AddDapperRepository<TDbFactory>(this IServiceCollection services, Action<RepositoryOption> optionAction = null)
            where TDbFactory : class, IDbFactory
        {
            services.AddTransient<IDbFactory, TDbFactory>();
            var option = new RepositoryOption();
            optionAction?.Invoke(option);
            if (option.ServiceActions != null) option.ServiceActions.ForEach(x => x.Invoke(services));
            services.AddTransient(typeof(IRepository<>), typeof(DapperRepository<>));
            services.AddTransient(typeof(IRepository<,>), typeof(DapperRepository<,>));
            services.AddRepository();
            return services;
        }

        private static void AddRepository(this IServiceCollection services)
        {
            var assemblys = GetCurrentPathAssembly();
            foreach (var assembly in assemblys)
            {
                AddRepository(services, assembly, typeof(DapperRepository<>));
                AddRepository(services, assembly, typeof(DapperRepository<,>));
            }
        }

        private static void AddRepository(IServiceCollection services, Assembly assembly, Type baseType)
        {
            var serviceTypes = assembly.GetTypes()
                                  .Where(x => x.IsClass
                                          && !x.IsAbstract
                                          && x.HasImplementedRawGeneric(baseType));
            foreach (var type in serviceTypes)
            {
                var interfaces = type.GetInterfaces();
                var interfaceType = interfaces.FirstOrDefault(x => x.Name == $"I{type.Name}");
                if (interfaceType == null) interfaceType = type;
                var serviceDescriptor = new ServiceDescriptor(interfaceType, type, ServiceLifetime.Transient);
                if (!services.Contains(serviceDescriptor)) services.Add(serviceDescriptor);
            }
        }

        private static List<Assembly> GetCurrentPathAssembly()
        {
            var dlls = DependencyContext.Default.CompileLibraries
                .Where(x => !x.Name.StartsWith("Microsoft") && !x.Name.StartsWith("System"))
                .ToList();

            var list = new List<Assembly>();
            if (dlls.Any())
            {
                foreach (var dll in dlls)
                {
                    if (dll.Type == "project")
                        list.Add(Assembly.Load(dll.Name));
                }
            }
            return list;
        }

        private static bool HasImplementedRawGeneric(this Type type, Type generic)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (generic == null) throw new ArgumentNullException(nameof(generic));
            var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
            if (isTheRawGenericType) return true;
            while (type != null && type != typeof(object))
            {
                isTheRawGenericType = IsTheRawGenericType(type);
                if (isTheRawGenericType) return true;
                type = type.BaseType;
            }
            return false;

            bool IsTheRawGenericType(Type t) => generic == (t.IsGenericType ? t.GetGenericTypeDefinition() : t);
        }
    }
}
