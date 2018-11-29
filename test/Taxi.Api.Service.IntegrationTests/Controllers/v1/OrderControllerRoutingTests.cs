using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Core.Database;
using Core.Database.Abstract;
using Core.Models.ApiModels.Fakes;
using Ether.Outcomes.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NSubstitute;
using Taxi.Api.Service.Controllers.v1;
using Xunit;

namespace Taxi.Api.Service.IntegrationTests.Controllers.v1
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Integration - Routing")]
    public class OrderControllerRoutingTests : RoutingTest
    {
        public OrderControllerRoutingTests(WebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Post__MakeNewOrder()
        {
            // Arrange
            var makeOrderModel = MakeOrderModelFake.Generate();
            var json = JsonConvert.SerializeObject(makeOrderModel);
            var url = "/api/v1/order/make";
            var client = WebApplicationFactory.CreateClient();
            var httpContent = CreateHttpContent(json);
            // Act
            var response = await client.PostAsync(url, httpContent);
            var makeOrderResultAsJson = await response.Content.ReadAsStringAsync();
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("\"success\":true", makeOrderResultAsJson);
        }
    }
}