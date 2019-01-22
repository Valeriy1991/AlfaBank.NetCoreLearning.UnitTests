using System.Diagnostics.CodeAnalysis;
using Core.Models.Fake;
using Core.Models.Tests.Json.Abstract;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace Core.Models.Tests.Json
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Integration - Json")]
    public class DriverTestsV2 : JsonTest
    {
        private readonly Driver _driver;
        private readonly string _jsonString;

        public DriverTestsV2()
        {
            _driver = DriverFake.Generate();
            _driver.Orders = OrderFake.Generate(3);
            // Используем общую логику сериализации из базового класса
            _jsonString = SerializeObjectToJson(_driver);
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
        public void ToJson__Phone()
        {
            // Arrange
            // Act
            dynamic json = JObject.Parse(_jsonString);
            // Assert
            Assert.Equal(_driver.Phone, (string)json["phone"]);
        }
    }
}