using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Dotnet.CQRS.Application.Employees.Commands.Create;
using Dotnet.CQRS.Application.Employees.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.CQRS.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [TranslateResultToActionResult]
    [HttpGet]
    [Route("{id:int}")]
    public async Task<Result<GetByIdResponse>> GetByIdAsync(int id)
    {
        var query = new GetByIdQuery(id);
        var result = await _mediator.Send(query);
        return result;
    }
    
    [HttpGet]
    [TranslateResultToActionResult]
    [Route("")]
    public async Task<Result<List<GetAllResponse>>> GetAllAsync()
    {
        var query = new GetAllQuery();
        var result = await _mediator.Send(query);
        return result;
    }

    [HttpPost]
    [TranslateResultToActionResult]
    [Route("")]
    public async Task<Result> CreateAsync([FromBody] CreateEmployeeCommand command)
    {
        var result = await _mediator.Send(command);
        return result;
    }
}

