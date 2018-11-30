using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Taxi.Api.Service.IntegrationTests.Infrastructure;
using Xunit;

namespace Taxi.Api.Service.IntegrationTests
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Integration - Routing")]
    public class SiteRoutingTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public SiteRoutingTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        public async Task Get_ClientReturnSuccess(string url)
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var response = await client.GetAsync(url);
            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
