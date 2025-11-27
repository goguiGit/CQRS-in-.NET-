namespace Dotnet.CQRS.Infrastructure.Repositories;

public class EmployeeRepository(ApplicationDbContext context) : RepositoryBase<Employee, int>(context), IEmployeeRepository;