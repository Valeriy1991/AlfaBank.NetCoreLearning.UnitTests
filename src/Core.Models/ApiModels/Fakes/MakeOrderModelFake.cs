using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Models.ApiModels.Fakes
{
    [ExcludeFromCodeCoverage]
    public static class MakeOrderModelFake
    {
        public static MakeOrderModel Generate()
        {
            return new MakeOrderModel()
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