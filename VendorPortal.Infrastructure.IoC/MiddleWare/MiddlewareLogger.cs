using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Newtonsoft.Json;
using VendorPortal.Application.Helpers;
using VendorPortal.Application.Models.Common;
using Serilog;
using System.Linq;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging.Abstractions;

namespace VendorPortal.Infrastructure.IoC.Middleware
{
    public class MiddlewareLogger
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _log;
        private readonly RecyclableMemoryStreamManager _memory;
        public MiddlewareLogger(RequestDelegate next, Serilog.ILogger log)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _memory = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context)
        {
            Stopwatch sw = Stopwatch.StartNew();

            //Request
            context.Request.EnableBuffering();
            await using var requestStream = _memory.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            var requestData = ReadStreamInChunks(requestStream);
            context.Request.Body.Position = 0;
            
            //Response
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _memory.GetStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var responseData = await new StreamReader(context.Response.Body).ReadToEndAsync();
            
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            try
            {
                sw.Stop();

                var settings = new JsonSerializerSettings
                {
                    Error = (sender, args) => { args.ErrorContext.Handled = true; },
                    MissingMemberHandling = MissingMemberHandling.Error
                };

                var baseResponse = JsonConvert.DeserializeObject<BaseResponse>(responseData, settings);

                // context.Response.Headers.Add("traceID", Agent.Tracer?.CurrentTransaction?.TraceId);
                context.Response.Headers.Append("Content-Security-Policy", "frame-ancestors 'self';");
                context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

                var request = context.Request;
                var response = context.Response;

                if (!context.Response.Headers.ContainsKey("responsecode"))
                {
                    context.Response.Headers.Append("responsecode", response.StatusCode != StatusCodes.Status200OK
                        ? response.StatusCode.ToString() 
                        : baseResponse?.status?.code);
                }

                if (!context.Response.Headers.ContainsKey("responsemessage"))
                {
                    context.Response.Headers.Append("responsemessage", response.StatusCode != StatusCodes.Status200OK
                        ? MappingResponseMessage(response.StatusCode.ToString()) 
                        : MappingResponseMessage(baseResponse?.status?.code));
                }

                var ip = context.Connection.RemoteIpAddress;
                var port = context.Connection.RemotePort;

                var fullIP = ip == null ? "Localhost" : (IPAddress.IsLoopback(ip) ? "Localhost" : $"{ip}:{port}");

                var requestHeadersDictionary = new Microsoft.AspNetCore.Http.HeaderDictionary();
                var responseHeadersDictionary = new Microsoft.AspNetCore.Http.HeaderDictionary();

                foreach (var itemRequestHeader in request.Headers)
                {
                    requestHeadersDictionary.Add(itemRequestHeader.Key, itemRequestHeader.Value.FirstOrDefault());
                }

                foreach (var itemResponseHeader in response.Headers)
                {
                    responseHeadersDictionary.Add(itemResponseHeader.Key, itemResponseHeader.Value.FirstOrDefault());
                }

                request.Headers.TryGetValue("refer", out StringValues refer);
                request.Headers.TryGetValue("forward", out StringValues forward);
                
                _log.ForContext("Info", "", true).Information("Receive request and processed");

                var logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "request_response_log.txt");
                Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
                var logEntry = new StringBuilder();
                logEntry.AppendLine("Header:");
                foreach (var header in request.Headers)
                {
                    logEntry.AppendLine($"{header.Key}: {header.Value}");
                }
                logEntry.Append("Query:");
                foreach (var item in request.Query)
                {
                    logEntry.Append(item.Key + ": " + item.Value + "|");
                }
                logEntry.AppendLine();
                logEntry.AppendLine("--------------------------------------------------");
                logEntry.AppendLine("Request:");
                logEntry.AppendLine(requestData);
                logEntry.AppendLine("Response:");
                logEntry.AppendLine(responseData);
                logEntry.AppendLine("Execution Time:");
                logEntry.AppendLine(sw.ElapsedMilliseconds.ToString());
                logEntry.AppendLine("--------------------------------------------------");

                await File.AppendAllTextAsync(logFilePath, logEntry.ToString());
            }
            catch (Exception ex)
            {
                _log.Error(ex, ex.Message);
            }

            await responseBody.CopyToAsync(originalBodyStream);
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);
            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk,
                                                   0,
                                                   readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);
            return textWriter.ToString();
        }

        private string MappingResponseMessage(string responseCode)
        {
            return responseCode switch
            {
                ("200") => "Success",
                ("400") => "Bad Request",
                ("401") => "Unauthorized",
                ("403") => "Forbidden",
                ("404") => "Not Found",
                ("409") => "Conflict",
                ("422") => "Unprocessable Entity",
                ("500") => "Internal Server Error",
                ("501") => "Not Implemented",
                ("502") => "Bad Gateway",
                ("503") => "Service Unavailable",
                ("504") => "Gateway Time-Out",
                _ => responseCode,
            };
        }
    }
}