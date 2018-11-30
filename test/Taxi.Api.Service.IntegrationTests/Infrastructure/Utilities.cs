using System;
using System.Collections.Generic;
using Core.Database;
using Core.Models;
using Core.Models.Enums;

namespace Taxi.Api.Service.IntegrationTests.Infrastructure
{
    // Code from https://github.com/aspnet/Docs/blob/master/aspnetcore/test/integration-tests/samples/2.x/IntegrationTestsSample/tests/RazorPagesProject.Tests/Helpers/Utilities.cs
    public static class Utilities
    {
        #region snippet1
        public static void InitializeDbForTests(OrderContext db)
        {
            db.Orders.AddRange(GetSeedingMessages());
            db.SaveChanges();
        }

        public static List<Order> GetSeedingMessages()
        {
            return new List<Order>()
            {
                new Order()
                {
                    Id = 1,
                    Phone = "+7 123 456 78 90",
                    From = "from place",
                    To = "to place",
                    Status = StatusEnum.New,
                    When = DateTime.Now
                }
            };
        }
        #endregion
    }
}