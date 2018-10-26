using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Core.BusinessLogic.CommandRequests;
using Core.Models.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
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
    }
}