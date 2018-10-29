using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Core.BusinessLogic.CommandHandlers;
using Core.BusinessLogic.CommandRequests;
using Core.BusinessLogic.Notifications;
using Core.Database;
using Core.Database.Abstract;
using Core.Database.DbExecutors;
using Core.Models.Settings;
using DbConn.DbExecutor.Abstract;
using DbConn.DbExecutor.Dapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Taxi.Api.Service.Extensions;

namespace Taxi.Api.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        private IHostingEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Почему-то так не работает:
            //services.AddRouting(options => options.LowercaseUrls = true);

            services.AddLogging();
            services.Configure<AppSettings>(Configuration);

            services.AddTransient<IDbExecutorFactory, DapperDbExecutorFactory>();
            services.AddTransient<IDbContextFactory<OrderContext>, OrdersDbContextFactory>();
            services.AddTransient<INotifier, SmsNotifier>();
            services.AddTransient(provider =>
            {
                var appSettingsOptions = provider.GetRequiredService<IOptions<AppSettings>>();
                return appSettingsOptions.Value;
            });

            services.AddMediatR(typeof(MakeTaxiOrderCommandRequest).GetTypeInfo().Assembly);

            services.AddMvc(options =>
            {
                options.Filters.Add<ModelValidatorActionFilter>();
                options.Filters.Add<ErrorExceptionFilter>();
            });

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "Taxi API", Version = "v1"}); });

            services.AddMediatR(typeof(MakeTaxiOrderCommandRequest));

            services.AddDbContext<OrderContext>(options => options.UseSqlite($"Data Source={_env.ContentRootPath}/data.db"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Taxi API v1");
                c.RoutePrefix = "";
            });
            app.UseMvc();
        }
    }
}
