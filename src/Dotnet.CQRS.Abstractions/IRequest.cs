namespace Dotnet.CQRS.Abstractions;

/// <summary>
/// Marker interface to represent a request with a void response
/// </summary>
public interface IRequest : IBaseRequest;

/// <summary>
/// Marker interface to represent a request with a response
/// </summary>
/// <typeparam name="TResponse">Response type</typeparam>
public interface IRequest<out TResponse> : IBaseRequest;