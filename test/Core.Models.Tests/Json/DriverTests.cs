using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core.Models.Fake;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Core.Models.Tests.Json
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Integration - Json")]
    public class DriverTests
    {
        private readonly Driver _driver;
        private readonly string _jsonString;

        public DriverTests()
        {
            _driver = DriverFake.Generate();
            _driver.Orders = OrderFake.Generate(3);
            _jsonString = JsonConvert.SerializeObject(_driver);
        }

        [Fact]
        public void ToJson__IdIsValid()
        {
            // Arrange
            // Act
            dynamic json = JObject.Parse(_jsonString);
            // Assert
            Assert.Equal(_driver.Id, (int)json["id"]);
        }

        [Fact]
        public void ToJson__FullNameIsValid()
        {
            // Arrange
            // Act
            dynamic json = JObject.Parse(_jsonString);
            // Assert
            Assert.Equal(_driver.FullName, (string)json["fullName"]);
        }

        [Fact]
        public void ToJson__PhoneIsValid()
        {
            // Arrange
            // Act
            dynamic json = JObject.Parse(_jsonString);
            // Assert
            Assert.Equal(_driver.Phone, (string)json["phone"]);
        }

        [Fact]
        public void ToJson__OrdersContainsValidItems()
        {
            // Arrange
            // Act
            dynamic json = JObject.Parse(_jsonString);
            // Assert
            var orders = (List<Order>)json["orders"].ToObject<List<Order>>();
            Assert.Equal(_driver.Orders.Count, orders.Count);
            Assert.DoesNotContain(orders, e => e == null);
        }

        [Fact]
        public void ToJson__IgnoredPropertiesIsNull()
        {
            // Arrange
            // Act
            dynamic json = JObject.Parse(_jsonString);
            // Assert
            Assert.Null(json["SomeIgnoredProperty"]);
        }
    }
}