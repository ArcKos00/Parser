using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Parser.Common.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {

            var problemDetails = new ValidationProblemDetails()
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status400BadRequest,
                Detail = context.Exception.Message
            };

            context.Result = new BadRequestObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            _logger.LogError(
                    new EventId(context.Exception.HResult),
                    context.Exception,
                    context.Exception.Message);

            context.ExceptionHandled = true;
        }
    }
}
