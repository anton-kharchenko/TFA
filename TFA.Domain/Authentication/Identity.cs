﻿using TFA.Domain.Interfaces.Authentication;

namespace TFA.Domain.Authentication;

public class User(Guid userId) : IIdentity
{
    public Guid UserId { get; } = userId;

    public static User Guest => new(Guid.Empty);
}