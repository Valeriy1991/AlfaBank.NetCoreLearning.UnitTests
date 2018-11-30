using Core.Database;
using Core.Database.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Taxi.Api.Service.IntegrationTests.Infrastructure
{
    public class OrdersDbContextTestFactory : IDbContextFactory<OrderContext>
    {
        public OrderContext Create(string connectionString)
        {
            //var optionsBuilder = new DbContextOptionsBuilder<OrderContext>();
            //return new OrderContext(optionsBuilder.Options);
            return new OrderContext();
        }
    }
}