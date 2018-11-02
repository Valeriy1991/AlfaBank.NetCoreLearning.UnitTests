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
using Core.Database.Commands;
using Core.Models.ApiModels.Fakes;
using Core.Models.Settings.Fakes;
using Ether.Outcomes;
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

        private MakeOrderCommandHandler CreateTestedComponent(
            AppSettings appSettings,
            IDbContextFactory<OrderContext> dbContextFactory,
            INotifier notifier)
        {
            var logger = Substitute.For<ILogger<MakeOrderCommandHandler>>();
            return new MakeOrderCommandHandler(logger, appSettings, dbContextFactory, notifier);
        }

        private MakeOrderCommandRequest GenerateCommandRequest()
        {
            var myCustomPhone = "123-456-789";
            // 1 способ:
            //var apiModel = MakeOrderModelFake.Generate();
            //apiModel.Phone = myCustomPhone;
            // 2 способ - Fluent API:
            var apiModel = MakeOrderModelFake.Generate().WithPhone(myCustomPhone);
            return MakeOrderCommandRequest.Create(apiModel);
        }

        [Fact]
        public async Task Handle__CreateNewOrderIsFail__ReturnFailure_v1()
        {
            // Arrange
            var errorMessage = $"test-error: {_faker.Random.Words()}";
            var notifier = Substitute.For<INotifier>();
            var dbContext = Substitute.For<OrderContext>();
            var appSettings = AppSettingsFake.Generate();

            #region DbContext
            
            // Минусы: 
            // 1) приходится вникать в логику сохранения нового заказа;
            // 2) приходится ее воспроизводить, да так, чтобы не возникло ошибок:
            //      - если мы не зададим, DbSet<Orders> - получим NRE
            //      - если не зададим поведение SaveChanges() - он выполнится успешно, и мы получим не тот
            //        результат, который ожидаем
            
            var ordersDbSet = Substitute.For<DbSet<Order>>();
            dbContext.Orders = ordersDbSet;
            dbContext.SaveChanges().Throws(new Exception(errorMessage));
            
            #endregion

            #region DbContextFactory

            var dbContextFactory = Substitute.For<IDbContextFactory<OrderContext>>();
            dbContextFactory.Create(appSettings.ConnectionStrings.OrdersDb).Returns(dbContext);

            #endregion
            
            var handler = CreateTestedComponent(appSettings, dbContextFactory, notifier);
            
            var commandRequest = GenerateCommandRequest();
            // Act
            var result = await handler.Handle(commandRequest, CancellationToken.None);
            // Assert
            Assert.True(result.Failure);
            Assert.Contains(errorMessage, result.ToMultiLine());
        }


        [Fact]
        public async Task Handle__CreateNewOrderIsFail__ReturnFailure_v2()
        {
            // Arrange
            var errorMessage = $"test-error: {_faker.Random.Words()}";
            var notifier = Substitute.For<INotifier>();
            var dbContext = Substitute.For<OrderContext>();
            var appSettings = AppSettingsFake.Generate();
            
            #region DbContextFactory

            var dbContextFactory = Substitute.For<IDbContextFactory<OrderContext>>();
            dbContextFactory.Create(appSettings.ConnectionStrings.OrdersDb).Returns(dbContext);

            #endregion
            
            var handler = CreateTestedComponent(appSettings, dbContextFactory, notifier);

            #region CreateNewOrderCommand

            // Минус: немало кода (минус весьма сомнительный, потому что это в нашем примере добавление заказа в БД простое)
            // Плюс:
            //      1) изоляция от логики добавления нового заказа в БД
            //      2) соответственно, отсутствует необходимость дублирования этой логики

            // ВАЖНО: нас не интересует содержимое, нас интересует РЕЗУЛЬТАТ команды создания нового заказа, 
            // т.к. от этого результата зависит поведение бизнес-компонента (если fail - выйти; если success - двигаемся дальше)

            var createNewOrderCommand = Substitute.For<CreateNewOrderCommand>(dbContext);
            createNewOrderCommand.Execute(Arg.Any<CreateNewOrderCommand.Context>())
                // Задаем нужный нам результат
                .Returns(Outcomes.Failure<Order>().WithMessage(errorMessage));
            var createNewOrderCommandFactory = Substitute.For<CreateNewOrderCommand.Factory>();
            createNewOrderCommandFactory.Create(dbContext).Returns(createNewOrderCommand);
            handler.SetCreateNewOrderCommandFactory(createNewOrderCommandFactory);

            #endregion

            var commandRequest = GenerateCommandRequest();
            // Act
            var result = await handler.Handle(commandRequest, CancellationToken.None);
            // Assert
            Assert.True(result.Failure);
            Assert.Contains(errorMessage, result.ToMultiLine());
        }

        [Fact]
        public async Task Handle__CreateNewOrderIsSuccess__CallSendMethodOfINotifierAtOnce()
        {
            // Arrange
            var notifier = Substitute.For<INotifier>();
            var dbContext = Substitute.For<OrderContext>();
            var appSettings = AppSettingsFake.Generate();
            var order = new Order();
            var commandRequest = GenerateCommandRequest();

            #region DbContextFactory

            var dbContextFactory = Substitute.For<IDbContextFactory<OrderContext>>();
            dbContextFactory.Create(appSettings.ConnectionStrings.OrdersDb).Returns(dbContext);

            #endregion

            var handler = CreateTestedComponent(appSettings, dbContextFactory, notifier);

            #region CreateNewOrderCommand
            
            var createNewOrderCommand = Substitute.For<CreateNewOrderCommand>(dbContext);
            createNewOrderCommand.Execute(Arg.Any<CreateNewOrderCommand.Context>()).Returns(Outcomes.Success(order));
            var createNewOrderCommandFactory = Substitute.For<CreateNewOrderCommand.Factory>();
            createNewOrderCommandFactory.Create(dbContext).Returns(createNewOrderCommand);
            handler.SetCreateNewOrderCommandFactory(createNewOrderCommandFactory);

            #endregion

            // Act
            await handler.Handle(commandRequest, CancellationToken.None);
            // Assert
            notifier.Received(1).Send(Arg.Any<Notification>());
        }
        [Fact]
        public async Task Handle__CreateNewOrderIsFail__SendMethodOfINotifierWasNotCalled()
        {
            // Arrange
            var notifier = Substitute.For<INotifier>();
            var dbContext = Substitute.For<OrderContext>();
            var appSettings = AppSettingsFake.Generate();
            var commandRequest = GenerateCommandRequest();

            #region DbContextFactory

            var dbContextFactory = Substitute.For<IDbContextFactory<OrderContext>>();
            dbContextFactory.Create(appSettings.ConnectionStrings.OrdersDb).Returns(dbContext);

            #endregion

            var handler = CreateTestedComponent(appSettings, dbContextFactory, notifier);

            #region CreateNewOrderCommand

            var createNewOrderCommand = Substitute.For<CreateNewOrderCommand>(dbContext);
            createNewOrderCommand.Execute(Arg.Any<CreateNewOrderCommand.Context>())
                .Returns(Outcomes.Failure<Order>().WithMessage("test-error"));
            var createNewOrderCommandFactory = Substitute.For<CreateNewOrderCommand.Factory>();
            createNewOrderCommandFactory.Create(dbContext).Returns(createNewOrderCommand);
            handler.SetCreateNewOrderCommandFactory(createNewOrderCommandFactory);

            #endregion

            // Act
            await handler.Handle(commandRequest, CancellationToken.None);
            // Assert
            notifier.Received(0).Send(Arg.Any<Notification>());
        }
    }
}