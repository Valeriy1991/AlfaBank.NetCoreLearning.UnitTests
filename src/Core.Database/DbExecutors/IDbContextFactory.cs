using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Core.Database.DbExecutors
{
    public interface IDbContextFactory<out TDbContext>
    where TDbContext : DbContext
    {
        TDbContext Create(string connectionString);
    }
}