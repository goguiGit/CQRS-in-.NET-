namespace Dotnet.CQRS.Application.Employees.Queries.GetById;

public record GetByIdResponse(int Id, string Name, string Email);