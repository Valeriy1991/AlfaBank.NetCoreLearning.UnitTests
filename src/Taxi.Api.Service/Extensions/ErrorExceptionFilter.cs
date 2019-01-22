using Core.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Taxi.Api.Service.Extensions
{
    public class ErrorExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var error = new ErrorModel()
            {
                Message = context.Exception.Message
            };
            context.Result = new BadRequestObjectResult(error);
        }
    }
}