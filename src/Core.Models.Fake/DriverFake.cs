using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Bogus;
using Core.Models.ApiModels;

namespace Core.Models.Fake
{
    [ExcludeFromCodeCoverage]
    public static class DriverFake
    {
        private static readonly Faker<Driver> Faker =
                new Faker<Driver>()
                    .RuleFor(e => e.Id, f => f.Random.Int(min: 0))
                    .RuleFor(e => e.FullName, f => f.Person.FullName)
                    .RuleFor(e => e.Phone, f => f.Phone.PhoneNumber("+7-9##-###-##-##"))
            ;

        public static Driver Generate()
        {
            return Faker.Generate();
        }
        public static List<Driver> Generate(int count)
        {
            return Faker.Generate(count).ToList();
        }
    }
}
