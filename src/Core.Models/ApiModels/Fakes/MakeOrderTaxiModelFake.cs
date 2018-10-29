using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Models.ApiModels.Fakes
{
    [ExcludeFromCodeCoverage]
    public static class MakeOrderTaxiModelFake
    {
        public static MakeOrderTaxiModel Generate()
        {
            return new MakeOrderTaxiModel()
            {
                From = "from-address",
                To = "to-address",
                Comments = "some-comments",
                Phone = "123456789",
                When = DateTime.Now
            };
        }
    }
}