using Dotnet.CQRS.Abstractions;
using Dotnet.CQRS.Application.Interfaces;
using Dotnet.CQRS.Application.Repositories;
using Dotnet.CQRS.Domain;

namespace Dotnet.CQRS.Application.Employees.Commands.Create;

public class CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork) 
    : ICommandHandler<CreateEmployeeCommand, Result>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<Result> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employeeDb = await _employeeRepository.SingleOrDefaultAsync(e => e.Email == request.Email);

        if (employeeDb is not null)
        {
            return Result.Invalid(new ValidationError("An employee with the same email already exists."));
        }

        var employee = new Employee(request.FirstName, request.LastName, request.Email);
        _employeeRepository.Add(employee);
        await _unitOfWork.SaveEntitiesAsync(cancellationToken);
        return Result.Success();
    }
}