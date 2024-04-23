using FluentValidation;
using TFA.Forums.Domain.Commands.GetTopics;
using TFA.Forums.Domain.Keys;

namespace TFA.Forums.Domain.Validations.Queries.GetTopics;

internal class GetTopicQueryValidator : AbstractValidator<GetTopicsQuery>
{
    public GetTopicQueryValidator()
    {
        RuleFor(q => q.ForumId).NotEmpty().WithErrorCode(ValidationErrorCodeKeys.Empty);
        RuleFor(q => q.Skip).GreaterThanOrEqualTo(0).WithErrorCode(ValidationErrorCodeKeys.Invalid);
        RuleFor(q => q.Take).GreaterThanOrEqualTo(0).WithErrorCode(ValidationErrorCodeKeys.Invalid);
    }
}