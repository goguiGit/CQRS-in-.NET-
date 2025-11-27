using Dotnet.CQRS.Application.Repositories;
using Dotnet.CQRS.MediatR.Abstractions;

namespace Dotnet.CQRS.Application.Employees.Queries.GetById;

public class GetByIdQueryHandler(IEmployeeRepository employeeRepository) : IQueryHandler<GetByIdQuery, GetByIdResponse>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));

    public async Task<GetByIdResponse> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.SingleOrDefaultAsync(e => e.Id == request.Id);

        if (employee is null)
        {
            throw new KeyNotFoundException($"Employee with ID {request.Id} not found.");
        }

        return new GetByIdResponse(employee.Id, employee.FullName, employee.Email);
    }
}