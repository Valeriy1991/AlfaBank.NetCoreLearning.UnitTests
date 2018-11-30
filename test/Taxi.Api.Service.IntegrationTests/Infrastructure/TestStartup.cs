using System;
using System.Reflection;
using Core.BusinessLogic.CommandRequests;
using Core.BusinessLogic.Notifications;
using Core.BusinessLogic.WebServices;
using Core.BusinessLogic.WebServices.UrlBuilders;
using Core.Database;
using Core.Database.Abstract;
using Core.Database.DbExecutors;
using Core.Models.Settings;
using DbConn.DbExecutor.Abstract;
using DbConn.DbExecutor.Dapper.Sqlite;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Taxi.Api.Service.Extensions;

namespace Taxi.Api.Service.IntegrationTests.Infrastructure
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration, IHostingEnvironment env)
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
            services.AddTransient(provider =>
            {
                var appSettingsOptions = provider.GetRequiredService<IOptions<AppSettings>>();
                var appSettings = appSettingsOptions.Value;
                if (appSettings.ConnectionStrings == null)
                {
                    throw new ArgumentNullException(
                        $"The \"{nameof(appSettings.ConnectionStrings)}\" is empty in app settings file");
                }

                return appSettings.ConnectionStrings;
            });
            services.AddTransient(provider =>
            {
                var appSettingsOptions = provider.GetRequiredService<IOptions<AppSettings>>();
                var appSettings = appSettingsOptions.Value;
                if (appSettings.Notification == null)
                {
                    throw new ArgumentNullException(
                        $"The \"{nameof(appSettings.Notification)}\" is empty in app settings file");
                }

                return appSettings.Notification;
            });
            services.AddTransient(provider =>
            {
                var appSettingsOptions = provider.GetRequiredService<IOptions<AppSettings>>();
                var appSettings = appSettingsOptions.Value;
                if (appSettings.WebServices == null)
                {
                    throw new ArgumentNullException(
                        $"The \"{nameof(appSettings.WebServices)}\" is empty in app settings file");
                }

                return appSettings.WebServices;
            });
            ;
            services.AddTransient(provider =>
            {
                var webServicesSettings = provider.GetRequiredService<WebServicesSettings>();
                if (webServicesSettings.Rest == null)
                {
                    throw new ArgumentNullException(
                        $"The \"{nameof(webServicesSettings.Rest)}\" is empty in app settings file");
                }

                return webServicesSettings.Rest;
            });

            services.AddMediatR(typeof(MakeOrderCommandRequest).GetTypeInfo().Assembly);
            services.AddTransient<DriverRestService>();
            services.AddTransient<DriverRestServiceUrlBuilder>();

            services.AddMvc(options =>
            {
                options.Filters.Add<ModelValidatorActionFilter>();
                options.Filters.Add<ErrorExceptionFilter>();
            });

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "Taxi API", Version = "v1" }); });
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