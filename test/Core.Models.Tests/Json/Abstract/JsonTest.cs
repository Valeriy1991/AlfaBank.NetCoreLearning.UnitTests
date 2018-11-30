using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Core.Models.Tests.Json.Abstract
{
    [ExcludeFromCodeCoverage]
    public abstract class JsonTest
    {
        public string SerializeObjectToJson(object model)
        {
            return JsonConvert.SerializeObject(model, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }
    }
}