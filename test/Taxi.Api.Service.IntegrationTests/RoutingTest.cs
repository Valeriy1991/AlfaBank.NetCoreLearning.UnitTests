using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Taxi.Api.Service.IntegrationTests
{
    [ExcludeFromCodeCoverage]
    public abstract class RoutingTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        protected WebApplicationFactory<Startup> WebApplicationFactory { get; }

        protected RoutingTest(WebApplicationFactory<Startup> factory)
        {
            WebApplicationFactory = factory;
        }

        protected T DeserializeJson<T>(string jsonAsString)
        {
            var jsonSerializationSettings = new JsonSerializerSettings()
            {
                
            };
            return JsonConvert.DeserializeObject<T>(jsonAsString, jsonSerializationSettings);
        }

        protected StringContent CreateHttpContent(string json)
        {
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}