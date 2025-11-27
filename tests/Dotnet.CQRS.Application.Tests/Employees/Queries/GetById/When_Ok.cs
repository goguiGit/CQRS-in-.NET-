using AutoFixture;
using AutoFixture.AutoMoq;
using Dotnet.CQRS.Application.Employees.Queries.GetById;
using Dotnet.CQRS.Application.Repositories;
using Moq;
using System.Linq.Expressions;
using Dotnet.CQRS.Domain;
using Shouldly;

namespace Dotnet.CQRS.Application.Tests.Employees.Queries.GetById;

public class When_Ok
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

        var query = new GetByIdQuery(employeeId);
        var handler = fixture.Create<GetByIdQueryHandler>();
        
        // Act
        var expectedResult = await handler.Handle(query, CancellationToken.None);

        // Assert
        expectedResult.ShouldNotBeNull();
        expectedResult.Id.ShouldBe(employeeId);

    }
}