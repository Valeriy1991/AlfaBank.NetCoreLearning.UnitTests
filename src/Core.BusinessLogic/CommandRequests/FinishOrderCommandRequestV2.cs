using System;
using Ether.Outcomes;
using MediatR;

namespace Core.BusinessLogic.CommandRequests
{
    public class FinishOrderCommandRequestV2 : IRequest<IOutcome>
    {
        public int OrderId { get; set; }
        public DateTime? FinishDateTime { get; set; } = DateTime.Now;
    }
}