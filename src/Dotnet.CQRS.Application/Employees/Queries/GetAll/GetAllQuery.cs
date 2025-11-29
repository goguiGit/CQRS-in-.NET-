namespace Dotnet.CQRS.Application.Employees.Queries.GetAll;

public record GetAllQuery : IQuery<Result<List<GetAllResponse>>>;