using Common;
using Common.Constant;
using Core.Configs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;


namespace Infastructure.Authentication
{
    public class CustomAuthorizationResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

        public async Task HandleAsync(
            RequestDelegate next,
            HttpContext context,
            AuthorizationPolicy policy,
            PolicyAuthorizationResult authorizeResult)
        {
            if (authorizeResult.Challenged)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";

                var propelHeaders = context.RequestServices.GetService<PropagateHeaders>();
                var correlationId = propelHeaders?.Headers?.FirstOrDefault(x => x.Key == HeadersConstant.correlationId).Value?.ToString() ?? Guid.NewGuid().ToString();

                var response = new AuthErrorResponseModel
                {
                    status = 401,
                    type = "Unauthorized Access",
                    message = "You are not authorized to access this resource.",
                    correlationId = correlationId
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            if (authorizeResult.Forbidden)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.Response.ContentType = "application/json";

                var propelHeaders = context.RequestServices.GetService<PropagateHeaders>();
                var correlationId = propelHeaders?.Headers?.FirstOrDefault(x => x.Key == HeadersConstant.correlationId).Value?.ToString() ?? Guid.NewGuid().ToString();

                var response = new AuthErrorResponseModel
                {
                    status = 403,
                    type = "Forbidden Access",
                    message = "You do not have permission to access this resource.",
                    correlationId = correlationId
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            // Fallback to default handler if not challenged or forbidden
            await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}
