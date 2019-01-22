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

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<DriverOrder> DriverOrders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
