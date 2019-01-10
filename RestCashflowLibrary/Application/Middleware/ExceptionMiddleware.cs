using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestCashflowLibrary.Infrastructure.CustomException;
using RestCashflowLibrary.Infrastructure.Model.ValueObject;

namespace RestCashflowLibrary.Application.Middleware
{
    public class ExceptionMiddleware  : IMiddleware<ExceptionMiddleware>
    {
        public ILogger<ExceptionMiddleware> Logger { get; set; }
        public IConfiguration Configuration { get; set; }

        public ExceptionMiddleware(
            ILogger<ExceptionMiddleware> logger,
            IConfiguration configuration
        )
        {
            Logger = logger;
            Configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
                return;
            }
            catch (ApiException apiException)
            {
                await HandleExceptionAsync(context, apiException);
                return;
            }
            catch (Exception ex)
            {
                var apiException = new ApiException(ex.Message, HttpStatusCode.InternalServerError);
                await HandleExceptionAsync(context, apiException);
                return;
            }
        }

        public  async Task HandleExceptionAsync(HttpContext context, ApiException apiException)
        {
            Logger.LogError(JsonConvert.SerializeObject(new {
                Message = apiException.Message,
                StackTrace = apiException.StackTrace
            }));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)apiException.HttpStatusCode;
            var errorObj = new
            {
                errors = new object[] {
                    new ErrorResponseValueObject() {
                        message = apiException.Message
                    }
                }
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorObj));
            return;
        }
    }
}
