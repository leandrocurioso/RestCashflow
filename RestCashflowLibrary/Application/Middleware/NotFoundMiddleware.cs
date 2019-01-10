using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestCashflowLibrary.Infrastructure.Model.ValueObject;

namespace RestCashflowLibrary.Application.Middleware
{
    public class NotFoundMiddleware : IMiddleware<NotFoundMiddleware>
    {
        public ILogger<NotFoundMiddleware> Logger { get; set; }
        public IConfiguration Configuration { get; set; }

        public NotFoundMiddleware(
            ILogger<NotFoundMiddleware> logger,
            IConfiguration configuration
        )
        {
            Logger = logger;
            Configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next(context);
            context.Response.ContentType = "application/json";
            if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                var errorObj = new
                {
                    errors = new object[] {
                        new ErrorResponseValueObject() {
                            message =  "Not Found"
                        }
                }
                };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorObj));
            }
            return;
        }
    }
}
