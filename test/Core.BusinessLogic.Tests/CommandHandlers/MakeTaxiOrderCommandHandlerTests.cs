using Core.BusinessLogic.CommandHandlers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Core.Models.Settings;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;
using Core.Database.Abstract;
using Core.Database;
using Microsoft.Extensions.Logging;
using Core.BusinessLogic.Notifications;
using System.Threading.Tasks;
using Core.BusinessLogic.CommandRequests;
using Core.Models;
using System.Collections.Generic;
using Bogus;
using Core.Models.ApiModels.Fakes;
using Core.Models.Settings.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NSubstitute.ExceptionExtensions;

namespace Core.BusinessLogic.Tests.CommandHandlers
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit")]
    public class MakeTaxiOrderCommandHandlerTests
    {
        private readonly Faker _faker = new Faker();

        private MakeTaxiOrderCommandHandler CreateTestedComponent(
            AppSettings appSettings,
            IDbContextFactory<OrderContext> dbContextFactory,
            INotifier notifier)
        {
            var logger = Substitute.For<ILogger<MakeTaxiOrderCommandHandler>>();
            return new MakeTaxiOrderCommandHandler(logger, appSettings, dbContextFactory, notifier);
        }

        private MakeTaxiOrderCommandRequest GenerateCommandRequest()
        {
            var myCustomPhone = "123-456-789";
            // 1 способ:
            //var apiModel = MakeOrderModelFake.Generate();
            //apiModel.Phone = myCustomPhone;
            // 2 способ - Fluent API:
            var apiModel = MakeOrderModelFake.Generate().WithPhone(myCustomPhone);
            return MakeTaxiOrderCommandRequest.Create(apiModel);
        }

        [Fact]
        public async Task Handle__CreateNewOrderIsFail__ReturnFailure()
        {
            // Arrange
            var errorMessage = $"test-error: {_faker.Random.Words()}";
            var appSettings = AppSettingsFake.Generate();

            #region DbContext

            var ordersDbSet = Substitute.For<DbSet<Order>>();
            var dbContext = Substitute.For<OrderContext>();
            dbContext.Orders = ordersDbSet;
            dbContext.SaveChanges().Throws(new Exception(errorMessage));

            #endregion

            #region DbContextFactory

            var dbContextFactory = Substitute.For<IDbContextFactory<OrderContext>>();
            dbContextFactory.Create(appSettings.ConnectionStrings.OrdersDb).Returns(dbContext);

            #endregion

            var notifier = Substitute.For<INotifier>();

            var handler = CreateTestedComponent(appSettings, dbContextFactory, notifier);

            var commandRequest = GenerateCommandRequest();
            // Act
            var result = await handler.Handle(commandRequest, CancellationToken.None);
            // Assert
            Assert.True(result.Failure);
            Assert.Contains(errorMessage, result.ToMultiLine());
        }
    }
}