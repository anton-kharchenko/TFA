using FluentValidation;
using TFA.Domain.Commands.GetTopics;
using TFA.Domain.Keys;

namespace TFA.Domain.Validations.GetTopics;

internal class GetTopicQueryValidator : AbstractValidator<GetTopicsQuery>
{
    public GetTopicQueryValidator()
    {
        RuleFor(q => q.ForumId).NotEmpty().WithErrorCode(ValidationErrorCodeKeys.Empty);
        RuleFor(q => q.Skip).GreaterThanOrEqualTo(0).WithErrorCode(ValidationErrorCodeKeys.Invalid);
        RuleFor(q => q.Take).GreaterThanOrEqualTo(0).WithErrorCode(ValidationErrorCodeKeys.Invalid);
    }
}