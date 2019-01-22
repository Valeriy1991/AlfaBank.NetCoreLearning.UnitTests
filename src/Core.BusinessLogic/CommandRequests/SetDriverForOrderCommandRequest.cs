using Ether.Outcomes;
using MediatR;

namespace Core.BusinessLogic.CommandRequests
{
    public class SetDriverForOrderCommandRequest : IRequest<IOutcome>
    {
        public int OrderId { get; set; }
        public int DriverId { get; set; }
    }
}