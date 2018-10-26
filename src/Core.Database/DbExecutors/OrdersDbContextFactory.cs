using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Core.Database.DbExecutors
{
    public class OrdersDbContextFactory : IDbContextFactory<OrderContext>
    {
        public OrderContext Create(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrderContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new OrderContext(optionsBuilder.Options);
        }
    }
}