using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using TaskManagement.Core.Application.Exceptions;

namespace TaskManagement.API.Middlewares
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate next;

        public ExceptionHandler(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string titleText = "Internal Server Error.";
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var traceId = Activity.Current?.Id ?? context?.TraceIdentifier;

            switch (exception)
            {
                case EntityValidationException e:
                    titleText = "One or more validation errors occurred.";
                    statusCode = (int)e.StatusCode;
                    break;
                case Exception _:
                    Serilog.Log.Error(exception, exception.Message);
                    //exception = new Exception("Internal Server Error");
                    break;
            }


            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var logContent = new
            {
                title = titleText,
                status = statusCode,
                traceId = traceId,
                errors = new
                {
                    messages = new string[] { exception.InnerException?.InnerException?.Message ?? exception.InnerException?.Message ?? exception.Message }
                }
            };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(logContent));
        }
    }
}
