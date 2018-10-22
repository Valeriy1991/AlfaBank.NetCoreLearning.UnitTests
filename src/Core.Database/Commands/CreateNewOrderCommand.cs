using System;
using Core.Database.Abstract;
using Core.Models;
using CQRSlight.Abstract;
using Ether.Outcomes;

namespace Core.Database.Commands
{
    public class CreateNewOrderCommand : DbContextCommand<OrderContext, CreateNewOrderCommand.Context, Order>
    {
        public class Context
        {
            public string Phone { get; set; }
            public string From { get; set; }
            public string To { get; set; }
            public string Comments { get; set; }
            public DateTime When { get; set; }
        }

        public CreateNewOrderCommand(OrderContext dbContext) : base(dbContext)
        {
        }

        public override IOutcome<Order> Execute(Context commandContext)
        {
            try
            {
                var order = new Order()
                {
                    Phone = commandContext.Phone,
                    Comments = commandContext.Comments,
                    From = commandContext.From,
                    To = commandContext.To,
                    When = commandContext.When,
                    Status = StatusEnum.New
                };
                DbContext.Orders.Add(order);
                DbContext.SaveChanges();
                return Outcomes.Success(order);
            }
            catch (Exception ex)
            {
                return Outcomes.Failure<Order>().FromException(ex);
            }
        }
    }
}