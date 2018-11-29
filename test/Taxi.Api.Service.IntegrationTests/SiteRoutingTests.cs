using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Taxi.Api.Service.IntegrationTests
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Integration - Routing")]
    public class SiteRoutingTests : RoutingTest
    {
        public SiteRoutingTests(WebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("/")]
        public async Task Get_ClientReturnSuccess(string url)
        {
            // Arrange
            var client = WebApplicationFactory.CreateClient();
            // Act
            var response = await client.GetAsync(url);
            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
