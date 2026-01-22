using MediatAmR.Abstractions;

namespace WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand,TResponse>
    where TCommand : ICommand<TResponse>;

