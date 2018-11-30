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
        [Fact]
        public void ToJson__AllSimplePropertiesIsValid()
        {
            // Arrange
            var model = DriverFake.Generate();
            // Act
            var jsonString = JsonConvert.SerializeObject(model);
            // Assert
            dynamic json = JObject.Parse(jsonString);
            Assert.Equal(model.Id, (int)json["id"]);
            Assert.Equal(model.FullName, (string)json["fullName"]);
            Assert.Equal(model.Phone, (string)json["phone"]);
        }

        [Fact]
        public void ToJson__OrdersContainsValidItems()
        {
            // Arrange
            var driver = DriverFake.Generate();
            driver.Orders = OrderFake.Generate(3);
            var jsonString = JsonConvert.SerializeObject(driver);
            // Act
            dynamic json = JObject.Parse(jsonString);
            // Assert
            var orders = (List<Order>)json["orders"].ToObject<List<Order>>();
            Assert.Equal(driver.Orders.Count, orders.Count);
            Assert.DoesNotContain(orders, e => e == null);
        }

        [Fact]
        public void ToJson__IgnoredPropertiesIsNull()
        {
            // Arrange
            var model = DriverFake.Generate();
            // Act
            var jsonString = JsonConvert.SerializeObject(model);
            // Assert
            dynamic json = JObject.Parse(jsonString);
            Assert.Null(json["SomeIgnoredProperty"]);
            Assert.Null(json["someIgnoredProperty"]);
        }
    }
}