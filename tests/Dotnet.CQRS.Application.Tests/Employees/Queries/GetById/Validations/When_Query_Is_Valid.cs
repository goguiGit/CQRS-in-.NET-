using Dotnet.CQRS.Application.Employees.Queries.GetById;
using Shouldly;

namespace Dotnet.CQRS.Application.Tests.Employees.Queries.GetById.Validations;

public class When_Query_Is_Valid
{
    [Fact]
    public async Task Then_Ok()
    {

        // Arrange
        var query = new GetByIdQuery(42);
        var validator = new GetByIdQueryValidator();

        // Act
        var result = await validator.ValidateAsync(query, CancellationToken.None);
            
        // Assert
        result.ShouldNotBeNull();
        result.IsValid.ShouldBeTrue();

    }
}