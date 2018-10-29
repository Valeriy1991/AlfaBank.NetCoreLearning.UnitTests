using System;
using Core.Models.ApiModels;
using Ether.Outcomes;
using MediatR;

namespace Core.BusinessLogic.CommandRequests
{
    public class MakeTaxiOrderCommandRequest : IRequest<IOutcome>
    {
        public string Phone { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Comments { get; set; }
        public DateTime When { get; set; }

        public static MakeTaxiOrderCommandRequest Create(MakeOrderTaxiModel model)
        {
            return new MakeTaxiOrderCommandRequest()
            {
                From = model.From,
                To = model.To,
                Comments = model.Comments,
                Phone = model.Phone,
                When = model.When
            };
        }
    }
}