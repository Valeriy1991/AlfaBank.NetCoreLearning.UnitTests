using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Core.BusinessLogic.CommandHandlers;
using Core.BusinessLogic.CommandRequests;
using Core.Database;
using Core.Database.Abstract;
using Core.Models;
using Core.Models.Enums;
using Core.Models.Settings;
using Core.Models.Settings.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Core.BusinessLogic.Tests.CommandHandlers
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit")]
    public class FinishOrderCommandHandlerV2Tests
    {
        private readonly Faker _faker = new Faker();

        private FinishOrderCommandHandlerV2 CreateTestedComponent(
            AppSettings appSettings,
            IDbContextFactory<OrderContext> dbContextFactory)
        {
            var logger = Substitute.For<ILogger<FinishOrderCommandHandlerV2>>();
            return new FinishOrderCommandHandlerV2(logger, appSettings, dbContextFactory);
        }

        [Fact]
        public async Task Handle__ReturnSuccess()
        {
            // Arrange
            var orderId = _faker.Random.Int(min: 0);
            var order = new Order() { Id = orderId, FinishDateTime = null, Status = "" };
            var orders = new List<Order>()
            {
                order
            };
            var ordersAsQueryable = orders.AsQueryable();
            var dbContext = Substitute.For<OrderContext>();
            var appSettings = AppSettingsFake.Generate();

            #region DbContext

            var ordersDbSet = Substitute.For<DbSet<Order>, IQueryable<Order>>();
            ((IQueryable<Order>) ordersDbSet).Provider.Returns(ordersAsQueryable.Provider);
            ((IQueryable<Order>) ordersDbSet).Expression.Returns(ordersAsQueryable.Expression);
            ((IQueryable<Order>) ordersDbSet).ElementType.Returns(ordersAsQueryable.ElementType);
            ((IQueryable<Order>) ordersDbSet).GetEnumerator().Returns(ordersAsQueryable.GetEnumerator());

            dbContext.Orders.Returns(ordersDbSet);
            dbContext.SaveChanges().Returns(1);

            #endregion

            #region DbContextFactory

            var dbContextFactory = Substitute.For<IDbContextFactory<OrderContext>>();
            dbContextFactory.Create(appSettings.ConnectionStrings.OrdersDb).Returns(dbContext);

            #endregion

            var handler = CreateTestedComponent(appSettings, dbContextFactory);
            var commandRequest = new FinishOrderCommandRequestV2()
            {
                OrderId = orderId
            };
            // Act
            var result = await handler.Handle(commandRequest, CancellationToken.None);
            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task Handle__OrderHasValidFinishDateTimeAndStatusEqualsFinish()
        {
            // Arrange
            var orderId = _faker.Random.Int(min: 0);
            var finishDateTime = _faker.Date.Recent();
            var order = new Order() {Id = orderId, FinishDateTime = null, Status = ""};
            var orders = new List<Order>()
            {
                order
            };
            var ordersAsQueryable = orders.AsQueryable();
            var dbContext = Substitute.For<OrderContext>();
            var appSettings = AppSettingsFake.Generate();

            #region DbContext

            var ordersDbSet = Substitute.For<DbSet<Order>, IQueryable<Order>>();
            ((IQueryable<Order>) ordersDbSet).Provider.Returns(ordersAsQueryable.Provider);
            ((IQueryable<Order>) ordersDbSet).Expression.Returns(ordersAsQueryable.Expression);
            ((IQueryable<Order>) ordersDbSet).ElementType.Returns(ordersAsQueryable.ElementType);
            ((IQueryable<Order>) ordersDbSet).GetEnumerator().Returns(ordersAsQueryable.GetEnumerator());

            dbContext.Orders.Returns(ordersDbSet);
            dbContext.SaveChanges().Returns(1);

            #endregion

            #region DbContextFactory

            var dbContextFactory = Substitute.For<IDbContextFactory<OrderContext>>();
            dbContextFactory.Create(appSettings.ConnectionStrings.OrdersDb).Returns(dbContext);

            #endregion

            var handler = CreateTestedComponent(appSettings, dbContextFactory);
            var commandRequest = new FinishOrderCommandRequestV2()
            {
                OrderId = orderId,
                FinishDateTime = finishDateTime
            };
            // Act
            await handler.Handle(commandRequest, CancellationToken.None);
            // Assert
            Assert.Equal(finishDateTime, order.FinishDateTime);
            Assert.Equal(StatusEnum.Finished, order.Status);
        }
    }
}