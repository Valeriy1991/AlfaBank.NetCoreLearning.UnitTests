using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Bogus;
using Core.Database.Commands;
using Core.Database.DbExecutors;
using Core.Database.IntegrationTests.Abstract;
using Core.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Core.Database.IntegrationTests.Commands
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Integration - Database")]
    public class CreateNewOrderCommandTests : IntegrationTest
    {
        private readonly Faker _faker = new Faker();

        private CreateNewOrderCommand CreateTestedComponent(OrderContext dbContext)
        {
            return new CreateNewOrderCommand(dbContext);
        }

        private readonly CreateNewOrderCommand.Context _commandContext;

        public CreateNewOrderCommandTests()
        {
            _commandContext = new CreateNewOrderCommand.Context()
            {
                Phone = _faker.Phone.PhoneNumber("+7 ### ###-##-##"),
                From = _faker.Address.FullAddress(),
                To = _faker.Address.FullAddress(),
                When = _faker.Date.Soon()
            };
        }

        [Fact]
        public void Execute__ReturnSuccess()
        {
            using (var dbContext = CreateTransactionalDbContext())
            {
                // Arrange
                var command = CreateTestedComponent(dbContext);
                // Act
                var result = command.Execute(_commandContext);
                // Assert
                Assert.True(result.Success);
            }
        }

        [Fact]
        public void Execute__OrdersTableContainsNewOrder()
        {
            using (var dbContext = CreateTransactionalDbContext())
            {
                // Arrange
                var command = CreateTestedComponent(dbContext);
                // Act
                var result = command.Execute(_commandContext);
                // Assert
                var insertedId = result.Value.Id;
                var insertedOrder = dbContext.Orders.FirstOrDefault(e => e.Id == insertedId);
                Assert.NotNull(insertedOrder);
            }
        }

        [Fact]
        public void Execute__NewOrderHasStatusNew()
        {
            using (var dbContext = CreateTransactionalDbContext())
            {
                // Arrange
                var command = CreateTestedComponent(dbContext);
                // Act
                var result = command.Execute(_commandContext);
                // Assert
                var insertedId = result.Value.Id;
                var insertedOrder = dbContext.Orders.FirstOrDefault(e => e.Id == insertedId);
                Assert.Equal(StatusEnum.New, insertedOrder?.Status);
            }
        }
    }
}