using System;
using System.Collections.Generic;
using Core.Models;
using MediatR;

namespace Core.BusinessLogic.QueryRequests
{
    public class VacantDriversQueryRequest : IRequest<List<Driver>>
    {
        public DateTime CurrentDateTime { get; set; } = DateTime.Now;
    }
}