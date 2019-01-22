using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Core.BusinessLogic.CommandHandlers;
using Core.BusinessLogic.CommandRequests;
using Core.Models;
using Core.Models.Settings;
using Core.Models.Settings.Fakes;
using DbConn.DbExecutor.Abstract;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Core.BusinessLogic.Tests.CommandHandlers
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit")]
    public class SetDriverForOrderCommandHandlerTests
    {
        private readonly Faker _faker = new Faker();

        private SetDriverForOrderCommandHandler CreateTestedComponent(
            AppSettings appSettings,
            IDbExecutorFactory dbExecutorFactory)
        {
            var logger = Substitute.For<ILogger<SetDriverForOrderCommandHandler>>();
            return new SetDriverForOrderCommandHandler(logger, appSettings, dbExecutorFactory);
        }

        [Fact]
        public async Task Handle__DriverNotFoundInStorage__ReturnFailureWithValidErrorMessage()
        {
            // Arrange
            var orderId = _faker.Random.Int(min: 0);
            var driverId = _faker.Random.Int(min: 0);
            var appSettings = AppSettingsFake.Generate();

            #region IDbExecutor
            
            var dbExecutor = Substitute.For<IDbExecutor>();
            // 1 способ:
            dbExecutor.Query<Driver>(Arg.Any<string>()).Returns(new List<Driver>());
            // 2 способ:
//            var getDriverSql = $@"
//select * 
//from Drivers
//where Id = {driverId};
//";
//            dbExecutor.Query<Driver>(getDriverSql).Returns(new List<Driver>());

            #endregion

            #region IDbExecutorFactory

            var dbExecutorFactory = Substitute.For<IDbExecutorFactory>();
            dbExecutorFactory.Create(appSettings.ConnectionStrings.OrdersDb).Returns(dbExecutor);

            #endregion

            var handler = CreateTestedComponent(appSettings, dbExecutorFactory);
            var request = new SetDriverForOrderCommandRequest()
            {
                OrderId = orderId,
                DriverId = driverId
            };
            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            // Assert
            Assert.True(result.Failure);
            Assert.Contains("Назначаемый водитель не найден", result.ToMultiLine());
        }

        [Fact]
        public async Task Handle__DriverExistInStorage__ReturnSuccess()
        {
            // Arrange
            var orderId = _faker.Random.Int(min: 0);
            var driverId = _faker.Random.Int(min: 0);
            var driver = new Driver()
            {
                Id = driverId,
                FullName = _faker.Person.FullName,
                Phone = _faker.Phone.PhoneNumber("+7 (9##) ###-##-##")
            };
            var appSettings = AppSettingsFake.Generate();

            #region IDbExecutor

            var dbExecutor = Substitute.For<IDbExecutor>();
            dbExecutor.Query<Driver>(Arg.Any<string>()).Returns(new List<Driver>() {driver});
            // NOTE: здесь надо быть осторожным, потому что если у нас будет несколько вызовов Execute внутри
            // тестируемого компонента, то т.к. стоит Arg.Any<T>, то будут переопределены все вызовы
            dbExecutor.Execute(Arg.Any<string>());

            #endregion

            #region IDbExecutorFactory

            var dbExecutorFactory = Substitute.For<IDbExecutorFactory>();
            dbExecutorFactory.Create(appSettings.ConnectionStrings.OrdersDb).Returns(dbExecutor);

            #endregion

            var handler = CreateTestedComponent(appSettings, dbExecutorFactory);
            var request = new SetDriverForOrderCommandRequest()
            {
                OrderId = orderId,
                DriverId = driverId
            };
            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            // Assert
            Assert.True(result.Success);
        }
    }
}