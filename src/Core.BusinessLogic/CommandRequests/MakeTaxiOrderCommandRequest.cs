using System;
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
    }
}