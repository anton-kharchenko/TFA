﻿namespace TFA.Forums.Domain.Exceptions;

public class ForumNotFoundException(Guid forumId) : DomainException(410, $"Forum with id {forumId} not found.");