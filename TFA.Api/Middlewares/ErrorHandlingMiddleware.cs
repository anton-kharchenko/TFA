using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TFA.Api.Extensions;
using TFA.Forum.Domain.Exceptions;

namespace TFA.Api.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext httpContext,
        ILogger<ErrorHandlingMiddleware> logger,
        ProblemDetailsFactory problemDetailsFactory)
    {
        try
        {
            logger.LogInformation("Error handling started for request in the path {RequestPath}",
                httpContext.Request.Path.Value);

            await next.Invoke(httpContext);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error has happened with {RequestPath}, the message is {ErrorMessage}",
                httpContext.Request.Path.Value, exception.Message);
            ProblemDetails problemDetails;
            switch (exception)
            {
                case IntentionManagerException intentionManagerException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, intentionManagerException);
                    break;
                case ValidationException validationException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, validationException);
                    logger.LogInformation(validationException, "Invalid body request!");
                    break;
                case DomainException domainException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, domainException);
                    logger.LogError(domainException, "Domain exception occured");
                    break;
                default:
                    problemDetails = problemDetailsFactory.CreateProblemDetails(httpContext,
                        StatusCodes.Status500InternalServerError, "Unhandled error! Please contact us.");
                    logger.LogError(exception, "Unhandled error! Please contact us.");
                    break;
            }

            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, problemDetails.GetType());
        }
    }
}