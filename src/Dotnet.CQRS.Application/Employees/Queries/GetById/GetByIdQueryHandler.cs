using Dotnet.CQRS.Abstractions;
using MediatR;
using Dotnet.CQRS.Application.Repositories;

namespace Dotnet.CQRS.Application.Employees.Queries.GetById;

public class GetByIdQueryHandler(IEmployeeRepository employeeRepository) : IQueryHandler<GetByIdQuery, Result<GetByIdResponse>>, IRequestHandler<GetByIdQuery, Result<GetByIdResponse>>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));

    public async Task<Result<GetByIdResponse>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.SingleOrDefaultAsync(e => e.Id == request.Id);
        
        if (employee is null)
        {
            return Result.NotFound();
        }

        return Result.Success(new GetByIdResponse(employee.Id, employee.FullName, employee.Email));
    }
}