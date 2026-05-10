using Common;
using Common.Constant;
using Infastructure.Header;
using Cortex.Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using Serilog.Extensions.Hosting;
using System.Net.Http;
using System.Text.Json;
using Cortex.Mediator.Commands;
using System.Windows.Input;

namespace Infastructure.Behaviour.RequestLoggingPipeline
{
    public class RequestLoggingPipelineBehavior<TCommand, TResponse> : BaseRequestLoggingBehavior, ICommandPipelineBehavior<TCommand, TResponse> where TCommand : class, ICommand<TResponse> where TResponse : class
    {
        public RequestLoggingPipelineBehavior(IHttpContextAccessor httpContextAccessor, PropagateHeaders propagateHeaders) : base(httpContextAccessor, propagateHeaders)
        {
        }
        public async Task<TResponse> Handle(TCommand command, CommandHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            LogRequest(command);
            TResponse response = await next();
            LogResponse(response);
            return response;
        }
    }

}
