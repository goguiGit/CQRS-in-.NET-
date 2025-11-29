using Dotnet.CQRS.Application.Repositories;

namespace Dotnet.CQRS.Application.Employees.Queries.GetAll;

public class GetAllQueryHandler(IEmployeeRepository employeeRepository)
    : IQueryHandler<GetAllQuery, Result<List<GetAllResponse>>>
{
    private readonly IEmployeeRepository _employeeRepository =
        employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));

    public async Task<Result<List<GetAllResponse>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.GetAllAsync();
        var response = employees.Select(e => new GetAllResponse(e.Id, e.FullName, e.Email)).ToList();
        return Result.Success(response);
    }
}