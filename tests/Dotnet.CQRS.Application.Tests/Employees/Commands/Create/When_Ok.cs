using AutoFixture;
using AutoFixture.AutoMoq;
using Dotnet.CQRS.Application.Repositories;
using Dotnet.CQRS.Domain;
using Moq;
using System.Linq.Expressions;
using Dotnet.CQRS.Application.Employees.Commands.Create;
using Dotnet.CQRS.Application.Interfaces;
using Shouldly;

namespace Dotnet.CQRS.Application.Tests.Employees.Commands.Create;

public class When_Ok
{
    [Fact]
    public async Task Then_Ok()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        var employeeRepository = fixture.Freeze<Mock<IEmployeeRepository>>();

        employeeRepository
            .Setup(e => e.SingleOrDefaultAsync(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync((Employee)null!);

        var unitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();

        var command = new CreateEmployeeCommand("Obiwan", "Kenobi", "obiwan@gmail.com");
        var handler = fixture.Create<CreateEmployeeCommandHandler>();

        // Act
        var expectedResult = await handler.Handle(command, CancellationToken.None);

        // Assert
        expectedResult.IsSuccess.ShouldBeTrue();
        unitOfWork.Verify(u => u.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Once);

    }
}