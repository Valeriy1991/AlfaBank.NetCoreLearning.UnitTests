﻿using System;
using System.Linq;
using Core.Models;
using Dapper;
using DbConn.DbExecutor.Abstract;

namespace Core.Database.TestsRepository
{
    public class DriverTestRepository
    {
        public int Create(IDbExecutor dbExecutor, Driver driver)
        {
            var insertDriverSql = $@"
insert into Drivers(FullName, Phone)
values (""{driver.FullName}"", ""{driver.Phone}"");

select last_insert_rowid();
";
            var driverId = dbExecutor.Query<int>(insertDriverSql).First();
            return driverId;
        }
    }
}
