using Dotnet.CQRS.Application.Employees.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.CQRS.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));


    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<GetByIdResponse>> GetEmployee(int id)
    {
        var query = new GetByIdQuery(id);
        var employee = await _mediator.Send(query);
        return Ok(employee);
    }
    
}

