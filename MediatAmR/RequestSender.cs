using System.Reflection;
using MediatAmR.Abstractions;

namespace MediatAmR;
public class RequestSender : IRequestSender
{
    private readonly IServiceProvider _serviceProvider;
    public RequestSender(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        Type handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        object handler = _serviceProvider
            .GetService(handlerType) ?? throw new InvalidOperationException($"Handler of type '{handlerType}' not found.");

        MethodInfo? handleMethod = handlerType.GetMethod("Handle");
        ArgumentNullException.ThrowIfNull(handleMethod);
        return await (Task<TResponse>)handleMethod.Invoke(handler, [request, cancellationToken])!;
    }
}
