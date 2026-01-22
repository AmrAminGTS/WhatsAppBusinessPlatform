using MediatAmR.Abstractions;

namespace WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
public interface IQuery<out TResponse> : IRequest<TResponse>;
