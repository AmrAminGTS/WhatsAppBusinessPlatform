namespace MediatAmR.Abstractions;
#pragma warning restore S2326 // Unused type parameters should be removed
public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
}
