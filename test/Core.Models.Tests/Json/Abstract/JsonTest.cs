using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Models.Tests.Json.Abstract
{
    [ExcludeFromCodeCoverage]
    public abstract class JsonTest
    {
        protected string SerializeObjectToJson(object model)
        {
            var contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            return JsonConvert.SerializeObject(model, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
        }
    }
}