using Common;
using Common.Constant;
using Infastructure.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Serilog.Context;
using System.Data.SqlClient;

namespace Util.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ILogger<ErrorHandlingMiddleware> logger, PropagateHeaders _propagateHeaders)
        {
            try
            {
                List<CommonHeader> headersList = HeadersConstant.getAllHeaders();

                foreach (CommonHeader tTHeaders in headersList)
                {

                    string header = string.Empty;
                    httpContext.Request.Headers.TryGetValue(tTHeaders.Name, out StringValues headerValues);
                    header = headerValues.FirstOrDefault()!;

                    if ((header == null || header == "") && !tTHeaders.Required)
                    {
                        header = tTHeaders.GeneratedValue();
                        httpContext.Request.Headers.Remove(tTHeaders.Name);
                        httpContext.Request.Headers.Add(tTHeaders.Name, header);
                    }

                    if (tTHeaders.TemplateName != null)
                    {
                        LogContext.PushProperty(tTHeaders.TemplateName, header);
                    }

                    _propagateHeaders.Headers.Add(tTHeaders.Name, header!);
                }
                await _next(httpContext);
            }
            catch (SqlException ex)
            {
               // await _emailService.SendSqlException(ex);
                await HandleSqlExceptionAsync(httpContext, ex, _propagateHeaders);
            }
            catch (ValidationException ex)
            {
                await HandleValidationsAsync(httpContext, ex, logger, _propagateHeaders);
            }
            catch (Exception ex)
            {
              //  await _emailService.SendException(ex);
                await HandleExceptionAsync(httpContext, ex, logger, _propagateHeaders);
            }
        }
        private async Task HandleValidationsAsync(HttpContext httpContext, Exception ex, ILogger<ErrorHandlingMiddleware> logger, PropagateHeaders _propagateHeaders)
        {
            var correlationId = _propagateHeaders.Headers.Where(x => x.Key == HeadersConstant.correlationId).FirstOrDefault().Value?.ToString();
            var exceptionDetails = GetExceptionDetails(ex);
            var problemDetails = new CustomProblemDetail
            {
                Status = exceptionDetails.Status,
                Type = exceptionDetails.Type,
                Title = exceptionDetails.Title,
                Detail = exceptionDetails.Details,
                CorrelationId = correlationId

            };
            if (exceptionDetails.Errors is not null)
            {
                problemDetails.Extensions["errors"] = exceptionDetails.Errors;
            }
            httpContext.Response.StatusCode = exceptionDetails.Status;
            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }
        private static ExceptionDetails GetExceptionDetails(Exception exception)
        {
            return exception switch
            {
                ValidationException validationException => new ExceptionDetails(
                    StatusCodes.Status400BadRequest,
                    "Validation Failure",
                    "Validation Error",
                    "One or more validation errors has occurred",
                    validationException.Errors),
                _ => new ExceptionDetails(
                    StatusCodes.Status500InternalServerError,
                    "Server Error",
                    "Server error",
                    "An unexpected error has occurred",
                    null
                    )
            };
        }
        public class CustomProblemDetail : ProblemDetails
        {
            public string CorrelationId { get; set; }
        }
        internal record ExceptionDetails(
            int Status,
            string Type,
            string Title,
            string Details,
            IEnumerable<object> Errors
            );
        public static string GetInnerException(Exception ex)
        {
            if (ex.InnerException != null)
            {
                return string.Format("{0} > {1} ", ex.InnerException.Message, GetInnerException(ex.InnerException));
            }
            return string.Empty;
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<ErrorHandlingMiddleware> logger, PropagateHeaders _propagateHeaders)
        {
            logger.LogError($"{ex.Message}");
            var CorrelationId = _propagateHeaders.Headers.Where(x => x.Key == HeadersConstant.correlationId).FirstOrDefault().Value?.ToString();
            var errorResponse = new ServerErrorResponseModel
            {
                status = 500,
                type = "General Exception",
                message = ex.Message,
                correlationId = CorrelationId,
                exception = new ExceptionResponse
                {
                    message = ex.Message,
                    inner = GetInnerException(ex)
                }
            };

            var result = JsonConvert.SerializeObject(errorResponse,
            Newtonsoft.Json.Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            return context.Response.WriteAsync(result);
        }

        private static Task HandleSqlExceptionAsync(HttpContext context, SqlException ex, PropagateHeaders _propagateHeaders)
        {
            var correlationId = _propagateHeaders.Headers.Where(x => x.Key == HeadersConstant.correlationId).FirstOrDefault().Value?.ToString();
            var errorList = new List<Object>();

            for (int i = 0; i < ex.Errors.Count; i++)
            {
                errorList.Add(new
                {

                    Message = ex.Errors[i].Message,
                    Procedure = ex.Errors[i].Procedure,
                    LineNumber = ex.Errors[i].LineNumber,
                    Source = ex.Errors[i].Source,
                    Server = ex.Errors[i].Server
                });
            }

            var result = JsonConvert.SerializeObject(new
            {
                Type = "SQL Exception",
                Exceptions = errorList,
                CorrelationId = correlationId
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            return context.Response.WriteAsync(result);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            // var serviceCollection = new Microsoft.Extensions.DependencyInjection.ServiceCollection();

            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
