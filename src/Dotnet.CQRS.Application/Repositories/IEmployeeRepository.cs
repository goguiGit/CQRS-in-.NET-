using Dotnet.CQRS.Domain;

namespace Dotnet.CQRS.Application.Repositories;

public interface IEmployeeRepository : IRepository<Employee, int>;