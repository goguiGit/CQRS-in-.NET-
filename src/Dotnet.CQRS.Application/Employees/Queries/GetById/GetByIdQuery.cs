using Dotnet.CQRS.Abstractions;
using MediatR;

namespace Dotnet.CQRS.Application.Employees.Queries.GetById;

public record GetByIdQuery(int Id) : IQuery<Result<GetByIdResponse>>, IRequest<Result<GetByIdResponse>>;
