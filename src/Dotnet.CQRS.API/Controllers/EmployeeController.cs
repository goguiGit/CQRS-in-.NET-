using Ardalis.Result;
using Ardalis.Result.AspNetCore;
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
    public async Task<Result<GetByIdResponse>> GetEmployee(int id)
    {
        var query = new GetByIdQuery(id);
        var result = await _mediator.Send(query);
        return result;
    }
    
}

