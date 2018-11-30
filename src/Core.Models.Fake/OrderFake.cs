using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Bogus;
using Core.Models.ApiModels;
using Core.Models.Enums;

namespace Core.Models.Fake
{
    [ExcludeFromCodeCoverage]
    public static class OrderFake
    {
        private static readonly Faker<Order> Faker =
                new Faker<Order>()
                    .RuleFor(e => e.Id, f => f.Random.Int(min: 0))
                    .RuleFor(e => e.From, f => f.Address.FullAddress())
                    .RuleFor(e => e.To, f => f.Address.FullAddress())
                    .RuleFor(e => e.Phone, f => f.Phone.PhoneNumber("+7-9##-###-##-##"))
                    .RuleFor(e => e.Comments, f => f.Random.Words())
                    .RuleFor(e => e.When, f => f.Date.Soon())
            ;

        public static Order Generate()
        {
            return Faker.Generate();
        }
        public static List<Order> Generate(int count)
        {
            return Faker.Generate(count).ToList();
        }

        #region WithStatus

        public static Order WithStatus(this Order order, string status)
        {
            order.Status = status;
            return order;
        }

        public static List<Order> WithStatus(this List<Order> orders, string status)
        {
            orders.ForEach(e => e.WithStatus(status));
            return orders;
        }

        #endregion
    }
}
