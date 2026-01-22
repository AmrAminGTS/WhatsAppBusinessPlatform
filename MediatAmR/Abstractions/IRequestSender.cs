namespace MediatAmR.Abstractions;

public interface IRequestSender 
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
