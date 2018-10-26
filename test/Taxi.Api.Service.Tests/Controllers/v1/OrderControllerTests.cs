using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Core.BusinessLogic.CommandRequests;
using Core.Models.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Taxi.Api.Service.Controllers.v1;
using Xunit;

namespace Taxi.Api.Service.Tests.Controllers.v1
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit")]
    public class OrderControllerTests
    {
        private OrderController CreateTestedComponent(IMediator mediator)
        {
            return new OrderController(mediator);
        }

        private MakeOrderTaxiModel GenerateMakeOrderTaxiModel()
        {
            return new MakeOrderTaxiModel()
            {
                From = "from-address",
                To = "to-address",
                Comments = "some-comments",
                Phone = "123456789",
                When = DateTime.Now
            };
        }

        [Fact]
        public async Task Make__SendMethodOfIMediatorWasCalledAtOnceWithValidRequest()
        {
            // Arrange
            var mediator = Substitute.For<IMediator>();
            var controller = CreateTestedComponent(mediator);
            var model = GenerateMakeOrderTaxiModel();
            // Act
            await controller.Make(model);
            // Assert
            await mediator.Received(1).Send(Arg.Any<MakeTaxiOrderCommandRequest>());
        }
        [Fact]
        public async Task Make__ReturnOk()
        {
            // Arrange
            var mediator = Substitute.For<IMediator>();
            var controller = CreateTestedComponent(mediator);
            var model = GenerateMakeOrderTaxiModel();
            // Act
            var result = await controller.Make(model);
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Make__MediatorSendThrowException()
        {
            // Arrange
            var errorMessage = "test error";
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<MakeTaxiOrderCommandRequest>(), CancellationToken.None)
                .Throws(info => new Exception(errorMessage));
            var controller = CreateTestedComponent(mediator);
            var model = GenerateMakeOrderTaxiModel();
            Func<Task> act = () => controller.Make(model);
            // Act
            var ex = Record.ExceptionAsync(act)?.Result;
            // Assert
            Assert.IsType<Exception>(ex);
            Assert.Equal(errorMessage, ex.Message);
        }
    }
}