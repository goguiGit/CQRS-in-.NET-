using System.Linq.Expressions;
using Ardalis.Result;
using AutoFixture;
using AutoFixture.AutoMoq;
using Dotnet.CQRS.Application.Employees.Queries.GetById;
using Dotnet.CQRS.Application.Repositories;
using Dotnet.CQRS.Domain;
using Moq;
using Shouldly;

namespace Dotnet.CQRS.Application.Tests.Employees.Queries.GetById;

public class When_NotFound
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
            .ReturnsAsync((Employee)null!);

        var query = new GetByIdQuery(employeeId);
        var handler = fixture.Create<GetByIdQueryHandler>();

        // Act
        var expectedResult = await handler.Handle(query, CancellationToken.None);

        // Assert
        // Assert
        expectedResult.ShouldNotBeNull();
        expectedResult.IsSuccess.ShouldBeFalse();
        expectedResult.Status.ShouldBe(ResultStatus.NotFound);
        expectedResult.Value.ShouldBeNull();

    }
}