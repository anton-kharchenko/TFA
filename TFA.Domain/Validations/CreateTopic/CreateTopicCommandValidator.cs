﻿using FluentValidation;
using TFA.Domain.Commands.CreateTopic;

namespace TFA.Domain.Validations.CreateTopic;

public class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
{
    public CreateTopicCommandValidator()
    {
        RuleFor(c => c.ForumId).NotEmpty().WithErrorCode("Empty");
        RuleFor(c => c.Title).Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode("Empty")
            .MaximumLength(100).WithErrorCode("TooLong");
    }
}