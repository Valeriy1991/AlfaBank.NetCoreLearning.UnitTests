using System;
using Ether.Outcomes;
using MediatR;

namespace Core.BusinessLogic.CommandRequests
{
    public class FinishOrderCommandRequest : IRequest<IOutcome>
    {
        public int OrderId { get; set; }
    }
}