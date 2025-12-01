using Dotnet.CQRS.Abstractions;

namespace Dotnet.CQRS.Application.Employees.Queries.GetById;

public record GetByIdQuery(int Id) : IQuery<Result<GetByIdResponse>>;
