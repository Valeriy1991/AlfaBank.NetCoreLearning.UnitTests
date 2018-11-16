using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bogus;
using Core.BusinessLogic.WebServices;
using Core.BusinessLogic.WebServices.UrlBuilders;
using Core.Models;
using Core.Models.Fake;
using Core.Models.Settings;
using Flurl.Http;
using Flurl.Http.Testing;
using NSubstitute;
using Xunit;

namespace Core.BusinessLogic.Tests.WebServices
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit")]
    public class DriverRestServiceTests : IDisposable
    {
        private readonly Faker _faker = new Faker();
        private readonly HttpTest _httpTest;

        public DriverRestServiceTests()
        {
            _httpTest = new HttpTest();
        }

        private DriverRestService CreateTestedComponent(DriverRestServiceUrlBuilder urlBuilder)
        {
            return new DriverRestService(urlBuilder);
        }

        [Fact]
        public void GetVacantDriversAsync__UrlWasCalledAtOnce()
        {
            // Arrange
            var currentDateTime = _faker.Date.Recent();
            var url = _faker.Internet.Url();

            #region UrlBuilder

            var restServiceSettings = Substitute.For<RestServiceSettings>();
            var urlBuilder = Substitute.For<DriverRestServiceUrlBuilder>(restServiceSettings);
            urlBuilder.GetVacantDriversUrl(Arg.Any<DateTime>()).Returns(url);

            #endregion

            var restService = CreateTestedComponent(urlBuilder);
            // Act
            restService.GetVacantDriversAsync(currentDateTime);
            // Assert
            _httpTest.ShouldHaveCalled(url).Times(1);
        }

        [Fact]
        public async Task GetVacantDriversAsync__UrlWasCalledWithValidVerbAndConcentType()
        {
            // Arrange
            var drivers = DriverFake.Generate(4);
            _httpTest.RespondWithJson(drivers);
            var currentDateTime = _faker.Date.Recent();
            var url = _faker.Internet.Url();

            #region UrlBuilder

            var restServiceSettings = Substitute.For<RestServiceSettings>();
            var urlBuilder = Substitute.For<DriverRestServiceUrlBuilder>(restServiceSettings);
            urlBuilder.GetVacantDriversUrl(Arg.Any<DateTime>()).Returns(url);

            #endregion

            var restService = CreateTestedComponent(urlBuilder);
            // Act
            await restService.GetVacantDriversAsync(currentDateTime);
            // Assert
            _httpTest.ShouldHaveCalled(url)
                .WithVerb(HttpMethod.Get);
        }

        [Fact]
        public async Task GetVacantDriversAsync__ReturnValidVacantDrivers()
        {
            // Arrange
            var drivers = DriverFake.Generate(4);
            _httpTest.RespondWithJson(drivers);
            var currentDateTime = _faker.Date.Recent();
            var url = _faker.Internet.Url();

            #region UrlBuilder

            var restServiceSettings = Substitute.For<RestServiceSettings>();
            var urlBuilder = Substitute.For<DriverRestServiceUrlBuilder>(restServiceSettings);
            urlBuilder.GetVacantDriversUrl(Arg.Any<DateTime>()).Returns(url);

            #endregion

            var restService = CreateTestedComponent(urlBuilder);
            // Act
            var vacantDrivers = await restService.GetVacantDriversAsync(currentDateTime);
            // Assert
            Assert.Equal(drivers.Count, vacantDrivers.Count);
            Assert.All(vacantDrivers, driver => drivers.Exists(e => e.Id == driver.Id));
            Assert.True(vacantDrivers.All(e => drivers.Exists(driver => driver.Id == e.Id)));
        }

        public void Dispose()
        {
            _httpTest.Dispose();
        }
    }
}