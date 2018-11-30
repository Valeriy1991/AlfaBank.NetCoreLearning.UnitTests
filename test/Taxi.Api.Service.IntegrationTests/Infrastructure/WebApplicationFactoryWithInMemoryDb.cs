using System;
using Core.Database;
using Core.Database.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Taxi.Api.Service.IntegrationTests.Infrastructure
{
    // Code from https://github.com/aspnet/Docs/blob/master/aspnetcore/test/integration-tests/samples/2.x/IntegrationTestsSample/tests/RazorPagesProject.Tests/CustomWebApplicationFactory.cs
    public class WebApplicationFactoryWithInMemoryDb<TStartup> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Add a database context (ApplicationDbContext) using an in-memory 
                // database for testing.
                services.AddDbContext<OrderContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                services.AddTransient<IDbContextFactory<OrderContext>, OrdersDbContextTestFactory>();

                // Build the service provider.
                var anotherServiceProvider = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (OrderContext).
                using (var scope = anotherServiceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var orderContext = scopedServices.GetRequiredService<OrderContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<WebApplicationFactoryWithInMemoryDb<TStartup>>>();
                    
                    // Ensure the database is created.
                    orderContext.Database.EnsureCreated();
                    // Apply all migrations
                    //orderContext.Database.Migrate();

                    try
                    {
                        // Seed the database with test data.
                        Utilities.InitializeDbForTests(orderContext);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex,
                            $"An error occurred seeding the database with test messages. Error: {ex.Message}");
                    }
                }
            });
        }
    }
}