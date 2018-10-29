using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Bogus;

namespace Core.Models.ApiModels.Fakes
{
    [ExcludeFromCodeCoverage]
    public static class MakeOrderModelFake
    {
        /*
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
        */
        private static readonly Faker<MakeOrderModel> Faker =
                new Faker<MakeOrderModel>()
                    .RuleFor(e => e.From, f => f.Address.FullAddress())
                    .RuleFor(e => e.To, f => f.Address.FullAddress())
                    .RuleFor(e => e.Comments, f => f.Random.Words())
                    .RuleFor(e => e.Phone, f => f.Phone.PhoneNumber("+7-9##-###-##-##"))
                    .RuleFor(e => e.When, f => f.Date.Future())
            ;

        public static MakeOrderModel Generate()
        {
            return Faker.Generate();
        }
        public static List<MakeOrderModel> Generate(int count)
        {
            return Faker.Generate(count).ToList();
        }

        #region WithPhone

        public static MakeOrderModel WithPhone(this MakeOrderModel model, string phone)
        {
            model.Phone = phone;
            return model;
        }
        public static List<MakeOrderModel> WithPhone(this List<MakeOrderModel> models, string phone)
        {
            models.ForEach(e => e.WithPhone(phone));
            return models;
        }

        #endregion
    }
}