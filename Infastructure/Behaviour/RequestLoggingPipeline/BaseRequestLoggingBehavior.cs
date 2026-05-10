using Common;
using Common.Constant;
using Infastructure.Header;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Text.Json;

namespace Infastructure.Behaviour.RequestLoggingPipeline
{
    public class BaseRequestLoggingBehavior(IHttpContextAccessor _httpContextAccessor,PropagateHeaders _propagateHeaders)
    {

        protected void LogRequest<TRequest>(TRequest request) where TRequest : class
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var diagnosticContext = httpContext?.RequestServices.GetService<IDiagnosticContext>();
            JsonSerializerOptions options = new JsonSerializerOptions();
            string requestJson = JsonSerializer.Serialize(request, options);
            diagnosticContext?.Set("Request", requestJson);
        }
        protected void LogResponse<TResponse>(TResponse response) where TResponse : class
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var diagnosticContext = httpContext?.RequestServices.GetService<IDiagnosticContext>();

            JsonSerializerOptions options = new JsonSerializerOptions();

            if (response is BaseResponseModel baseResponse)
            {
                var correlationId = _propagateHeaders.Headers
                    .Where(x => x.Key == HeadersConstant.correlationId)
                    .FirstOrDefault().Value?.ToString();
                baseResponse.CorrelationId = correlationId;
            }

            string responseJson = JsonSerializer.Serialize(response, options);
            diagnosticContext?.Set("Response", responseJson);
        }
    }
}
