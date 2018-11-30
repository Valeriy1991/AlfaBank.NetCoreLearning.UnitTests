using System.Diagnostics.CodeAnalysis;
using Core.Database.DbExecutors;
using Core.Models.Settings;

namespace Core.Database.IntegrationTests.Abstract
{
    [ExcludeFromCodeCoverage]
    public abstract class IntegrationTest
    {
        public AppSettings AppSettings { get; set; }

        protected IntegrationTest()
        {
            AppSettings = new AppSettings()
            {
                ConnectionStrings = new ConnectionStringSettings()
                {
                    OrdersDb = @"Data Source=D:\Projects\Learn\NetCore\orders.db"
                }
            };
        }

        protected OrderContext CreateDbContext()
        {
            var orderDbContextFactory = new OrdersDbContextFactory();
            return orderDbContextFactory.Create(AppSettings.ConnectionStrings.OrdersDb);
        }
        protected OrderContext CreateTransactionalDbContext()
        {
            var orderDbContextFactory = new OrdersDbContextFactory();
            var dbContext = orderDbContextFactory.Create(AppSettings.ConnectionStrings.OrdersDb);
            dbContext.Database.BeginTransaction();
            return dbContext;
        }
    }
}