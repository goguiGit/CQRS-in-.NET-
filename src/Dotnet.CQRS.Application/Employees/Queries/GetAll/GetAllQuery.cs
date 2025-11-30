using Dotnet.CQRS.Abstractions;
using MediatR;

namespace Dotnet.CQRS.Application.Employees.Queries.GetAll;

public record GetAllQuery : IQuery<Result<List<GetAllResponse>>>, IRequest<Result<List<GetAllResponse>>>;
