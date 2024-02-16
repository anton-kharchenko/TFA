using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TFA.Api.Extensions;
using TFA.Domain.Exceptions;

namespace TFA.Api.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext, ProblemDetailsFactory problemDetailsFactory)
    {
        try
        {
            await next.Invoke(httpContext);
        }
        catch (Exception exception)
        {
            var problemDetails = exception switch
            {
                IntentionManagerException intentionManagerException => problemDetailsFactory.CreateFrom(httpContext, intentionManagerException),
                DomainException domainException => problemDetailsFactory.CreateFrom(httpContext, domainException),
                ValidationException validationException => problemDetailsFactory.CreateFrom(httpContext, validationException),
                _ => problemDetailsFactory.CreateProblemDetails(httpContext, StatusCodes.Status500InternalServerError, "Unhandled error", exception.Message)
            };
            
            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}