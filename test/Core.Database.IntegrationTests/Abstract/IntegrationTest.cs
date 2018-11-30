using System.Diagnostics.CodeAnalysis;
using Core.Database.DbExecutors;
using Core.Models.Settings;
using DbConn.DbExecutor.Abstract;
using DbConn.DbExecutor.Dapper.Sqlite;

namespace Core.Database.IntegrationTests.Abstract
{
    [ExcludeFromCodeCoverage]
    public abstract class IntegrationTest
    {
        public AppSettings AppSettings { get; set; }

        private readonly OrdersDbContextFactory _ordersDbContextFactory = new OrdersDbContextFactory();
        private readonly IDbExecutorFactory _dbExecutorFactory = new DapperDbExecutorFactory();

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
            return _ordersDbContextFactory.Create(AppSettings.ConnectionStrings.OrdersDb);
        }
        protected OrderContext CreateTransactionalDbContext()
        {
            var dbContext = _ordersDbContextFactory.Create(AppSettings.ConnectionStrings.OrdersDb);
            dbContext.Database.BeginTransaction();
            return dbContext;
        }

        protected IDbExecutor CreateDbExecutor()
        {
            return _dbExecutorFactory.Create(AppSettings.ConnectionStrings.OrdersDb);
        }
        protected IDbExecutor CreateTransactionalDbExecutor()
        {
            return _dbExecutorFactory.CreateTransactional(AppSettings.ConnectionStrings.OrdersDb);
        }
    }
}