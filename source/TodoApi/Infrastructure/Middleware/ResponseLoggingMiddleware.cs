using Serilog;
using Serilog.Context;
using TodoApi.Application.Common.Interfaces;

namespace TodoApi.Infrastructure.Middleware
{
    public class ResponseLoggingMiddleware : IMiddleware
    {

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            await next(httpContext);
            Stream originalBody = httpContext.Response.Body;
            using var newBody = new MemoryStream();
            httpContext.Response.Body = newBody;
            string responseBody;

            if (httpContext.Request.Path.ToString().Contains("tokens"))
            {
                responseBody = "[Redacted] Contains Sensitive Information.";
            }
            else
            {
                newBody.Seek(0, SeekOrigin.Begin);
                responseBody = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
            }

            LogContext.PushProperty("StatusCode", httpContext.Response.StatusCode);
            LogContext.PushProperty("ResponseTimeUTC", DateTime.UtcNow);
            Log.ForContext("ResponseHeaders", httpContext.Response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
                .ForContext("ResponseBody", responseBody)
                .Information("HTTP {RequestMethod} Request to {RequestPath} has Status Code {StatusCode}.", httpContext.Request.Method, httpContext.Request.Path, httpContext.Response.StatusCode);

            newBody.Seek(0, SeekOrigin.Begin);
            await newBody.CopyToAsync(originalBody);
        }

    }
}