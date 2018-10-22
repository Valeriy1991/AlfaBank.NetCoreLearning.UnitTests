using CQRSlight.Abstract;
using Ether.Outcomes;
using Microsoft.EntityFrameworkCore;

namespace Core.Database.Abstract
{
    public abstract class DbContextCommand<TDbContext, TCommandContext> : ICommand<TCommandContext>
        where TDbContext : DbContext
    {
        protected TDbContext DbContext { get; }

        protected DbContextCommand(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public abstract IOutcome Execute(TCommandContext commandContext);
    }

    public abstract class DbContextCommand<TDbContext, TCommandContext, TResult> : ICommand<TCommandContext, TResult>
        where TDbContext : DbContext
    {
        protected TDbContext DbContext { get; }

        protected DbContextCommand(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public abstract IOutcome<TResult> Execute(TCommandContext commandContext);
    }
}