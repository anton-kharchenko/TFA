using FluentValidation;
using TFA.Forum.Domain.Commands.GetTopics;
using TFA.Forum.Domain.Keys;

namespace TFA.Forum.Domain.Validations.Queries.GetTopics;

internal class GetTopicQueryValidator : AbstractValidator<GetTopicsQuery>
{
    public GetTopicQueryValidator()
    {
        RuleFor(q => q.ForumId).NotEmpty().WithErrorCode(ValidationErrorCodeKeys.Empty);
        RuleFor(q => q.Skip).GreaterThanOrEqualTo(0).WithErrorCode(ValidationErrorCodeKeys.Invalid);
        RuleFor(q => q.Take).GreaterThanOrEqualTo(0).WithErrorCode(ValidationErrorCodeKeys.Invalid);
    }
}