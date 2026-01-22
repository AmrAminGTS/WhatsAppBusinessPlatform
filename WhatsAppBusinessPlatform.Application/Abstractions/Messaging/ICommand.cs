using MediatAmR.Abstractions;

namespace WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
public interface ICommand;
#pragma warning disable S2326 // Unused type parameters should be removed
public interface ICommand<out TResponse> : IRequest<TResponse>;
#pragma warning restore S2326 // Unused type parameters should be removed
