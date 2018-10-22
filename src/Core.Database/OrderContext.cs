using System;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Database
{
    public class OrderContext : DbContext
    {
        public OrderContext()
        {
            
        }
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<DriverOrder> DriverOrders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data source=orders.db");
        }
    }
}
