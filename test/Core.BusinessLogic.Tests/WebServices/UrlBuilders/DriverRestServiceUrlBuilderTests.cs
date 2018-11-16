using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Bogus;
using Core.BusinessLogic.WebServices.UrlBuilders;
using Core.Models.Settings;
using Xunit;

namespace Core.BusinessLogic.Tests.WebServices.UrlBuilders
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit")]
    public class DriverRestServiceUrlBuilderTests
    {
        private readonly Faker _faker = new Faker();

        private DriverRestServiceUrlBuilder CreateTestedComponent(RestServiceSettings restServiceSettings)
        {
            return new DriverRestServiceUrlBuilder(restServiceSettings);
        }

        [Fact]
        public void GetVacantDriversUrl__ReturnValidUrlWithRightQueryParams()
        {
            // Arrange
            var host = _faker.Internet.DomainName();
            var version = $"v{_faker.Random.Int(min: 1, max: 9)}";
            var restServiceSettings = new RestServiceSettings()
            {
                DriverApi = new RestServiceSettings.Service()
                {
                    Host = host,
                    Version = version
                }
            };
            var urlBuilder = CreateTestedComponent(restServiceSettings);
            var onDateTime = _faker.Date.Recent();
            // Act
            var url = urlBuilder.GetVacantDriversUrl(onDateTime);
            // Assert
            var urlEncodedOnDateTime = WebUtility.UrlEncode(onDateTime.ToString("s"));
            var validUrl = $"{host}/{version}/driver/vacant?onDateTime={urlEncodedOnDateTime}";
            Assert.Equal(validUrl, url);
        }
    }
}