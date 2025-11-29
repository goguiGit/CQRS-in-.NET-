using System.Net;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Dotnet.CQRS.API.Configuration;

public class ExceptionHandlingMiddleware(RequestDelegate next, ProblemDetailsFactory problemDetailsFactory)
{
    private const string ContentType = "application/problem+json";

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException vex)
        {
            var details = problemDetailsFactory.CreateProblemDetails(
                context,
                statusCode: (int)HttpStatusCode.BadRequest,
                title: "Request validation failed",
                detail: "One or more validation errors occurred.");

            details.Extensions["errors"] = vex.Errors.Select(e => new
            {
                e.PropertyName,
                e.ErrorMessage,
                e.ErrorCode,
                e.AttemptedValue
            });

            context.Response.ContentType = ContentType;
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(details);
        }
        catch (KeyNotFoundException kex)
        {
            var details = problemDetailsFactory.CreateProblemDetails(
                context,
                statusCode: (int)HttpStatusCode.NotFound,
                title: "Resource not found",
                detail: kex.Message);

            context.Response.ContentType = ContentType;
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsJsonAsync(details);
        }
        catch (Exception ex)
        {
            var traceId = context.TraceIdentifier;
            var details = problemDetailsFactory.CreateProblemDetails(
                context,
                statusCode: (int)HttpStatusCode.InternalServerError,
                title: "An unexpected error occurred",
                detail: "Please contact support and provide the trace identifier.",
                instance: context.Request.Path);

            details.Extensions["traceId"] = traceId;

            context.Response.ContentType = ContentType;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(details);
        }
    }
}
