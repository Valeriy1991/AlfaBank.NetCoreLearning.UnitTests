using Microsoft.EntityFrameworkCore;

namespace Core.Database.Abstract
{
    public interface IDbContextFactory<out TDbContext>
    where TDbContext : DbContext
    {
        TDbContext Create(string connectionString);
    }
}