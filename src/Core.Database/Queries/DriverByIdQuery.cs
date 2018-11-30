using System.Linq;
using Core.Models;
using CQRSlight.Db.Abstract;
using DbConn.DbExecutor.Abstract;

namespace Core.Database.Queries
{
    public class DriverByIdQuery : DbQuery<int, Driver>
    {
        public class Factory
        {
            public virtual DriverByIdQuery Create(IDbExecutor dbExecutor)
            {
                return new DriverByIdQuery(dbExecutor);
            }
        }

        public DriverByIdQuery(IDbExecutor dbExecutor) : base(dbExecutor)
        {
        }

        public override Driver Get(int driverId)
        {
            var sql = $@"
select * 
from Drivers
where Id = {driverId};
";
            return DbExecutor.Query<Driver>(sql).FirstOrDefault();
        }
    }
}