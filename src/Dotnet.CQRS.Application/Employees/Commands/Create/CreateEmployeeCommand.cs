namespace Dotnet.CQRS.Application.Employees.Commands.Create;

public record CreateEmployeeCommand(string FirstName, string LastName, string Email) : ICommand<Result>;