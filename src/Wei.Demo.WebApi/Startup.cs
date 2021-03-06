using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Data.SQLite;
using System.IO;
using Wei.DapperRepository;
using Wei.Demo.WebApi.DbFactories;
using Dapper;
using System;

namespace Wei.Demo.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDapperRepository<SQLiteDbFactory>(option =>
            {
                //多数据库支持
                //option.AddDbFactory<MySqlDbFactory>();
                //option.AddDbFactory<SqlServerDbFactory>();
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wei.Demo.WebApi", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wei.Demo.WebApi v1"));
            }

            app.UseRouting();

            app.UseAuthorization();
            EnsureCreateTable(app);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        private void EnsureCreateTable(IApplicationBuilder app)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "demo.sqlite");
            if (!File.Exists(path))
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetService<IRepository<TestModelInt>>();

                    SQLiteConnection.CreateFile(path);
                    var sql = @"
                            CREATE TABLE 'TestModelInt' (
                                'Id' integer NOT NULL,
                                'MethodName' TEXT,
                                'Result' TEXT,
                                PRIMARY KEY('Id')
                            );
                            CREATE TABLE 'TestModelMultipeKey'(
                              'TypeId' TEXT NOT NULL,
                              'Type' TEXT NOT NULL,
                              'MethodName' TEXT,
                              'Result' TEXT,
                              PRIMARY KEY('TypeId', 'Type')
                            );";
                    repository.GetConnection().Execute(sql);
                }
            }
        }
    }
}
