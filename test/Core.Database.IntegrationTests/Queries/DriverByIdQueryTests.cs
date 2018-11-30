using System;
using System.Diagnostics.CodeAnalysis;
using Bogus;
using Core.Database.IntegrationTests.Abstract;
using Core.Database.Queries;
using Core.Database.TestsRepository;
using Core.Models.Fake;
using Dapper;
using DbConn.DbExecutor.Abstract;
using Xunit;

namespace Core.Database.IntegrationTests.Queries
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Integration - Database")]
    public class DriverByIdQueryTests : IntegrationTest
    {
        private readonly Faker _faker = new Faker();
        private readonly DriverTestRepository _driverTestRepository = new DriverTestRepository();

        private DriverByIdQuery CreateTestedComponent(IDbExecutor dbExecutor)
        {
            return new DriverByIdQuery(dbExecutor);
        }

        [Fact]
        public void Get__DriverDoesNotExistInStorage__ReturnNull()
        {
            using (var dbExecutor = CreateDbExecutor())
            {
                // Arrange
                var notExistsId = _faker.Random.Int(max: -1);
                var query = CreateTestedComponent(dbExecutor);
                // Act
                var driver = query.Get(notExistsId);
                // Assert
                Assert.Null(driver);
            }
        }

        [Fact]
        public void Get__DriverExistsInStorage__ReturnThisDriver_V1()
        {
            using (var dbExecutor = CreateDbExecutor())
            {
                // Arrange
                var driver = DriverFake.Generate();
                var insertDriverSql = $@"
insert into Drivers(FullName, Phone)
values ('{driver.FullName}', '{driver.Phone}');

select last_insert_rowid();
";
                var driverId = dbExecutor.InnerConnection.Execute(insertDriverSql);
                var query = CreateTestedComponent(dbExecutor);
                // Act
                var foundDriver = query.Get(driverId);
                // Assert
                Assert.NotNull(foundDriver);
                Assert.Equal(driver.Phone, foundDriver.Phone);
                Assert.Equal(driver.FullName, foundDriver.FullName);
            }
        }

        [Fact]
        public void Get__DriverExistsInStorage__ReturnThisDriver_V2()
        {
            using (var dbExecutor = CreateDbExecutor())
            {
                // Arrange
                var driver = DriverFake.Generate();
                var driverId = _driverTestRepository.Create(dbExecutor, driver);
                var query = CreateTestedComponent(dbExecutor);
                // Act
                var foundDriver = query.Get(driverId);
                // Assert
                Assert.NotNull(foundDriver);
                Assert.Equal(driver.Phone, foundDriver.Phone);
                Assert.Equal(driver.FullName, foundDriver.FullName);
            }
        }
    }
}