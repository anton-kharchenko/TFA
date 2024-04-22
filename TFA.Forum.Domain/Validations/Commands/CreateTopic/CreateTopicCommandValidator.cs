﻿using FluentValidation;
using TFA.Forum.Domain.Commands.CreateTopic;
using TFA.Forum.Domain.Keys;

namespace TFA.Forum.Domain.Validations.Commands.CreateTopic;

internal class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
{
    public CreateTopicCommandValidator()
    {
        RuleFor(c => c.ForumId).NotEmpty().WithErrorCode(ValidationErrorCodeKeys.Empty);
        RuleFor(c => c.Title).Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode(ValidationErrorCodeKeys.Empty)
            .MaximumLength(100).WithErrorCode(ValidationErrorCodeKeys.TooLong);
    }
}