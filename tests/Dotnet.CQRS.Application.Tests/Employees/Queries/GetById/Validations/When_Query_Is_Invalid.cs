using Dotnet.CQRS.Application.Employees.Queries.GetById;
using Shouldly;

namespace Dotnet.CQRS.Application.Tests.Employees.Queries.GetById.Validations;

public class When_Query_Is_Invalid
{
    [Fact]
    public async Task Then_Ok()
    {

        // Arrange
        var query = new GetByIdQuery(0);
        var validator = new GetByIdQueryValidator();

        // Act
        var result = await validator.ValidateAsync(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].PropertyName.ShouldBe(nameof(GetByIdQuery.Id));

    }
}