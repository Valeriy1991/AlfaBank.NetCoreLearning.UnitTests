using System;
using System.Diagnostics.CodeAnalysis;
using Bogus;

namespace Core.Models.ApiModels.Fakes
{
    [ExcludeFromCodeCoverage]
    public static class MakeOrderModelFake
    {
        private static readonly Faker Faker = new Faker();

        public static MakeOrderModel Generate()
        {
            return new MakeOrderModel()
            {
                From = Faker.Address.FullAddress(),
                To = Faker.Address.FullAddress(),
                Comments = Faker.Random.Words(),
                Phone = Faker.Phone.PhoneNumber("+7-###-###-##-##"),
                When = Faker.Date.Future()
            };
        }
    }
}