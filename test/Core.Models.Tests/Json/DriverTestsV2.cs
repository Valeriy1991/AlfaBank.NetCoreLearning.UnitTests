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
        [Fact]
        public void ToJson__AllSimplePropertiesIsValid()
        {
            // Arrange
            var model = DriverFake.Generate();
            // Act
            var jsonString = SerializeObjectToJson(model);
            // Assert
            dynamic json = JObject.Parse(jsonString);
            Assert.Equal(model.Id, (int)json["id"]);
            Assert.Equal(model.FullName, (string)json["fullNamename"]);
            Assert.Equal(model.Phone, (string)json["phone"]);
        }
    }
}