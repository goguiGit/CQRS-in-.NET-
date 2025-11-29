using System.Linq.Expressions;
using AutoFixture;
using AutoFixture.AutoMoq;
using Dotnet.CQRS.Application.Employees.Commands.Create;
using Dotnet.CQRS.Application.Repositories;
using Dotnet.CQRS.Domain;
using Moq;
using Shouldly;

namespace Dotnet.CQRS.Application.Tests.Employees.Commands.Create;

public class When_Email_Already_Exists
{
    [Fact]
    public async Task Then_Ok()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        var employeeRepository = fixture.Freeze<Mock<IEmployeeRepository>>();

        const int employeeId = 42;
        employeeRepository
            .Setup(e => e.SingleOrDefaultAsync(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(new Employee("Obiwan", "Kenobi", "obiwan@gmail.com")
            {
                Id = employeeId
            });

        var command = fixture.Create<CreateEmployeeCommand>();
        var handler = fixture.Create<CreateEmployeeCommandHandler>();
        
        // Act
        var expectedResult = await handler.Handle(command, CancellationToken.None);

        // Assert
        expectedResult.IsSuccess.ShouldBeFalse();
        
    }
}