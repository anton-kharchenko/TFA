using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TFA.Forums.Domain.Exceptions;

namespace TFA.Api.Extensions;

public static class ProblemDetailsFactoryExtensions
{
    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, HttpContext httpContext,
        IntentionManagerException intentionManagerException)
    {
        return factory.CreateProblemDetails(httpContext, StatusCodes.Status403Forbidden, "Authorization failed",
            intentionManagerException.Message);
    }

    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, HttpContext httpContext,
        DomainException domainException)
    {
        return factory.CreateProblemDetails(httpContext, StatusCodes.Status403Forbidden, "Domain exception",
            domainException.Message);
    }

    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, HttpContext httpContext,
        ValidationException validationException)
    {
        var modelState4Dictionary = new ModelStateDictionary();

        foreach (var error in validationException.Errors)
            modelState4Dictionary.AddModelError(error.PropertyName, error.ErrorCode);

        return factory.CreateValidationProblemDetails(httpContext, modelState4Dictionary,
            StatusCodes.Status403Forbidden, "Validation error",
            validationException.Message);
    }
}