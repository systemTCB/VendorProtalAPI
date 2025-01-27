using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using VendorPortal.Application.Helpers;

namespace VendorPortal.API.Middleware
{
    public class MiddlewareLogger
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MiddlewareLogger> _logger;
        private readonly AppConfigHelper _appConfigHelper;
        public MiddlewareLogger(RequestDelegate next, ILogger<MiddlewareLogger> logger , AppConfigHelper appConfigHelper)
        {
            _next = next;
            _logger = logger;
            _appConfigHelper = appConfigHelper;
        }


        public async Task Invoke(HttpContext context)
        {
            LogRequest(context);
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                LogResponse(context);

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
        private void LogRequest(HttpContext context)
        {
            var request = context.Request;
            var requestLog = new StringBuilder();
            requestLog.AppendLine("Incoming Request:");
            requestLog.AppendLine($"HTTP {request.Method} {request.Path.Value}");
            requestLog.AppendLine($"Host: {request.Host.Value}");
            requestLog.AppendLine($"Content-Type: {request.ContentType}");
            requestLog.AppendLine($"Content-Length: {request.ContentLength}");
            var logMessage = requestLog.ToString();
            _logger.LogInformation(logMessage);
        }

        private void LogResponse(HttpContext context)
        {
            // var response = context.Response;
            // var responseLog = new StringBuilder();
            // responseLog.AppendLine("Outgoing Response:");
            // responseLog.AppendLine($"HTTP {response.StatusCode}");
            // responseLog.AppendLine($"Content-Type: {response.ContentType}");
            // responseLog.AppendLine($"Content-Length: {response.ContentLength}");
            // var logMessage = responseLog.ToString();
            // _logger.LogInformation(responseLog.ToString());
        }
    }
}