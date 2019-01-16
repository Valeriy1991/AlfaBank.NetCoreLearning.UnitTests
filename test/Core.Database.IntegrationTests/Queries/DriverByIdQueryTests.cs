using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
    public class DriverByIdQueryTests : IntegrationTest, IDisposable
    {
        private readonly Faker _faker = new Faker();

        private DriverByIdQuery CreateTestedComponent(IDbExecutor dbExecutor)
        {
            return new DriverByIdQuery(dbExecutor);
        }

        private readonly DriverTestRepository _driverTestRepository = new DriverTestRepository();
        private readonly IDbExecutor _dbExecutor;
        private readonly DriverByIdQuery _query;

        public DriverByIdQueryTests()
        {
            // Т.к. мы только читаем данные из БД, то не используем транзакционность
            _dbExecutor = CreateDbExecutor();
            _query = CreateTestedComponent(_dbExecutor);
        }

        [Fact]
        public void Get__DriverDoesNotExistInStorage__ReturnNull()
        {
            // Arrange
            var notExistsId = _faker.Random.Int(max: -1);
            // Act
            var driver = _query.Get(notExistsId);
            // Assert
            Assert.Null(driver);
        }

        [Fact]
        public void Get__DriverExistsInStorage__ReturnThisDriver_V1()
        {
            // Arrange
            var driver = DriverFake.Generate();
            var insertDriverSql = $@"
insert into Drivers(FullName, Phone)
values (""{driver.FullName}"", ""{driver.Phone}"");

select last_insert_rowid();
";
            var driverId = _dbExecutor.Query<int>(insertDriverSql).First();
            // Act
            var foundDriver = _query.Get(driverId);
            // Assert
            Assert.NotNull(foundDriver);
            Assert.Equal(driver.Phone, foundDriver.Phone);
            Assert.Equal(driver.FullName, foundDriver.FullName);
        }

        [Fact]
        public void Get__DriverExistsInStorage__ReturnThisDriver_V2()
        {
            // Arrange
            var driver = DriverFake.Generate();
            var driverId = _driverTestRepository.Create(_dbExecutor, driver);
            // Act
            var foundDriver = _query.Get(driverId);
            // Assert
            Assert.NotNull(foundDriver);
            Assert.Equal(driver.Phone, foundDriver.Phone);
            Assert.Equal(driver.FullName, foundDriver.FullName);
        }

        public void Dispose()
        {
            _dbExecutor?.Dispose();
        }
    }
}